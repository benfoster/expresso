using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Expresso.Values;

namespace Expresso.Ast
{
    public abstract class BinaryExpression : Expression
    {
        protected BinaryExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public Expression Left { get; }

        public Expression Right { get; }

        /// <summary>
        /// Evaluates two operands and tries to avoid state machines.
        /// </summary>
        public override ValueTask<ExpressoValue> EvaluateAsync(ExpressoContext context)
        {
            var leftTask = Left.EvaluateAsync(context);
            var rightTask = Right.EvaluateAsync(context);

            if (leftTask.IsCompletedSuccessfully && rightTask.IsCompletedSuccessfully)
            {
                return ValueTask.FromResult(Evaluate(leftTask.Result, rightTask.Result));
            }

            return Awaited(leftTask, rightTask);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private async ValueTask<ExpressoValue> Awaited(
            ValueTask<ExpressoValue> leftTask,
            ValueTask<ExpressoValue> rightTask)
        {
            var leftValue = await leftTask;
            var rightValue = await rightTask;

            return Evaluate(leftValue, rightValue);
        }

        // sub-classes using the default implementation need to override this
        internal virtual ExpressoValue Evaluate(ExpressoValue leftValue, ExpressoValue rightValue)
        {
            throw new NotImplementedException();
        }
    }
}
