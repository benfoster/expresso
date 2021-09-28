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
            ExpressoParser.TryParse(text, out Expression? expression, out _).ShouldBeTrue();
            expression.ShouldNotBeNull();            
            var result = await expression.EvaluateAsync(new ExpressoContext());
            result.ToBooleanValue().ShouldBe(expectedResult);
        }
    }
}
