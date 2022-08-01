using Expresso.Values;

namespace Expresso.Ast
{
    public class LiteralExpression : Expression
    {
        private readonly ExpressoValue _value;

        public LiteralExpression(ExpressoValue value)
        {
            _value = value;
        }
        
        public override ValueTask<ExpressoValue> EvaluateAsync(ExpressoContext context) 
            => ValueTask.FromResult(_value);
    }
}
