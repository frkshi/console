using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace FunctionLib
{
    /// <summary>
    /// 针对集合元素提供的扩展方法
    /// </summary>
    public static class EnumerableHelper
    {
        /// <summary>
        /// 校验集合是否为NULL或者包含元素数量为0。
        /// </summary>
        /// <param name="source">需要校验集合对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null || source.Count() == 0)
                return true;

            return false;
        }

        /// <summary>
        /// 针对IEnumerable&lt;<typeparamref name="T"/>&gt;类型扩展一个Convert方法，提供批量转换集合内元素的方法
        /// </summary>
        /// <typeparam name="TInput">源集合的元素类型</typeparam>
        /// <typeparam name="TOutput">输出类型</typeparam>
        /// <param name="source">集合实例</param>
        /// <param name="converter">转换操作委托实例</param>
        /// <returns>转换结果</returns>
        public static IEnumerable<TOutput> ConvertAll<TInput, TOutput>(this IEnumerable<TInput> source,
            Converter<TInput, TOutput> converter)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (converter == null)
                throw new ArgumentNullException("converter");

            List<TOutput> result = new List<TOutput>();

            foreach (var item in source)
            {
                result.Add(converter(item));
            }

            return result;
        }

        /// <summary>
        /// 针对IEnumerable&lt;<typeparamref name="T"/>&gt;类型扩展一个Convert方法，提供批量转换集合内元素的方法
        /// </summary>
        /// <typeparam name="TInput">源集合的元素类型</typeparam>
        /// <typeparam name="TOutput">输出类型</typeparam>
        /// <param name="source">集合实例</param>
        /// <param name="converter">转换操作委托实例</param>
        /// <param name="predicate">提供筛选元集合的方法</param>
        /// <returns>转换结果</returns>
        public static IEnumerable<TOutput> ConvertAll<TInput, TOutput>(this IEnumerable<TInput> source,
            Converter<TInput, TOutput> converter, Predicate<TInput> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (converter == null)
                throw new ArgumentNullException("converter");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            List<TOutput> result = new List<TOutput>();

            foreach (var item in source)
            {
                if (predicate(item))
                    result.Add(converter(item));
            }

            return result;
        }

        /// <summary>
        /// 针对IEnumerable&lt;<typeparamref name="T"/>&gt;类型扩展一个Foreach语句的方法, 代替foreach语句
        /// </summary>
        /// <typeparam name="T">集合内具体类型</typeparam>
        /// <param name="source">集合实例</param>
        /// <param name="action">迭代函数委托</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (action == null)
                throw new ArgumentNullException("action");

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// 针对IEnumerable&lt;<typeparamref name="T"/>&gt;类型扩展一个Foreach语句的方法, 代替foreach语句
        /// 可以通过Predicate&lt;T&gt;进行过滤;
        /// </summary>
        /// <typeparam name="T">集合内具体类型</typeparam>
        /// <param name="source">集合实例</param>
        /// <param name="action">迭代函数委托</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action, Predicate<T> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (action == null)
                throw new ArgumentNullException("action");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (var item in source)
            {
                if (predicate(item))
                    action(item);
            }
        }

        /// <summary>
        /// 针对IEnumerable&lt;<typeparamref name="T"/>&gt;类型扩展一个Foreach语句的方法, 代替foreach语句
        /// 可以通过Predicate&lt;T&gt;进行过滤;
        /// </summary>
        /// <typeparam name="T">集合内具体类型</typeparam>
        /// <param name="source">集合实例</param>
        /// <param name="func">迭代函数委托</param>
        public static void ForEach<T>(this IEnumerable<T> source, Func<T, bool> func, Predicate<T> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (func == null)
                throw new ArgumentNullException("action");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (var item in source)
            {
                if (predicate(item))
                    if (!func(item))
                        break;
            }
        }

        /// <summary>
        /// 针对IDictionary&lg<typeparamref name="TKey"/>,<typeparamref name="TValue"/>&gt;类型取Value值时如果key不存在则给出默认值。
        /// </summary>
        /// <typeparam name="TKey">Key Type</typeparam>
        /// <typeparam name="TValue">Value Type</typeparam>
        /// <param name="source">实例</param>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回值</returns>
        public static TValue Value<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Count == 0)
                return defaultValue;

            if (!source.ContainsKey(key))
                return defaultValue;

            return source[key];
        }

        /// <summary>
        /// 针对IDictionary&lg<typeparamref name="TKey"/>,<typeparamref name="TValue"/>&gt;类型,设置新值。Value值时如果key不存在则给出默认值。
        /// </summary>
        /// <typeparam name="TKey">Key Type</typeparam>
        /// <typeparam name="TValue">Value Type</typeparam>
        /// <param name="source">实例</param>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回值</returns>
        public static TValue Val<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Count == 0)
                source.Add(key, defaultValue);

            if (!source.ContainsKey(key))
                source.Add(key, defaultValue);

            return source[key] = defaultValue;
        }

        /// <summary>
        /// 字符串连接操作
        /// </summary>
        /// <param name="source">字符串集合</param>
        /// <param name="sp">间隔字符</param>
        /// <returns>连接后的字符串结果</returns>
        public static string Join(this IEnumerable<string> source, string splitChar)
        {
            if (source == null || source.Count() == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            int i = 0;
            int length = source.Count();

            foreach (string s in source)
            {
                sb.Append(s);
                sb.Append(i == length - 1 ? string.Empty : splitChar);
                i++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 使用指定的分隔符以字符串形式连接集合内的元素的值，最后返回字符串。
        /// 格式化值可以通过<paramref name="func"/>来得到。
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">source</param>
        /// <param name="func">格式化</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public static string JoinFormat<T>(this IEnumerable<T> source, Func<T, string> func, string splitChar)
        {
            if (source == null || source.Count() == 0)
                return string.Empty;

            if (func == null)
                return string.Empty;

            string sp = string.IsNullOrEmpty(splitChar) ? "," : splitChar;

            StringBuilder sb = new StringBuilder();
            int i = 0;
            int length = source.Count();

            foreach (T it in source)
            {
                sb.Append(func(it));
                sb.Append(i == length - 1 ? string.Empty : sp);
                i++;
            }

            return sb.ToString();
        }

        public static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : class
        {
            if (source == null)
                throw new ArgumentNullException("source");

            foreach (T it in source)
            {
                if (predicate(it))
                    return true;
            }

            return false;
        }
    }
}
