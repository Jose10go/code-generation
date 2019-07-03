using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CodeGen.Commands.Abstract;
using CodeGen.Engine.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.Engine.CSharp.DocumentEditing
{
    public class AutofacResolver : DI.AutofacResolver<Solution, CSharpSyntaxNode, DocumentEditor>
    {
        protected override void DoAutomaticRegister(ContainerBuilder builder)
        {
            base.DoAutomaticRegister(builder);

            builder.RegisterGeneric(typeof(CSharpTargetBuilder<,>)).AsImplementedInterfaces();
        }
    }
}
