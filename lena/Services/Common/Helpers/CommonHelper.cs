using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Common.Helpers
{
  public class CommonHelper
  {
    private static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(Byte),
            typeof(SByte),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(Decimal),
            typeof(Double),
            typeof(Single),
        };

    public static bool IsNumericType(Type type)
    {
      return NumericTypes.Contains(type) ||
             NumericTypes.Contains(Nullable.GetUnderlyingType(type));
    }
  }
}
