﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGen.Context.CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class CSharpAutofacResolver : AutofacResolver
        {
            protected override void DoAutomaticRegister(ContainerBuilder builder)
            {
                base.DoAutomaticRegister(builder);
                builder.RegisterGeneric(typeof(CSharpTarget<>)).As(typeof(ICSharpTarget<>));
                builder.RegisterGeneric(typeof(CSharpTarget<>)).As(typeof(ITarget<>));
                builder.RegisterGeneric(typeof(CSharpTargetBuilder<>)).As(typeof(ICSharpTargetBuilder<>));
                builder.RegisterGeneric(typeof(CSharpTargetBuilder<>)).As(typeof(ITargetBuilder<>));
            }
        }
    }
}