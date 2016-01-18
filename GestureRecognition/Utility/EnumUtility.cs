using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestureRecognition.Utility
{
    public static class EnumUtility {
        public static IEnumerable<T> GetValues<T>() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
