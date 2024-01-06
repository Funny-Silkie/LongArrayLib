using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LongArrayLib
{
#if NET8_0_OR_GREATER
    [CollectionBuilder(typeof(LongArray), nameof(LongArray.Create))]
#endif

    public partial class LongArray<T>
    {
        /// <summary>
        /// <see cref="LongArray{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="length">配列長</param>
        /// <param name="zeroed">ゼロ埋めを行うかどうか</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/>が0未満</exception>
        public unsafe LongArray(long length, bool zeroed = true)
        {
            ThrowHelper.ThrowIfNegative(length);

            Length = length;
            items = zeroed
                ? NativeMemory.AllocZeroed(checked((nuint)length), checked((nuint)Unsafe.SizeOf<T>()))
                : NativeMemory.Alloc(checked((nuint)length), checked((nuint)Unsafe.SizeOf<T>()));
        }

        /// <summary>
        /// <see cref="LongArray{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="reference">先頭の要素の参照</param>
        /// <param name="length">配列長</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/>が0未満</exception>
        internal unsafe LongArray(ref T reference, long length)
            : this(length, false)
        {
            long bytesToCopy = length * Unsafe.SizeOf<T>();
            fixed (void* sourcePtr = &reference)
            {
                Buffer.MemoryCopy(sourcePtr, items, bytesToCopy, bytesToCopy);
            }
        }
    }

    public partial class LongArray
    {
        /// <summary>
        /// 配列から<see cref="LongArray{T}"/>の新しいインスタンスを生成します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">使用する配列</param>
        /// <returns><paramref name="array"/>のコピーを持つ<see cref="LongArray{T}"/>の新しいインスタンス</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        public static LongArray<T> Create<T>(params T[] array)
        {
            ArgumentNullException.ThrowIfNull(array);

            if (array.Length == 0) return LongArray<T>.Empty;

            return new LongArray<T>(ref MemoryMarshal.GetArrayDataReference(array), array.Length);
        }

        /// <summary>
        /// 配列から<see cref="LongArray{T}"/>の新しいインスタンスを生成します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">使用する配列</param>
        /// <returns><paramref name="array"/>のコピーを持つ<see cref="LongArray{T}"/>の新しいインスタンス</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        public static LongArray<T> Create<T>(ReadOnlySpan<T> array)
        {
            if (array.Length == 0) return LongArray<T>.Empty;

            return new LongArray<T>(ref MemoryMarshal.GetReference(array), array.Length);
        }
    }
}
