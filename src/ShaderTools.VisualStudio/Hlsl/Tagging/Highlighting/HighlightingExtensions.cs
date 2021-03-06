﻿using System.Collections.Generic;
using System.Linq;
using ShaderTools.Hlsl.Compilation;
using ShaderTools.Hlsl.Syntax;
using ShaderTools.VisualStudio.Hlsl.Tagging.Highlighting.Highlighters;

namespace ShaderTools.VisualStudio.Hlsl.Tagging.Highlighting
{
    internal static class HighlightingExtensions
    {
        public static IEnumerable<HighlightSpan> GetHighlights(this SemanticModel semanticModel, SourceLocation position, IEnumerable<IHighlighter> highlighters)
        {
            var result = new List<HighlightSpan>();

            foreach (var highlighter in highlighters)
            {
                result.AddRange(highlighter
                    .GetHighlights(semanticModel, position)
                    .Where(x => x.Span.IsInRootFile));
            }

            return result;
        }
    }
}