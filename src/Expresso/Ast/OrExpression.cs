using Expresso.Values;

namespace Expresso.Ast
{
    public class OrExpression : BinaryExpression
    {
        public OrExpression(Expression left, Expression right) : base(left, right)
        {
        }

        internal override ExpressoValue Evaluate(ExpressoValue leftValue, ExpressoValue rightValue)
            => BooleanValue.Create(leftValue.ToBooleanValue() || rightValue.ToBooleanValue());
    }
}
