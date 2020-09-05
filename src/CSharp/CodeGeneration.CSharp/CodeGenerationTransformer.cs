﻿using static CodeGen.CSharp.Context.CSharpContext;

namespace CodeGeneration.CSharp
{
    public abstract class CodeGenerationTransformer 
    {
        public CSharpCodeGenerationEngine Engine { get; set; }

        public abstract void Transform();
    }

    public abstract class CodeGenerationPrecompileTimeTransformer:CodeGenerationTransformer 
    {
    }

    public abstract class CodeGenerationDesignTimeTransformer:CodeGenerationTransformer
    {
    }
}
