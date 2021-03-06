﻿namespace ShaderTools.Unity.Syntax
{
    public enum SyntaxKind
    {
        None,

        TrueKeyword,
        FalseKeyword,

        // Tokens
        OpenParenToken,
        CloseParenToken,
        OpenBracketToken,
        CloseBracketToken,
        OpenBraceToken,
        CloseBraceToken,

        SemiToken,
        CommaToken,

        LessThanToken,
        LessThanEqualsToken,
        GreaterThanToken,
        GreaterThanEqualsToken,
        LessThanLessThanToken,
        GreaterThanGreaterThanToken,
        PlusToken,
        PlusPlusToken,
        MinusToken,
        MinusMinusToken,
        AsteriskToken,
        SlashToken,
        PercentToken,
        AmpersandToken,
        BarToken,
        AmpersandAmpersandToken,
        BarBarToken,
        CaretToken,
        NotToken,
        TildeToken,
        QuestionToken,
        ColonToken,
        ColonColonToken,

        EqualsToken,
        AsteriskEqualsToken,
        SlashEqualsToken,
        PercentEqualsToken,
        PlusEqualsToken,
        MinusEqualsToken,
        LessThanLessThanEqualsToken,
        GreaterThanGreaterThanEqualsToken,
        AmpersandEqualsToken,
        CaretEqualsToken,
        BarEqualsToken,

        EqualsEqualsToken,
        ExclamationEqualsToken,
        DotToken,

        IdentifierToken,
        IntegerLiteralToken,
        FloatLiteralToken,
        StringLiteralToken,
        BracketedStringLiteralToken,

        // Trivia
        EndOfLineTrivia,
        WhitespaceTrivia,
        SingleLineCommentTrivia,
        BlockCommentEndOfFile,
        MultiLineCommentTrivia,
        SkippedTokensTrivia,
        CgProgramTrivia,

        EndOfFileToken,

        // Names
        IdentifierName = EndOfFileToken + 1000,
        QualifiedName,
        ArrayRankSpecifier,

        IdentifierDeclarationName,
        QualifiedDeclarationName,

        // Expressions
        ParenthesizedExpression,
        ConditionalExpression,
        MethodInvocationExpression,
        FunctionInvocationExpression,
        NumericConstructorInvocationExpression,
        ElementAccessExpression,
        Argument,
        ArgumentList,
        TemplateArgumentList,
        CastExpression,
        ArrayInitializerExpression,
        StateInitializer,
        StateArrayInitializer,
        SamplerStateInitializer,
        CompoundExpression,
        CompileExpression,

        // Binary expressions
        AddExpression,
        SubtractExpression,
        MultiplyExpression,
        DivideExpression,
        ModuloExpression,
        LeftShiftExpression,
        RightShiftExpression,
        LogicalOrExpression,
        LogicalAndExpression,
        BitwiseOrExpression,
        BitwiseAndExpression,
        ExclusiveOrExpression,
        EqualsExpression,
        NotEqualsExpression,
        LessThanExpression,
        LessThanOrEqualExpression,
        GreaterThanExpression,
        GreaterThanOrEqualExpression,

        // Assignment expressions
        SimpleAssignmentExpression,
        AddAssignmentExpression,
        SubtractAssignmentExpression,
        MultiplyAssignmentExpression,
        DivideAssignmentExpression,
        ModuloAssignmentExpression,
        AndAssignmentExpression,
        ExclusiveOrAssignmentExpression,
        OrAssignmentExpression,
        LeftShiftAssignmentExpression,
        RightShiftAssignmentExpression,

        // Unary expressions
        UnaryPlusExpression,
        UnaryMinusExpression,
        BitwiseNotExpression,
        LogicalNotExpression,
        PreIncrementExpression,
        PreDecrementExpression,
        PostIncrementExpression,
        PostDecrementExpression,

        // Primary expressions
        NumericLiteralExpression,
        StringLiteralExpression,
        TrueLiteralExpression,
        FalseLiteralExpression,

        // Attributes
        Attribute,
        AttributeArgumentList,

        IncompleteMember,
        BadToken,

        // Unity
        CompilationUnit,
        Shader,
        ShaderProperties,
        ShaderProperty,
        ShaderPropertyAttribute,
        ShaderPropertySimpleType,
        ShaderPropertyRangeType,
        ShaderPropertyNumericDefaultValue,
        ShaderPropertyVectorDefaultValue,
        ShaderPropertyTextureDefaultValue,
        Vector3,
        Vector4,
        ShaderTags,
        ShaderTag,
        Category,
        SubShader,
        Pass,
        UsePass,
        GrabPass,
        CgProgram,
        CgInclude,
        CommandConstantValue,
        CommandConstantColorValue,
        CommandVariableValue,
        CommandFallback,
        CommandCustomEditor,
        CommandCull,
        CommandZWrite,
        CommandZTest,
        CommandOffset,
        CommandBlendOff,
        CommandBlendColor,
        CommandBlendColorAlpha,
        CommandColorMask,
        CommandLod,
        CommandName,
        CommandLighting,
        CommandStencil,
        CommandStencilRef,
        CommandStencilReadMask,
        CommandStencilWriteMask,
        CommandStencilComp,
        CommandStencilPass,
        CommandStencilFail,
        CommandStencilZFail,
        CommandDependency,
        CommandMaterial,
        CommandMaterialDiffuse,
        CommandMaterialAmbient,
        CommandMaterialShininess,
        CommandMaterialSpecular,
        CommandMaterialEmission,
        CommandFog,
        CommandFogMode,
        CommandFogColor,
        CommandFogDensity,
        CommandFogRange,
        CommandSeparateSpecular,
        CommandSetTexture,
        CommandSetTextureCombine,
        CommandSetTextureCombineSource,
        CommandSetTextureCombineUnaryValue,
        CommandSetTextureCombineBinaryValue,
        CommandSetTextureCombineLerpValue,
        CommandSetTextureCombineMultiplyAlphaValue,
        CommandSetTextureCombineAlphaComponent,
        CommandSetTextureConstantColor,
        CommandSetTextureMatrix,
        CommandAlphaTestOff,
        CommandAlphaTestComparison,
        CommandAlphaToMask,
        CommandColorMaterial,
        CommandBindChannels,
        CommandBindChannelsBind,
        EnumNameExpression,

        ShaderKeyword,
        PropertiesKeyword,
        RangeKeyword,
        FloatKeyword,
        IntKeyword,
        ColorKeyword,
        VectorKeyword,
        _2DKeyword,
        _3DKeyword,
        CubeKeyword,
        AnyKeyword,
        CategoryKeyword,
        SubShaderKeyword,
        TagsKeyword,
        PassKeyword,
        CgProgramKeyword,
        CgIncludeKeyword,
        EndCgKeyword,
        FallbackKeyword,
        CustomEditorKeyword,
        CullKeyword,
        ZWriteKeyword,
        ZTestKeyword,
        OffsetKeyword,
        BlendKeyword,
        BlendOpKeyword,
        ColorMaskKeyword,
        AlphaToMaskKeyword,
        LodKeyword,
        NameKeyword,
        LightingKeyword,
        StencilKeyword,
        RefKeyword,
        ReadMaskKeyword,
        WriteMaskKeyword,
        CompKeyword,
        CompBackKeyword,
        CompFrontKeyword,
        FailKeyword,
        ZFailKeyword,
        UsePassKeyword,
        GrabPassKeyword,
        DependencyKeyword,
        MaterialKeyword,
        DiffuseKeyword,
        AmbientKeyword,
        ShininessKeyword,
        SpecularKeyword,
        EmissionKeyword,
        FogKeyword,
        ModeKeyword,
        DensityKeyword,
        SeparateSpecularKeyword,
        SetTextureKeyword,
        CombineKeyword,
        AlphaKeyword,
        LerpKeyword,
        DoubleKeyword,
        QuadKeyword,
        ConstantColorKeyword,
        MatrixKeyword,
        AlphaTestKeyword,
        ColorMaterialKeyword,
        BindChannelsKeyword,
        BindKeyword
    }
}