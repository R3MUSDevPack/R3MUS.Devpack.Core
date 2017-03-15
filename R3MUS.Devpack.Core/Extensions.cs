using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace R3MUS.Devpack.Core
{
    public static class Extensions
    {
        public static object Deserialize(this string me, Type t)
        {
            return JsonConvert.DeserializeObject(me, t);
        }

        public static void SetProperties(this object dest, object src)
        {
            if (dest.GetType() == src.GetType())
            {
                var srcType = src.GetType();
                var destType = dest.GetType();

                var properties = srcType.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).ToList<PropertyInfo>();

                properties.ForEach(srcProp => {
                    var destProp = destType.GetProperty(srcProp.Name);
                    destProp.SetValue(dest, srcProp.GetValue(src));
                });
            }
            else
            {
                throw new Exception("Type mismatch");
            }
        }
    }
}
