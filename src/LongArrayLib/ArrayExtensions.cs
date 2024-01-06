using LongArrayLib.Internal;
using System;
using System.Runtime.CompilerServices;

namespace LongArrayLib
{
    /// <summary>
    /// 配列の拡張を記述します。
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">コピー元の配列</param>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index"><paramref name="array"/>における貼り付け開始インデックス</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="source"/>のサイズが不足</exception>
        public static unsafe void CopyTo<T>(this T[] source, LongArray<T> array, long index)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(array);
            ThrowHelper.ThrowIfNegative(index);
            if (source.Length + index > array.Length) ThrowHelper.ThrowAsShortArray(nameof(source));

            long bytesToCopy = source.Length * (long)Unsafe.SizeOf<T>();
            fixed (void* sourcePtr = source)
            {
                Buffer.MemoryCopy(sourcePtr, UnsafeHelper.Increment<T>(array.AsPointer(), index), bytesToCopy, bytesToCopy);
            }
        }
    }
}
