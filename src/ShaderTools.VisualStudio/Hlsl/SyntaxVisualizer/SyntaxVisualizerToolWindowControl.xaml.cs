﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using ShaderTools.Core.Text;
using ShaderTools.Hlsl.Syntax;
using ShaderTools.VisualStudio.Core.Text;
using ShaderTools.VisualStudio.Core.Util;
using ShaderTools.VisualStudio.Core.Util.Extensions;
using ShaderTools.VisualStudio.Hlsl.Util.Extensions;

namespace ShaderTools.VisualStudio.Hlsl.SyntaxVisualizer
{
    public sealed partial class SyntaxVisualizerToolWindowControl : UserControl, IVsRunningDocTableEvents, IDisposable
    {
        private readonly TimeSpan _typingTimerTimeout = TimeSpan.FromMilliseconds(300.0);

        private IVsRunningDocumentTable _runningDocumentTable;
        private uint _runningDocumentTableCookie;

        private IWpfTextView _activeWpfTextView;
        private SyntaxTree _activeSyntaxTree;

        private DispatcherTimer _typingTimer;

        private bool _isNavigatingFromSourceToTree;
        private bool _isNavigatingFromTreeToSource;

        private IVsRunningDocumentTable RunningDocumentTable
        {
            get
            {
                if (_runningDocumentTable == null)
                    _runningDocumentTable = Core.Util.Extensions.ServiceProviderExtensions.GetGlobalService<SVsRunningDocumentTable, IVsRunningDocumentTable>();
                return _runningDocumentTable;
            }
        }

        public SyntaxVisualizerToolWindowControl()
        {
            InitializeComponent();
            InitializeRunningDocTable();
        }

        private void InitializeRunningDocTable()
        {
            RunningDocumentTable?.AdviseRunningDocTableEvents(this, out _runningDocumentTableCookie);
        }

        int IVsRunningDocTableEvents.OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        int IVsRunningDocTableEvents.OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        int IVsRunningDocTableEvents.OnAfterSave(uint docCookie)
        {
            return VSConstants.S_OK;
        }

        int IVsRunningDocTableEvents.OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        int IVsRunningDocTableEvents.OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            if (IsVisible && fFirstShow == 0)
            {
                var wpfTextView = pFrame.GetWpfTextView();
                if (wpfTextView != null)
                {
                    var contentType = wpfTextView.TextBuffer.ContentType;
                    if (contentType.IsOfType(HlslConstants.ContentTypeName))
                    {
                        Clear();
                        _activeWpfTextView = wpfTextView;
                        _activeWpfTextView.Selection.SelectionChanged += HandleSelectionChanged;
                        _activeWpfTextView.TextBuffer.Changed += HandleTextBufferChanged;
                        _activeWpfTextView.LostAggregateFocus += HandleTextViewLostFocus;
                        RefreshSyntaxVisualizer();
                    }
                }
            }
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            if (IsVisible && _activeWpfTextView != null && pFrame.GetWpfTextView() == _activeWpfTextView)
                Clear();
            return VSConstants.S_OK;
        }

        void IDisposable.Dispose()
        {
            if (_runningDocumentTableCookie == 0)
                return;
            _runningDocumentTable.UnadviseRunningDocTableEvents(_runningDocumentTableCookie);
            _runningDocumentTableCookie = 0;
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            HandleTextBufferChanged(sender, e);
        }

        private void OnControlUnloaded(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            if (_typingTimer != null)
            {
                _typingTimer.Stop();
                _typingTimer.Tick -= HandleTypingTimerTimeout;
                _typingTimer = null;
            }
            if (_activeWpfTextView != null)
            {
                _activeWpfTextView.Selection.SelectionChanged -= HandleSelectionChanged;
                _activeWpfTextView.TextBuffer.Changed -= HandleTextBufferChanged;
                _activeWpfTextView.LostAggregateFocus -= HandleTextViewLostFocus;
                _activeWpfTextView = null;
            }
            _activeSyntaxTree = null;
            TreeView.Items.Clear();
        }

        private void HandleTextBufferChanged(object sender, EventArgs e)
        {
            if (_typingTimer == null)
            {
                _typingTimer = new DispatcherTimer();
                _typingTimer.Interval = _typingTimerTimeout;
                _typingTimer.Tick += HandleTypingTimerTimeout;
            }
            _typingTimer.Stop();
            _typingTimer.Start();
        }

        private void HandleTypingTimerTimeout(object sender, EventArgs e)
        {
            _typingTimer.Stop();
            RefreshSyntaxVisualizer();
        }

        private void HandleSelectionChanged(object sender, EventArgs e)
        {
            NavigateFromSource();
        }

        private void HandleTextViewLostFocus(object sender, EventArgs e)
        {
            _typingTimer?.Stop();
        }

