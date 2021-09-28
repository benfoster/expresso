using System.Threading.Tasks;
using Expresso.Ast;
using Shouldly;
using Xunit;

namespace Expresso.Tests
{
    public class ExpressoParserTests
    {
        [Theory]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public async Task Parses_boolean_literals(string text, bool expectedResult)
        {
            (await Evaluate(text)).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("true and true", true)]
        [InlineData("true and false", false)]
        [InlineData("false and true", false)]
        [InlineData("false and false", false)]
        [InlineData("TRUE AND TRUE", true)]
        public async Task Logical_and_boolean_literals(string text, bool expectedResult)
        {
            (await Evaluate(text)).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("true or false", true)]
        [InlineData("false or true", true)]
        [InlineData("false or false", false)]
        [InlineData("FALSE OR TRUE", true)]
        public async Task Logical_or_boolean_literals(string text, bool expectedResult)
        {
            (await Evaluate(text)).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("true and true and true", true)]
        [InlineData("false and true and false", false)]
        [InlineData("false or false or false", false)]
        [InlineData("false or false or true", true)]
        public async Task Multiple_logical_operations(string text, bool expectedResult)
        {
            (await Evaluate(text)).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("false and false or true", false)] // false AND (false OR true) > false
        public async Task Logical_operators_are_right_associative(string text, bool expectedResult)
        {
            (await Evaluate(text)).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("(false and false) or true", true)]
        [InlineData("((false and false) or (false or false)) or true", true)]
        public async Task Supports_grouping_with_parenthesis(string text, bool expectedResult)
        {
            (await Evaluate(text)).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("10 > 0", true)]
        [InlineData("-10 > 0", false)]
        [InlineData("10.563 > 10.56", true)]
        [InlineData("10 > false", true)]
        [InlineData("0 > false", false)]
        public async Task Greater_than_comparison(string text, bool expectedResult)
        {
            (await Evaluate(text)).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("10 > 1 and (false or true) ", true)]
        [InlineData("true and 100 > 1000 ", false)]
        public async Task Can_combine_comparison_and_logical_operators(string text, bool expectedResult)
        {
            (await Evaluate(text)).ShouldBe(expectedResult);
        }

        private async Task<bool> Evaluate(string text)
        {
            ExpressoParser.TryParse(text, out Expression? expression, out _).ShouldBeTrue();
            expression.ShouldNotBeNull();            
            var result = await expression.EvaluateAsync(new ExpressoContext());
            return result.ToBooleanValue();
        }
    }
}
