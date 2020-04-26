using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace CodeGen.CSharp
{
    internal static class Extensions
    {
        public static SyntaxTokenList GetModifiers(params SyntaxToken[] tokens)
        {
            SyntaxTokenList result = new SyntaxTokenList();
            foreach (var item in tokens)
                if (item != default)
                    result = result.Add(item);
            return result;
        }

        public static SyntaxTokenList GetModifiers(SyntaxTokenList list, params SyntaxToken[] tokens)
        {
            SyntaxTokenList result = list==default ? new SyntaxTokenList() : list;
            foreach (var item in tokens)
                if (item != default)
                    result=result.Add(item);
            return result;
        }

        public static BaseListSyntax GetBaseTypes(BaseListSyntax implementedInterfaces,BaseTypeSyntax inheritsType=null) 
        {
            return (implementedInterfaces,inheritsType) switch
            {
                (null, null) => null,
                (_, null) => implementedInterfaces,
                (null, _) => SyntaxFactory.BaseList().AddTypes(inheritsType),
                (_, _) => SyntaxFactory.BaseList().AddTypes(inheritsType)
                                                  .AddTypes(implementedInterfaces.Types.ToArray())
                                                  
            };
        }

        public static string GetCSharpName<T>()
        {
            return GetCSharpName(typeof(T));
        }

        public static string GetCSharpName(Type type)
        {
            //TODO: Complete al primitive types an correctly generate arrays and nested types...
            if (type == typeof(object))
                return "object";
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