        private async void RefreshSyntaxVisualizer()
        {
            if (!IsVisible || _activeWpfTextView == null)
                return;

            var currentSnapshot = _activeWpfTextView.TextBuffer.CurrentSnapshot;
            var contentType = currentSnapshot.ContentType;
            if (!contentType.IsOfType(HlslConstants.ContentTypeName))
                return;

            try
            {
                _activeSyntaxTree = await Task.Run(() => currentSnapshot.GetSyntaxTree(CancellationToken.None));
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to get syntax tree for syntax visualizer: " + ex);
                return;
            }
            
            DisplaySyntaxTree();
            NavigateFromSource();
        }

        private void NavigateFromSource()
        {
            if (!IsVisible || _activeWpfTextView == null)
                return;
            var snapshotSpan = _activeWpfTextView.Selection.StreamSelectionSpan.SnapshotSpan;
            NavigateToBestMatch(snapshotSpan);
        }

        private void DisplaySyntaxTree()
        {
            TreeView.Items.Clear();
            AddNode(TreeView.Items, _activeSyntaxTree.Root, string.Empty);
        }

        private static void AddNode(ItemCollection items, SyntaxNode node, string prefix)
        {
            var tooltip = string.Join(Environment.NewLine, node.GetDiagnostics().Select(x => x.ToString()));
            var treeViewItem = new TreeViewItem
            {
                Background = (node.ContainsDiagnostics) ? Brushes.Pink : Brushes.Transparent,
                Foreground = (node.IsToken) ? Brushes.DarkGreen : (node.Ancestors().Any(a => a.IsToken) ? Brushes.DarkRed : Brushes.Blue),
                Header = $"{prefix}{node.Kind} [{node.SourceRange.Start}..{node.SourceRange.End})",
                ToolTip = string.IsNullOrEmpty(tooltip) ? null : tooltip,
                Tag = node
            };

            foreach (var childNode in node.ChildNodes)
                AddNode(treeViewItem.Items, childNode, string.Empty);

            if (node.IsToken)
            {
                var token = (SyntaxToken) node;
                foreach (var childNode in token.LeadingTrivia)
                    AddNode(treeViewItem.Items, childNode, "Lead: ");
                foreach (var childNode in token.TrailingTrivia)
                    AddNode(treeViewItem.Items, childNode, "Trail: ");
            }

            items.Add(treeViewItem);
        }

        private void NavigateToBestMatch(SnapshotSpan snapshotSpan)
        {
            if (!_isNavigatingFromTreeToSource && _activeSyntaxTree != null)
            {
                _isNavigatingFromSourceToTree = true;
                var sourceRange = _activeSyntaxTree.MapRootFileRange(new TextSpan(snapshotSpan.Snapshot.ToSourceText(), snapshotSpan.Span.Start, snapshotSpan.Span.Length));
                NavigateToBestMatch((TreeViewItem) TreeView.Items[0], sourceRange);
                _isNavigatingFromSourceToTree = false;
            }
        }

        private bool NavigateToBestMatch(TreeViewItem treeViewItem, SourceRange span)
        {
            var currentNode = (SyntaxNode) treeViewItem.Tag;
            if (currentNode.FullSourceRange.Contains(span))
            {
                CollapseEverythingBut(treeViewItem);

                foreach (TreeViewItem childItem in treeViewItem.Items)
                    if (NavigateToBestMatch(childItem, span))
                        break;
                return true;
            }
            return false;
        }

        private void CollapseEverythingBut(TreeViewItem item)
        {
            if (item == null)
                return;
            DeepCollapse((TreeViewItem) TreeView.Items[0]);
            ExpandPathTo(item);
            item.IsSelected = true;
            item.BringIntoView();
        }

        private static void DeepCollapse(TreeViewItem item)
        {
            if (item == null)
                return;
            item.IsExpanded = false;
            foreach (TreeViewItem treeViewItem in item.Items)
                DeepCollapse(treeViewItem);
        }

        private static void ExpandPathTo(TreeViewItem item)
        {
            if (item == null)
                return;
            item.IsExpanded = true;

            if (item.Parent is TreeViewItem)
                ExpandPathTo((TreeViewItem) item.Parent);
        }

        private void OnTreeViewSelectedItemChanged(object sender, EventArgs e)
        {
            _isNavigatingFromTreeToSource = true;

            var newSelectedItem = (TreeViewItem) TreeView.SelectedItem;
            var syntaxNode = newSelectedItem?.Tag as SyntaxNode;

            PropertyGrid.SelectedObject = syntaxNode;

            if (!_isNavigatingFromSourceToTree && syntaxNode != null)
                NavigateToSource(syntaxNode.GetTextSpanRoot());

            _isNavigatingFromTreeToSource = false;
        }

        private void NavigateToSource(TextSpan span)
        {
            if (!IsVisible || _activeWpfTextView == null || !span.IsInRootFile || span == TextSpan.None)
                return;
            SnapshotSpan snapshotSpan = new SnapshotSpan(_activeWpfTextView.TextBuffer.CurrentSnapshot, span.Start, span.Length);
            _activeWpfTextView.Selection.Select(snapshotSpan, false);
            _activeWpfTextView.ViewScroller.EnsureSpanVisible(snapshotSpan);
        }
    }
}