using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace R3MUS.Devpack.Core
{
    public static class Extensions
    {
        public static T Deserialize<T>(this string me)
        {
            return (T)JsonConvert.DeserializeObject(me, typeof(T));
        }

        public static void SetProperties(this object dest, object src)
        {
            if (dest.GetType() == src.GetType())
            {
                var srcType = src.GetType();
                var destType = dest.GetType();

                var properties = srcType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList<PropertyInfo>();

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
