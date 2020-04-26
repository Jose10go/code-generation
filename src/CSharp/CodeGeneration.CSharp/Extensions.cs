using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace CodeGen.CSharp
{
    internal static class Extensions
    {
        internal static SyntaxTokenList MergeModifiers(params SyntaxToken[] tokens)
        {
            throw new Exception();//TODO:Implement this method
        }

        internal static SyntaxTokenList AddModifiers(this SyntaxTokenList list, params SyntaxToken[] tokens)
        {
            return MergeModifiers(list.AddRange(tokens).ToArray());
        }

        internal static string GetCSharpName<T>()
        {
            return GetCSharpName(typeof(T));
        }

        private static string GetCSharpName(Type type)
        {
            //TODO: Complete al primitive types an correctly generate arrays and nested types...
            if (type == typeof(string))
                return "string";
            if (type == typeof(bool))
                return "bool";
            if (type == typeof(byte))
                return "byte";
            if (type == typeof(short))
                return "short";
            if (type == typeof(ushort))
                return "ushort";
            if (type == typeof(int))
                return "int";
            if (type == typeof(uint))
                return "uint";
            if (type == typeof(long))
                return "long";
            if (type == typeof(ulong))
                return "ulong";
            if (type == typeof(float))
                return "float";
            if (type == typeof(double))
                return "double";
            if (type == typeof(decimal))
                return "decimal";
            if (type.GetGenericArguments().Length == 0)
                return type.Name;
            var genericArguments = type.GetGenericArguments();
            var typeName = type.Name.Substring(0, type.Name.IndexOf('`'));
            return typeName+"<"+String.Join(',',genericArguments.Select(GetCSharpName))+">";
        }

    }
}
