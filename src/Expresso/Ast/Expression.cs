using System.Threading.Tasks;
using Expresso.Values;

namespace Expresso.Ast
{
    public abstract class Expression
    {
        public abstract ValueTask<ExpressoValue> EvaluateAsync(ExpressoContext context);
    }
}
