namespace Expresso.Values
{
    public class NumericValue : ExpressoValue
    {
        private readonly decimal _value;

        private NumericValue(decimal value)
        {
            _value = value;
        }

        public static NumericValue Create(decimal value) => new NumericValue(value);

        public override bool ToBooleanValue() => true;
        public override decimal ToNumericValue() => _value;
    }
}
