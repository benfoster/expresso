namespace Expresso.Values
{
    using System;

    public abstract class ExpressoValue
    {
        public abstract bool ToBooleanValue();
        public abstract decimal ToNumericValue();

        public static ExpressoValue Create(object? value)
        {
            var typeOfValue = value?.GetType();

            switch (System.Type.GetTypeCode(typeOfValue))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                    return NumericValue.Create(Convert.ToInt32(value));
            };

            throw new InvalidOperationException();
        }
    }
}
