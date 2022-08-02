using Expresso.Values;

namespace Expresso.Ast
{
    public class NegateExpression : UnaryExpression
    {
        public NegateExpression(Expression inner) : base(inner)
        {
        }

        internal override ExpressoValue Evaluate(ExpressoValue innerValue)
            => BooleanValue.Create(!innerValue.ToBooleanValue());
    }
}
