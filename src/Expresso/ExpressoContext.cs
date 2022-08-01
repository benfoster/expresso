namespace Expresso
{
    public class ExpressoContext
    {
        private readonly Dictionary<string, object>? _variables;

        public ExpressoContext(Dictionary<string, object>? variables = null)
        {
            _variables = variables;
        }

        public object? GetValue(string name)
        {
            if (_variables != null && _variables.TryGetValue(name, out var value))
                return value;

            return null;
        }
    }
}
