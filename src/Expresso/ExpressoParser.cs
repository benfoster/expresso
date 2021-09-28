using Expresso.Ast;
using Expresso.Values;
using Parlot;
using Parlot.Fluent;
using static Parlot.Fluent.Parsers;

namespace Expresso
{
    public class ExpressoParser
    {
        protected static readonly Parser<Expression> ExpressionParser;

        protected static readonly Parser<string> True = Terms.Text("true", true);
        protected static readonly Parser<string> False = Terms.Text("false", true);
        protected static readonly Deferred<Expression> Primary = Deferred<Expression>();
        
        static ExpressoParser()
        {
            // primary => NUMBER | STRING | BOOLEAN | property
            Primary.Parser =
                True.Then<Expression>(x => new LiteralExpression(BooleanValue.True))
                .Or(False.Then<Expression>(x => new LiteralExpression(BooleanValue.False)));

            ExpressionParser = Primary;
        }

        public static bool TryParse(string text, out Expression? expression, out ParseError error)
            => ExpressionParser.TryParse(text, out expression, out error);
    }
}
