using LongArrayLib;
using LongArrayLib.Internal;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Linq
{
    /// <summary>
    /// LINQの拡張を記述します。
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// <see cref="LongArray{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">配列に変換するシーケンス</param>
        /// <returns>変換後の配列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/></exception>
        public static LongArray<T> ToLongArray<T>(this IEnumerable<T> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (source is LongArray<T> cloned) return (LongArray<T>)cloned.Clone();
            if (source is ICollection<T> c)
            {
                if (c.Count == 0) return LongArray<T>.Empty;
                if (source is T[] array) return new LongArray<T>(ref MemoryMarshal.GetArrayDataReference(array), array.Length);

                var result = new LongArray<T>(c.Count, false);
                T[] buffer = ArrayPool<T>.Shared.Rent(c.Count);

                try
                {
                    c.CopyTo(buffer, 0);
                    LongArray.Copy(buffer, result, c.Count);
                }
                finally
                {
                    ArrayPool<T>.Shared.Return(buffer);
                }
                return result;
            }

            using var builder = new LongArrayBuilder<T>(128);
            foreach (T current in source) builder.Add(current);
            return builder.ToArray();
        }
    }
}
