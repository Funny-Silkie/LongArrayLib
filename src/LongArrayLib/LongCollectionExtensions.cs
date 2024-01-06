using LongArrayLib.Internal;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LongArrayLib
{
    /// <summary>
    /// コレクションの拡張を記述します。
    /// </summary>
    public static class LongCollectionExtensions
    {
        /// <summary>
        /// <see cref="LongArray{T}"/>へ変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="list">変換する値</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="list"/>が<see langword="null"/></exception>
        public static LongArray<T> ToLongArray<T>(this List<T> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            return CollectionsMarshal.AsSpan(list).ToLongArray();
        }

        /// <summary>
        /// 配列へ要素をコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="collection">ソースとなるコレクション</param>
        /// <param name="array">コピー先の配列</param>
        /// <param name="arrayIndex"><paramref name="array"/>におけるコピー開始インデックス</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/>または<paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足</exception>
        public static unsafe void CopyTo<T>(this ICollection<T> collection, LongArray<T> array, long arrayIndex)
        {
            switch (collection)
            {
                case T[] sa: sa.CopyTo(array, arrayIndex); return;
                case LongArray<T> la: la.CopyTo(array, arrayIndex); return;
                case List<T> ls: ls.CopyTo(array, arrayIndex); return;
            }

            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(array);
            ThrowHelper.ThrowIfNegative(arrayIndex);
            if (arrayIndex + collection.Count > array.Length) ThrowHelper.ThrowAsShortArray(nameof(arrayIndex));

            T[] buffer = ArrayPool<T>.Shared.Rent(collection.Count);
            try
            {
                collection.CopyTo(buffer, 0);

                long bytesToCopy = (long)collection.Count * Unsafe.SizeOf<T>();
                void* arrayPtr = UnsafeHelper.Increment<T>(array.AsPointer(), arrayIndex);
                fixed (void* bufferPtr = buffer)
                {
                    Buffer.MemoryCopy(bufferPtr, arrayPtr, bytesToCopy, bytesToCopy);
                }
            }
            finally
            {
                ArrayPool<T>.Shared.Return(buffer);
            }
        }

        /// <summary>
        /// 配列へ要素をコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="list">ソースとなる<see cref="List{T}"/>のインスタンス</param>
        /// <param name="array">コピー先の配列</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/>または<paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足</exception>
        public static unsafe void CopyTo<T>(this List<T> list, LongArray<T> array)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(array);

            if (list.Count == 0) return;

            Span<T> listSpan = CollectionsMarshal.AsSpan(list);
            void* arrayPtr = array.AsPointer();
            long bytesToCopy = (long)list.Count * Unsafe.SizeOf<T>();
            fixed (void* listPtr = listSpan)
            {
                Buffer.MemoryCopy(listPtr, arrayPtr, bytesToCopy, bytesToCopy);
            }
        }

        /// <summary>
        /// 配列へ要素をコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="list">ソースとなる<see cref="List{T}"/>のインスタンス</param>
        /// <param name="array">コピー先の配列</param>
        /// <param name="arrayIndex"><paramref name="array"/>におけるコピー開始インデックス</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/>または<paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足</exception>
        public static unsafe void CopyTo<T>(this List<T> list, LongArray<T> array, long arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(array);
            ThrowHelper.ThrowIfNegative(arrayIndex);
            if (arrayIndex + list.Count > array.Length) ThrowHelper.ThrowAsShortArray(nameof(arrayIndex));

            if (list.Count == 0) return;

            Span<T> listSpan = CollectionsMarshal.AsSpan(list);
            void* arrayPtr = UnsafeHelper.Increment<T>(array.AsPointer(), arrayIndex);
            long bytesToCopy = (long)list.Count * Unsafe.SizeOf<T>();
            fixed (void* listPtr = listSpan)
            {
                Buffer.MemoryCopy(listPtr, arrayPtr, bytesToCopy, bytesToCopy);
            }
        }

        /// <summary>
        /// 配列へ要素をコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="list">ソースとなる<see cref="List{T}"/>のインスタンス</param>
        /// <param name="index"><paramref name="list"/>におけるコピー開始インデックス</param>
        /// <param name="array">コピー先の配列</param>
        /// <param name="arrayIndex"><paramref name="array"/>におけるコピー開始インデックス</param>
        /// <param name="count">コピーする要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/>または<paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>，<paramref name="arrayIndex"/>，<paramref name="count"/>の何れかが0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足または<paramref name="index"/>と<paramref name="count"/>によるインデックスが<paramref name="list"/>の範囲外</exception>
        public static unsafe void CopyTo<T>(this List<T> list, int index, LongArray<T> array, long arrayIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(array);
            ThrowHelper.ThrowIfNegative(index);
            ThrowHelper.ThrowIfNegative(arrayIndex);
            ThrowHelper.ThrowIfNegative(count);
            if (index + count > list.Count) ThrowHelper.ThrowAsInvalidRangeOfCollection(nameof(count));
            if (arrayIndex + count > array.Length) ThrowHelper.ThrowAsShortArray(nameof(arrayIndex));

            if (count == 0) return;

            Span<T> listSpan = CollectionsMarshal.AsSpan(list).Slice(index, count);
            void* arrayPtr = UnsafeHelper.Increment<T>(array.AsPointer(), arrayIndex);
            long bytesToCopy = (long)count * Unsafe.SizeOf<T>();
            fixed (void* listPtr = listSpan)
            {
                Buffer.MemoryCopy(listPtr, arrayPtr, bytesToCopy, bytesToCopy);
            }
        }

        /// <summary>
        /// 配列へ要素をコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="stack">ソースとなる<see cref="Stack{T}"/>のインスタンス</param>
        /// <param name="array">コピー先の配列</param>
        /// <param name="arrayIndex"><paramref name="array"/>におけるコピー開始インデックス</param>
        /// <exception cref="ArgumentNullException"><paramref name="stack"/>または<paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足</exception>
        public static unsafe void CopyTo<T>(this Stack<T> stack, LongArray<T> array, long arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(stack);
            ArgumentNullException.ThrowIfNull(array);
            ThrowHelper.ThrowIfNegative(arrayIndex);
            if (arrayIndex + stack.Count > array.Length) ThrowHelper.ThrowAsShortArray(nameof(arrayIndex));

            if (stack.Count == 0) return;

            void* arrayPtr = UnsafeHelper.Increment<T>(array.AsPointer(), arrayIndex);
            long bytesToCopy = (long)stack.Count * Unsafe.SizeOf<T>();
            UnsafeHelper.GetInnerArray(stack, out Span<T> stackSpan);
            fixed (void* stackPtr = stackSpan)
            {
                Buffer.MemoryCopy(stackPtr, arrayPtr, bytesToCopy, bytesToCopy);
            }
        }

        /// <summary>
        /// 配列へ要素をコピーします。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="queue">ソースとなる<see cref="Queue{T}"/>のインスタンス</param>
        /// <param name="array">コピー先の配列</param>
        /// <param name="arrayIndex"><paramref name="array"/>におけるコピー開始インデックス</param>
        /// <exception cref="ArgumentNullException"><paramref name="queue"/>または<paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足</exception>
        public static unsafe void CopyTo<T>(this Queue<T> queue, LongArray<T> array, long arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(queue);
            ArgumentNullException.ThrowIfNull(array);
            ThrowHelper.ThrowIfNegative(arrayIndex);
            if (arrayIndex + queue.Count > array.Length) ThrowHelper.ThrowAsShortArray(nameof(arrayIndex));

            if (queue.Count == 0) return;

            void* arrayPtr = UnsafeHelper.Increment<T>(array.AsPointer(), arrayIndex);
            UnsafeHelper.GetInnerArray(queue, out Span<T> head, out Span<T> tail);

            if (head.Length > 0)
            {
                long bytesToCopy = (long)head.Length * Unsafe.SizeOf<T>();
                fixed (void* queuePtr = head)
                {
                    Buffer.MemoryCopy(queuePtr, arrayPtr, bytesToCopy, bytesToCopy);
                }
                arrayPtr = (byte*)arrayPtr + bytesToCopy;
            }

            if (tail.Length > 0)
            {
                long bytesToCopy = (long)tail.Length * Unsafe.SizeOf<T>();
                fixed (void* queuePtr = tail)
                {
                    Buffer.MemoryCopy(queuePtr, arrayPtr, bytesToCopy, bytesToCopy);
                }
            }
        }
    }
}
