using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ListExtensions
    {
        public static List<T> ListMe<T>(this T thisObject)
        {
            return new List<T> { thisObject };
        }
    }
}
