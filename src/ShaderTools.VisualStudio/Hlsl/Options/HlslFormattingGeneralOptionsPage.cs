﻿using System;
using ShaderTools.VisualStudio.Core.Options.Views;
using ShaderTools.VisualStudio.Hlsl.Options.Views;

namespace ShaderTools.VisualStudio.Hlsl.Options
{
    internal sealed class HlslFormattingGeneralOptionsPage : HlslOptionsPageBase<GeneralOptions>
    {
        protected override OptionsControlBase CreateControl(IServiceProvider serviceProvider) => new HlslFormattingGeneralOptionsControl(Options);
    }

    internal class GeneralOptions
    {
        public bool AutomaticallyFormatStatementOnSemicolon { get; set; } = true;
        public bool AutomaticallyFormatBlockOnCloseBrace { get; set; } = true;
        public bool AutomaticallyFormatOnPaste { get; set; } = true;
    }
}