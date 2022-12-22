using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NovelAIHelper.Utils
{
    public static class TManager
    {
        public static void CopyTo<T>(this T source, T destination) where T : class, new()
        {
            foreach (var x in typeof(T).GetProperties().Where(p => p.CanWrite && (!p.PropertyType.IsClass && !typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string))))
            {
                var ps = x.GetValue(source,      null);
                var pd = x.GetValue(destination, null);
                if (!IsEqual(ps, pd))
                    x.SetValue(destination, ps, null);
            }
        }

        public static T GetCopy<T>(this T source) where T : class, new()
        {
            var destination = new T();
            foreach (var x in typeof(T).GetProperties().Where(p => p.CanWrite && (!p.PropertyType.IsClass && !typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string))))
            {
                var ps = x.GetValue(source,      null);
                var pd = x.GetValue(destination, null);
                if (!IsEqual(ps, pd))
                    x.SetValue(destination, ps, null);
            }

            return destination;
        }

        private static bool IsEqual(object? val1, object? val2)
        {
            if (val1 == null && val2 == null) return true;
            if (val1 == null || val2 == null) return false;
            return val1.Equals(val2);
        }

        private static bool IsEqual(string val1, string val2)
        {
            if (string.IsNullOrWhiteSpace(val1) && string.IsNullOrWhiteSpace(val2)) return true;
            if (string.IsNullOrWhiteSpace(val1) && !string.IsNullOrWhiteSpace(val2)) return false;
            return val1.Equals(val2);
        }
        //private static List<PropertyInfo> _propertyInfos = new();
        //public TManager(bool useAllProperties = true)
        //{
        //    if(useAllProperties)
        //        SetAllProperties();
        //}

        //public void SetAllProperties()
        //{
        //    _propertyInfos.Clear();
        //    _propertyInfos = typeof(T).GetProperties().Where(p => p.CanWrite && (!p.PropertyType.IsClass && !typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string))).ToList();

        //}

        //public void AddProperty<TT>(Expression<Func<TT>> property)
        //{
        //    var prop = typeof(T).GetProperty(((MemberExpression)property.Body).Member.Name);
        //    if(prop != null)
        //        _propertyInfos.Add(prop);
        //}
    }
}
