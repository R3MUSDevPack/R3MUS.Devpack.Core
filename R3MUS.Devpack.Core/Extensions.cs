using Newtonsoft.Json;
using R3MUS.Devpack.Core.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace R3MUS.Devpack.Core
{
    public static class Extensions
    {
        private static CultureInfo cultureInfo = new CultureInfo("en-GB");

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
                    if (srcProp.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
                    {
                        var destProp = destType.GetProperty(srcProp.Name);
                        destProp.SetValue(dest, srcProp.GetValue(src));
                    }
                });
            }
            else
            {
                throw new Exception("Type mismatch");
            }
        }

        public static NameValueCollection ToNameValueCollection(this object source, string prefix = "")
        {
            if (source == null)
            {
                return null;
            }

            var collection = new NameValueCollection();

            source.GetType()
                  .GetProperties()
                  .Where(x => x.GetValue(source, null) != null)
                  .Where(x => !x.GetCustomAttributes<IgnoreDataMemberAttribute>().Any())
                  .ToList()
                  .ForEach(p => collection.AddRange(GetNameAndValue(p, source, prefix)));

            return collection;
        }

        public static void AddRange(this NameValueCollection collection, IEnumerable<KeyValuePair<string, string>> items)
        {
            foreach (var item in items)
            {
                collection.Add(item.Key, item.Value);
            }
        }

        private static IEnumerable<KeyValuePair<string, string>> GetNameAndValue(PropertyInfo prop, object source, string prefix = "")
        {
            var propType = prop.PropertyType;
            if (propType != typeof(string) && propType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                return GetIEnumerableValues(prop, source, prefix).ToList();
            }
            else if (propType == typeof(DateTime) || propType == typeof(DateTime?))
            {
                return GetDateTimeValue(prop, source, prefix);
            }
            else if (propType == typeof(decimal) || propType == typeof(decimal?))
            {
                return GetDecimalValue(prop, source, prefix);
            }
            else if (propType.IsClass && propType != typeof(string))
            {
                var value = prop.GetValue(source);

                if (value != null)
                {
                    return ToKeyValuePairCollection(value, GetPropertyName(prop) + ".");
                }

                return Enumerable.Empty<KeyValuePair<string, string>>();
            }

            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(prefix + GetPropertyName(prop), GetValue(prop, source))
            };
        }

        // This won't deal with the case where there's an IEnumerable of IEnumerables (I.E. jagged arrays)
        private static IEnumerable<KeyValuePair<string, string>> GetIEnumerableValues(PropertyInfo prop, object source, string parentPrefix = "")
        {
            var index = 0; // IEnumerable doesn't have a count extension to get the length, so we can't use a for loop
            var collectionName = parentPrefix + GetPropertyName(prop);

            foreach (var item in (IEnumerable)prop.GetValue(source, null))
            {
                if (item != null)
                {
                    string itemName = string.Format("{0}[{1}]", collectionName, index);
                    index++;

                    if (item.GetType() == typeof(decimal))
                    {
                        yield return new KeyValuePair<string, string>(itemName, ((decimal)item).ToString(cultureInfo));
                    }
                    if (item.GetType().IsPrimitive || item.GetType() == typeof(string))
                    {
                        yield return new KeyValuePair<string, string>(itemName, item.ToString());
                    }
                    else
                    {
                        itemName += ".";
                        var childCollection = item.ToNameValueCollection(itemName);
                        var flattened = childCollection.AllKeys.SelectMany(key => childCollection.GetValues(key), (k, v) => new { Key = k, Value = v });

                        foreach (var kvp in flattened)
                        {
                            yield return new KeyValuePair<string, string>(kvp.Key, kvp.Value);
                        }
                    }
                }
            }
        }

        private static string GetPropertyName(PropertyInfo prop)
        {
            return prop.Name;
        }

        private static IEnumerable<KeyValuePair<string, string>> GetDateTimeValue(PropertyInfo prop, object source, string prefix = "")
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(prefix + GetPropertyName(prop), ((DateTime)prop.GetValue(source, null)).ToString("o"))
            };
        }

        private static IEnumerable<KeyValuePair<string, string>> GetDecimalValue(PropertyInfo prop, object source, string prefix = "")
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(prefix + GetPropertyName(prop), ((decimal)prop.GetValue(source, null)).ToString(cultureInfo))
            };
        }

        private static ICollection<KeyValuePair<string, string>> ToKeyValuePairCollection(this object source, string prefix = "")
        {
            if (source == null)
            {
                return null;
            }

            var collection = new List<KeyValuePair<string, string>>();

            source.GetType()
                  .GetProperties()
                  .Where(x => x.GetValue(source, null) != null)
                  .ToList()
                  .ForEach(p => collection.AddRange(GetNameAndValue(p, source, prefix)));

            return collection;
        }

        private static string GetValue(PropertyInfo prop, object source)
        {
            return prop.GetValue(source, null).ToString();
        }

        public static string ToQueryString(this NameValueCollection source, bool removeEmptyEntries = true)
        {
            if (source == null)
            {
                return string.Empty;
            }
            var result = "?" + string.Join("&", source.AllKeys
                .Where(key => !removeEmptyEntries || source.GetValues(key).Any(value => !string.IsNullOrEmpty(value)))
                .SelectMany(key => source.GetValues(key)
                    .Where(value => !removeEmptyEntries || !string.IsNullOrEmpty(value))
                    .Select(value => string.Format("{0}={1}", HttpUtility.UrlEncode(key).Replace("%5b", "[").Replace("%5d", "]"), value != null ? HttpUtility.UrlEncode(value) : string.Empty)))
                .ToArray());
            return result;
        }
        public static bool IsSimple(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }
    }
}
