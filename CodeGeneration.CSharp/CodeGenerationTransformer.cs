using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;

namespace CodeGeneration.CSharp
{
    public abstract class CodeGenerationTransformer 
    {
        public DocumentEditingCodeGenerationEngine Engine { get; set; }

        public abstract void Transform();

    }
}
