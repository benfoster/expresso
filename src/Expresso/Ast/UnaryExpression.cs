using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Expresso.Values;

namespace Expresso.Ast
{
    public abstract class UnaryExpression : Expression
    {
        public UnaryExpression(Expression inner)
        {
            Inner = inner;
        }

        public Expression Inner { get; set; }

        public override ValueTask<ExpressoValue> EvaluateAsync(ExpressoContext context)
        {
            var innerTask = Inner.EvaluateAsync(context);

            if (innerTask.IsCompletedSuccessfully)
            {
                return ValueTask.FromResult(Evaluate(innerTask.Result));
            }

            return Awaited(innerTask);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private async ValueTask<ExpressoValue> Awaited(ValueTask<ExpressoValue> innerTask)
        {
            var innerValue = await innerTask;

            return Evaluate(innerValue);
        }

        internal virtual ExpressoValue Evaluate(ExpressoValue innerValue)
        {
            throw new NotImplementedException();
        }
    }
}
