﻿using System.Collections.Generic;
using System.Dynamic;
namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public class Command<TSyntaxNode> : DynamicObject
        {
            Dictionary<string, dynamic> _members = new Dictionary<string, dynamic>();
            public ITarget<TSyntaxNode> Target { get; set; }
            public override bool TrySetIndex(SetIndexBinder binder, object[] indexs, object value)
            {
                var name = indexs[0] as string;
                if (!_members.ContainsKey(name))
                    _members.Add(name, value);
                else
                    _members[name] = value;
                return true;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (_members.ContainsKey(binder.Name))
                {
                    result = _members[binder.Name];
                    return true;
                }
                result = null;
                return false;
            }
        }
    }
}
