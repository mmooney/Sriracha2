using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class JsonExtensions
    {
        public static string ToJson(this object thisObject, bool format=false)
        {
            if(format)
            {
                return JsonConvert.SerializeObject(thisObject, Formatting.Indented);
            }
            else 
            {
                return JsonConvert.SerializeObject(thisObject);
            }
        }

        public static T FromJson<T>(this string thisString)
        {
            return JsonConvert.DeserializeObject<T>(thisString);
        }
    }
}
