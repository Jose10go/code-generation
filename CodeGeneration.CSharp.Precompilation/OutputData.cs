using System;
namespace CodeGeneration.CSharp.Precompilation
{
    [Serializable]
    public class OutputData 
    {
        public string FilePath { get; set; }
        public string Status { get; set; }
        public string Kind { get; set; }
    }
}