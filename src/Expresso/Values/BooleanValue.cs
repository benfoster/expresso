namespace Expresso.Values
{
    public class BooleanValue : ExpressoValue
    {
        public static readonly BooleanValue False = new BooleanValue(false);
        public static readonly BooleanValue True = new BooleanValue(true);

        private readonly bool _value;

        private BooleanValue(bool value)
        {
            _value = value;
        }

        public static BooleanValue Create(bool value) => value ? True : False;

        public override bool ToBooleanValue() => _value;
        public override decimal ToNumericValue() => _value ? 1 : 0;
    }
}
