using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expresso.Values;

namespace Expresso.Ast
{
    public class VariableExpression : Expression
    {
        private readonly string _name;

        public VariableExpression(string name)
        {
            _name = name;
        }

        public override ValueTask<ExpressoValue> EvaluateAsync(ExpressoContext context)
        {
            var value = context.GetValue(_name);
            return ValueTask.FromResult(value is null ? NilValue.Instance : ExpressoValue.Create(value));
        }
    }
}
