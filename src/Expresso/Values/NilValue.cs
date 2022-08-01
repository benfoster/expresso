namespace Expresso.Values
{
    public class NilValue : ExpressoValue
    {
        public static readonly NilValue Instance = new NilValue();

        private NilValue()
        {

        }

        public override bool ToBooleanValue() => false;
        public override decimal ToNumericValue() => 0;
    }
}
