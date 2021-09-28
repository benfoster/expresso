using Expresso.Values;

namespace Expresso.Ast
{
    public class AndExpression : BinaryExpression
    {
        public AndExpression(Expression left, Expression right) : base(left, right)
        {
        }

        internal override ExpressoValue Evaluate(ExpressoValue leftValue, ExpressoValue rightValue)
            => BooleanValue.Create(leftValue.ToBooleanValue() && rightValue.ToBooleanValue());
    }
}
