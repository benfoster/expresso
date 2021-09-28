using Expresso.Values;

namespace Expresso.Ast
{
    public class GreaterThanExpression : BinaryExpression
    {
        public GreaterThanExpression(Expression left, Expression right) : base(left, right)
        {
        }

        internal override ExpressoValue Evaluate(ExpressoValue leftValue, ExpressoValue rightValue)
        {
            if (leftValue is NumericValue)
            {
                return leftValue.ToNumericValue() > rightValue.ToNumericValue()
                    ? BooleanValue.True
                    : BooleanValue.False;
            }

            return NilValue.Instance;
        }
    }
}
