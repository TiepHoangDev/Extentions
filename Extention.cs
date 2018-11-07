using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCommon
{
    public static class Extention
    {
        public static string GetPropertyName<T, Y>(this Expression<Func<T, Y>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                var member = (MemberExpression)expression.Body;
                return member.Member.Name;
            }
            if (expression.Body is MethodCallExpression)
            {
                return ((MethodCallExpression)expression.Body).Method.Name;
            }
            if (expression.Body is UnaryExpression)
            {
                return ((expression.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            }
            return null;
        }

        public static Type GetPropertyType<T, Y>(this Expression<Func<T, Y>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                var member = (MemberExpression)expression.Body;
                return ((PropertyInfo)member.Member).PropertyType;
            }
            return ((PropertyInfo)((expression.Body as UnaryExpression).Operand as MemberExpression).Member).PropertyType;
        }

        public static bool IsNumericType(this Type o)
        {
            switch (Type.GetTypeCode(o))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static string Bindata(this string stringFormat, params object[] value)
        {
            return string.Format(stringFormat, value);
        }

        public static string Format(this object value, string format = "#,##0")
        {
            return string.Format("{0:" + format + "}", value);
        }

        /// <summary>
        /// Hiện thị money ở format "#,###"
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string ToMoneyFormat(this decimal? money)
        {
            string format = "#,###";
            if (money.HasValue) return money.Value.ToString(format);
            return "0";
        }

        public static Y TryGetValue<T, Y>(this T source, Expression<Func<T, Y>> valueMember)
        {
            if (source != null)
            {
                return (Y)source.GetType().GetProperty(valueMember.GetPropertyName()).GetValue(source);
            }
            return default(Y);
        }

        public static string TryGetMessage(this Exception ex)
        {
            string message = ex.Message;
            if (ex.InnerException != null)
            {
                message += ", InnerException: {0}".Bindata(ex.InnerException.TryGetValue(q => q.Message));
            }
            return message;
        }

        public static string GetName<T>(this T source, Expression<Func<T, object>> valueMember)
        {
            return valueMember.GetPropertyName();
        }

        /// <summary>
        /// Copy all Field CanWrite math Name and PropertyType.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Y CopyDeep<T, Y>(this Y target, T source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var pro_target = target.GetType().GetProperties().ToList();
            foreach (var item in source.GetType().GetProperties())
            {
                var math = pro_target.FirstOrDefault(q => q.Name == item.Name);
                if (math != null)
                {
                    if (math.CanWrite && math.PropertyType == item.PropertyType)
                    {
                        math.SetValue(target, item.GetValue(source));
                    }
                    pro_target.Remove(math);
                }
            }
            return target;
        }

        /// <summary>
        /// Copy only Field CanWrite math Name, PropertyType and Namespace = System.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Y CopyDeepOnlyValue<T, Y>(this Y target, T source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var pro_target = target.GetType().GetProperties().ToList();
            foreach (var item in source.GetType().GetProperties())
            {
                var math = pro_target.FirstOrDefault(q => q.Name == item.Name);
                if (math != null)
                {
                    if (math.CanWrite && math.PropertyType == item.PropertyType && math.PropertyType.Namespace == "System")
                    {
                        math.SetValue(target, item.GetValue(source));
                    }
                    pro_target.Remove(math);
                }
            }
            return target;
        }

        public static DateTime OnlyDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public static TimeSpan OnlyTime(this DateTime dateTime)
        {
            return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        }

        public static DateTime ToDate(this string sdate, string format = "yyyy-MM-dd")
        {
            DateTime date = DateTime.Now;
            if (DateTime.TryParseExact(sdate, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out date)) return date;
            return DateTime.Now;
        }

        public static string Display(this object obj, string format = "#,###")
        {
            return string.Format("{0:" + format + "}", obj);
        }
    }

    public static class LinqExtentions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            return source.Select(q =>
            {
                action(q);
                return q;
            });
        }
    }
}
