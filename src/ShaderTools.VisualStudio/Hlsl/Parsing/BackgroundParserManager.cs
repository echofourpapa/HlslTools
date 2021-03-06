﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using ShaderTools.VisualStudio.Core.Parsing;
using ShaderTools.VisualStudio.Hlsl.Util.Extensions;

namespace ShaderTools.VisualStudio.Hlsl.Parsing
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType(HlslConstants.ContentTypeName)]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal sealed class BackgroundParserManager : BackgroundParserManagerBase
    {
        protected override BackgroundParserBase GetBackgroundParser(ITextBuffer textBuffer)
        {
            return textBuffer.GetBackgroundParser();
        }
    }
}