using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
