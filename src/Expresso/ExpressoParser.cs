using System.Linq;
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
        protected static readonly Parser<string> BinaryAnd = Terms.Text("and", true);
        protected static readonly Parser<string> BinaryOr = Terms.Text("or", true);

        protected static readonly Deferred<Expression> Primary = Deferred<Expression>();
        
        static ExpressoParser()
        {
            // primary => NUMBER | STRING | BOOLEAN | property
            Primary.Parser =
                True.Then<Expression>(x => new LiteralExpression(BooleanValue.True))
                .Or(False.Then<Expression>(x => new LiteralExpression(BooleanValue.False)));

            var logicalExpression = Primary.And(
                ZeroOrMany(BinaryAnd.Or(BinaryOr).And(Primary))).Then(x =>
            {
                var result = x.Item1;

                foreach (var op in Enumerable.Reverse(x.Item2)) // Need to reverse to be right associative
                {
                    result = op.Item1.ToLowerInvariant() switch
                    {
                        "and" => new AndExpression(result, op.Item2),
                        "or" => new OrExpression(result, op.Item2),
                        _ => result
                    };
                }

                return result;
            });
            
            
            ExpressionParser = logicalExpression;
        }

        public static bool TryParse(string text, out Expression? expression, out ParseError error)
            => ExpressionParser.TryParse(text, out expression, out error);
    }
}
