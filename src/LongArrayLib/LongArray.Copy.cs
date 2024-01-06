using LongArrayLib.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LongArrayLib
{
    public partial class LongArray<T> : IList<T>, IReadOnlyList<T>, IList, IDisposable, ICloneable
    {
        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index"><paramref name="array"/>における貼り付け開始インデックス</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足</exception>
        public unsafe void CopyTo(T[] array, int index)
        {
            ArgumentNullException.ThrowIfNull(array);
            ThrowHelper.ThrowIfNegative(index);
            if (index + Length > array.Length) ThrowHelper.ThrowAsShortArray(nameof(array));

            long bytesToCopy = Length * Unsafe.SizeOf<T>();

            fixed (void* destPtr = array)
            {
                Buffer.MemoryCopy(items, UnsafeHelper.Increment<T>(destPtr, index), bytesToCopy, bytesToCopy);
            }
        }

        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index"><paramref name="array"/>における貼り付け開始インデックス</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足</exception>
        public unsafe void CopyTo(LongArray<T> array, long index)
        {
            ArgumentNullException.ThrowIfNull(array);
            ThrowHelper.ThrowIfNegative(index);
            if (index + Length > array.Length) ThrowHelper.ThrowAsShortArray(nameof(array));

            long bytesToCopy = Length * Unsafe.SizeOf<T>();
            Buffer.MemoryCopy(items, UnsafeHelper.Increment<T>(array.items, index), bytesToCopy, bytesToCopy);
        }

        /// <summary>
        /// 要素をメモリ領域にコピーします。
        /// </summary>
        /// <param name="destination">コピー先の領域</param>
        /// <exception cref="ArgumentException"><paramref name="destination"/>のサイズが不足</exception>
        public unsafe void CopyTo(Memory<T> destination)
        {
            if (Length > destination.Length) ThrowHelper.ThrowAsShortMemory(nameof(destination));

            long bytesToCopy = Length * Unsafe.SizeOf<T>();
            fixed (void* destPtr = destination.Span)
            {
                Buffer.MemoryCopy(items, destPtr, bytesToCopy, bytesToCopy);
            }
        }

        /// <summary>
        /// 要素をメモリ領域にコピーします。
        /// </summary>
        /// <param name="destination">コピー先の領域</param>
        /// <exception cref="ArgumentException"><paramref name="destination"/>のサイズが不足</exception>
        public unsafe void CopyTo(Span<T> destination)
        {
            if (Length > destination.Length) ThrowHelper.ThrowAsShortMemory(nameof(destination));

            long bytesToCopy = Length * Unsafe.SizeOf<T>();
            fixed (void* destPtr = destination)
            {
                Buffer.MemoryCopy(items, destPtr, bytesToCopy, bytesToCopy);
            }
        }

        /// <inheritdoc/>
        public object Clone()
        {
            if (Length == 0) return Empty;

            return new LongArray<T>(ref GetReference(), Length);
        }

        #region Explicit Interface Implementation

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            ThrowHelper.ThrowIfNegative(index);
            if (index + Length > array.Length) ThrowHelper.ThrowAsShortArray(nameof(array));

            switch (array)
            {
                case T[] t: CopyTo(t, index); return;
                case object?[] o:
                    try
                    {
                        ref T reference = ref GetReference();
                        for (long i = 0; i < Length; i++)
                        {
                            o[index++] = reference;
                            reference = ref Unsafe.Add(ref reference, 1);
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        ThrowHelper.ThrowAsInvalidType(nameof(array));
                    }
                    break;

                default: ThrowHelper.ThrowAsInvalidArray(nameof(array)); break;
            }
        }

        #endregion ICollection

        #endregion Explicit Interface Implementation
    }

    public partial class LongArray
    {
        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">コピー元の配列</param>
        /// <param name="destination">コピー先の配列</param>
        /// <param name="length">コピーする要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="destination"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="source"/>または<paramref name="destination"/>のサイズが不足</exception>
        public static unsafe void Copy<T>(T[] source, LongArray<T> destination, int length)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            ThrowHelper.ThrowIfNegative(length);
            if (length > source.Length) ThrowHelper.ThrowAsShortArray(nameof(source));
            if (length > destination.Length) ThrowHelper.ThrowAsShortArray(nameof(destination));

            long bytesToCopy = checked((long)length * Unsafe.SizeOf<T>());

            fixed (void* sourcePtr = source)
            {
                Buffer.MemoryCopy(sourcePtr, destination.AsPointer(), bytesToCopy, bytesToCopy);
            }
        }

        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">コピー元の配列</param>
        /// <param name="sourceIndex">コピーする範囲の開始インデックス</param>
        /// <param name="destination">コピー先の配列</param>
        /// <param name="destinationIndex"><paramref name="destination"/>における貼り付け開始インデックス</param>
        /// <param name="length">コピーする要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="destination"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="sourceIndex"/>または<paramref name="destinationIndex"/>が範囲外</item>
        /// <item><paramref name="length"/>が0未満</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="source"/>または<paramref name="destination"/>のサイズが不足</exception>
        public static unsafe void Copy<T>(T[] source, int sourceIndex, LongArray<T> destination, long destinationIndex, int length)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            if ((ulong)sourceIndex >= (ulong)source.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(sourceIndex));
            if ((uint)destinationIndex >= (uint)destination.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(destinationIndex));
            ThrowHelper.ThrowIfNegative(length);
            if (sourceIndex + length > source.Length) ThrowHelper.ThrowAsShortArray(nameof(source));
            if (destinationIndex + length > destination.Length) ThrowHelper.ThrowAsShortArray(nameof(destination));

            long bytesToCopy = checked((long)length * Unsafe.SizeOf<T>());

            fixed (void* sourcePtr = source)
            {
                Buffer.MemoryCopy(UnsafeHelper.Increment<T>(sourcePtr, sourceIndex), UnsafeHelper.Increment<T>(destination.AsPointer(), destinationIndex), bytesToCopy, bytesToCopy);
            }
        }

        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">コピー元の配列</param>
        /// <param name="destination">コピー先の配列</param>
        /// <param name="length">コピーする要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="destination"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/>が0未満超える</exception>
        /// <exception cref="ArgumentException"><paramref name="source"/>または<paramref name="destination"/>のサイズが不足</exception>
        public static unsafe void Copy<T>(LongArray<T> source, T[] destination, int length)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            ThrowHelper.ThrowIfNegative(length);
            if (length > source.Length) ThrowHelper.ThrowAsShortArray(nameof(source));
            if (length > destination.Length) ThrowHelper.ThrowAsShortArray(nameof(destination));

            long bytesToCopy = checked(length * Unsafe.SizeOf<T>());
            ref T destinationReference = ref MemoryMarshal.GetArrayDataReference(destination);

            Buffer.MemoryCopy(source.AsPointer(), Unsafe.AsPointer(ref destinationReference), bytesToCopy, bytesToCopy);
        }

        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">コピー元の配列</param>
        /// <param name="sourceIndex">コピーする範囲の開始インデックス</param>
        /// <param name="destination">コピー先の配列</param>
        /// <param name="destinationIndex"><paramref name="destination"/>における貼り付け開始インデックス</param>
        /// <param name="length">コピーする要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="destination"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="sourceIndex"/>または<paramref name="destinationIndex"/>が範囲外</item>
        /// <item><paramref name="length"/>が0未満</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="source"/>または<paramref name="destination"/>のサイズが不足</exception>
        public static unsafe void Copy<T>(LongArray<T> source, long sourceIndex, T[] destination, int destinationIndex, int length)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            if ((ulong)sourceIndex >= (ulong)source.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(sourceIndex));
            if ((uint)destinationIndex >= (uint)destination.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(destinationIndex));
            ThrowHelper.ThrowIfNegative(length);
            if (sourceIndex + length > source.Length) ThrowHelper.ThrowAsShortArray(nameof(source));
            if (destinationIndex + length > destination.Length) ThrowHelper.ThrowAsShortArray(nameof(destination));

            long bytesToCopy = checked(length * Unsafe.SizeOf<T>());
            ref T destinationReference = ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(destination), destinationIndex);

            Buffer.MemoryCopy(UnsafeHelper.Increment<T>(source.AsPointer(), sourceIndex), Unsafe.AsPointer(ref destinationReference), bytesToCopy, bytesToCopy);
        }

        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">コピー元の配列</param>
        /// <param name="destination">コピー先の配列</param>
        /// <param name="length">コピーする要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="destination"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="source"/>または<paramref name="destination"/>のサイズが不足</exception>
        public static unsafe void Copy<T>(LongArray<T> source, LongArray<T> destination, long length)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            ThrowHelper.ThrowIfNegative(length);
            if (length > source.Length) ThrowHelper.ThrowAsShortArray(nameof(source));
            if (length > destination.Length) ThrowHelper.ThrowAsShortArray(nameof(destination));

            long bytesToCopy = checked(length * Unsafe.SizeOf<T>());
            Buffer.MemoryCopy(source.AsPointer(), destination.AsPointer(), bytesToCopy, bytesToCopy);
        }

        /// <summary>
        /// 要素を配列にコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">コピー元の配列</param>
        /// <param name="sourceIndex">コピーする範囲の開始インデックス</param>
        /// <param name="destination">コピー先の配列</param>
        /// <param name="destinationIndex"><paramref name="destination"/>における貼り付け開始インデックス</param>
        /// <param name="length">コピーする要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="destination"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="sourceIndex"/>または<paramref name="destinationIndex"/>が範囲外</item>
        /// <item><paramref name="length"/>が0未満</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="source"/>または<paramref name="destination"/>のサイズが不足</exception>
        public static unsafe void Copy<T>(LongArray<T> source, long sourceIndex, LongArray<T> destination, long destinationIndex, long length)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            if ((ulong)sourceIndex >= (ulong)source.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(sourceIndex));
            if ((ulong)destinationIndex >= (ulong)destination.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(destinationIndex));
            ThrowHelper.ThrowIfNegative(length);
            if (sourceIndex + length > source.Length) ThrowHelper.ThrowAsShortArray(nameof(source));
            if (destinationIndex + length > destination.Length) ThrowHelper.ThrowAsShortArray(nameof(destination));

            long bytesToCopy = checked(length * Unsafe.SizeOf<T>());
            Buffer.MemoryCopy(UnsafeHelper.Increment<T>(source.AsPointer(), sourceIndex), UnsafeHelper.Increment<T>(destination.AsPointer(), destinationIndex), bytesToCopy, bytesToCopy);
        }
    }
}
