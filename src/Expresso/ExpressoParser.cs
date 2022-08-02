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
        protected static readonly Parser<string> Not = Terms.Text("not", true);
        protected static readonly Parser<char> LParen = Terms.Char('(');
        protected static readonly Parser<char> RParen = Terms.Char(')');
        protected static readonly Parser<string> GreaterThan = Terms.Text(">");
        protected static readonly Parser<string> LessThan = Terms.Text("<");
        
        static ExpressoParser()
        {
            var expression = Deferred<Expression>();

            var identifier = Terms.Identifier();
            
            // primary => NUMBER | STRING | BOOLEAN | property
            var literal =
                Terms.Decimal(NumberOptions.AllowSign).Then<Expression>(x => new LiteralExpression(NumericValue.Create(x)))
                    .Or(True.Then<Expression>(x => new LiteralExpression(BooleanValue.True)))
                    .Or(False.Then<Expression>(x => new LiteralExpression(BooleanValue.False)))
                    .Or(identifier.Then<Expression>(x => new VariableExpression(x.Span.ToString())));

            var groupExpression = Between(LParen, expression, RParen);

            var primary = literal.Or(groupExpression);

            var comparator = OneOf(GreaterThan, LessThan);

            var comparisonExpression = primary.And(ZeroOrMany(comparator.And(primary))).Then(x =>
            {
                var result = x.Item1;

                foreach (var op in Enumerable.Reverse(x.Item2))
                {
                    result = op.Item1.ToLowerInvariant() switch
                    {
                        ">" => new GreaterThanExpression(result, op.Item2),
                        "<" => new LessThanExpression(result, op.Item2),
                        _ => result
                    };
                }

                return result;
            });

            var logicalExpression = comparisonExpression.And(
                ZeroOrMany(BinaryAnd.Or(BinaryOr).And(comparisonExpression))).Then(x =>
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
            
            expression.Parser = logicalExpression;
            ExpressionParser = ZeroOrOne(Not).And(expression)
                .Then<Expression>(e => e.Item1 == null
                    ? e.Item2
                    : new NegateExpression(e.Item2));
        }

        public static bool TryParse(string text, out Expression? expression, out ParseError error)
            => ExpressionParser.TryParse(text, out expression, out error);
    }
}
