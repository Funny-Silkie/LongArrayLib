using LongArrayLib.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LongArrayLib
{
    /// <summary>
    /// メモリ領域処理の拡張を記述します。
    /// </summary>
    public static class LongMemoryExtensions
    {
        /// <summary>
        /// <see cref="Span{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this LongArray<T>? array)
        {
            if (array is null) return Span<T>.Empty;

            ref T reference = ref array.GetReference();
            return MemoryMarshal.CreateSpan(ref reference, checked((int)(array.Length)));
        }

        /// <summary>
        /// <see cref="Span{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外</exception>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this LongArray<T>? array, int start)
        {
            if (array is null)
            {
                if (start != 0) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

                return Span<T>.Empty;
            }

            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            ref T reference = ref array[start];
            return MemoryMarshal.CreateSpan(ref reference, checked((int)(array.Length - start)));
        }

        /// <summary>
        /// <see cref="Span{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外</exception>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this LongArray<T>? array, long start)
        {
            if (array is null)
            {
                if (start != 0) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

                return Span<T>.Empty;
            }

            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            ref T reference = ref array[start];
            return MemoryMarshal.CreateSpan(ref reference, checked((int)(array.Length - start)));
        }

        /// <summary>
        /// <see cref="Span{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外</exception>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this LongArray<T>? array, Index start)
        {
            long actualStart;
            try
            {
                actualStart = start.GetLongOffset(array?.Length ?? 0L);
            }
            catch (IndexOutOfRangeException)
            {
                ThrowHelper.ThrowAsInvalidIndex(nameof(start));
                throw;
            }

            if (array is null) return Span<T>.Empty;

            ref T reference = ref array[actualStart];
            return MemoryMarshal.CreateSpan(ref reference, checked((int)(array.Length - actualStart)));
        }

        /// <summary>
        /// <see cref="Span{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <param name="length">要素数</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>または<paramref name="length"/>が範囲外</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this LongArray<T>? array, long start, int length)
        {
            if (array is null)
            {
                if (start != 0) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
                if (length != 0) ThrowHelper.ThrowAsLargerLength(nameof(length));

                return Span<T>.Empty;
            }

            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(length);
            if (start + length > array.Length) ThrowHelper.ThrowAsLargerLength(nameof(length));

            ref T reference = ref array[start];
            return MemoryMarshal.CreateSpan(ref reference, length);
        }

        /// <summary>
        /// <see cref="Span{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="range">取り出す範囲</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="range"/>が範囲外</exception>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this LongArray<T>? array, Range range)
        {
            long start, end;
            try
            {
                (start, end) = range.GetLongOffsets(array?.Length ?? 0L);
            }
            catch (IndexOutOfRangeException)
            {
                ThrowHelper.ThrowAsInvalidRange(nameof(range));
                throw;
            }

            if (array is null || start > end) return Span<T>.Empty;
            int length = checked((int)(end - start + 1));

            ref T reference = ref array[start];
            return MemoryMarshal.CreateSpan(ref reference, length);
        }

        /// <summary>
        /// <see cref="Memory{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        public static Memory<T> AsMemory<T>(this LongArray<T>? array)
        {
            if (array is null) return Memory<T>.Empty;

            using var manager = new LongArrayMemoryManager<T>(array, 0, checked((int)array.Length));
            return manager.Memory;
        }

        /// <summary>
        /// <see cref="Memory{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外</exception>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        public static Memory<T> AsMemory<T>(this LongArray<T>? array, int start)
        {
            if (array is null)
            {
                if (start != 0) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

                return Memory<T>.Empty;
            }

            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            using var manager = new LongArrayMemoryManager<T>(array, start, checked((int)(array.Length - start)));
            return manager.Memory;
        }

        /// <summary>
        /// <see cref="Memory{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外</exception>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        public static Memory<T> AsMemory<T>(this LongArray<T>? array, long start)
        {
            if (array is null)
            {
                if (start != 0) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

                return Memory<T>.Empty;
            }

            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            using var manager = new LongArrayMemoryManager<T>(array, start, checked((int)(array.Length - start)));
            return manager.Memory;
        }

        /// <summary>
        /// <see cref="Memory{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外</exception>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        public static Memory<T> AsMemory<T>(this LongArray<T>? array, Index start)
        {
            long actualStart;
            try
            {
                actualStart = start.GetLongOffset(array?.Length ?? 0L);
            }
            catch (IndexOutOfRangeException)
            {
                ThrowHelper.ThrowAsInvalidIndex(nameof(start));
                throw;
            }

            if (array is null) return Memory<T>.Empty;

            using var manager = new LongArrayMemoryManager<T>(array, actualStart, checked((int)(array.Length - actualStart)));
            return manager.Memory;
        }

        /// <summary>
        /// <see cref="Memory{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <param name="length">要素数</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>または<paramref name="length"/>が範囲外</exception>
        public static Memory<T> AsMemory<T>(this LongArray<T>? array, long start, int length)
        {
            if (array is null)
            {
                if (start != 0) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
                if (length != 0) ThrowHelper.ThrowAsLargerLength(nameof(length));

                return Memory<T>.Empty;
            }

            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(length);
            if (start + length > array.Length) ThrowHelper.ThrowAsLargerLength(nameof(length));

            using var manager = new LongArrayMemoryManager<T>(array, start, length);
            return manager.Memory;
        }

        /// <summary>
        /// <see cref="Memory{T}"/>に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">変換する配列</param>
        /// <param name="range">取り出す範囲</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="range"/>が範囲外</exception>
        /// <exception cref="OverflowException">取り出す要素数が<see cref="int.MaxValue"/>を超える</exception>
        public static Memory<T> AsMemory<T>(this LongArray<T>? array, Range range)
        {
            long start, end;
            try
            {
                (start, end) = range.GetLongOffsets(array?.Length ?? 0L);
            }
            catch (IndexOutOfRangeException)
            {
                ThrowHelper.ThrowAsInvalidRange(nameof(range));
                throw;
            }

            if (array is null || start > end) return Memory<T>.Empty;
            int length = checked((int)(end - start + 1));

            using var manager = new LongArrayMemoryManager<T>(array, start, length);
            return manager.Memory;
        }

        /// <summary>
        /// <see cref="LongArray{T}"/>へ変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="memory">変換する値</param>
        /// <returns>変換後の値</returns>
        public static LongArray<T> ToLongArray<T>(this Memory<T> memory)
        {
            return new LongArray<T>(ref MemoryMarshal.GetReference(memory.Span), memory.Length);
        }

        /// <summary>
        /// <see cref="LongArray{T}"/>へ変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="memory">変換する値</param>
        /// <returns>変換後の値</returns>
        public static LongArray<T> ToLongArray<T>(this ReadOnlyMemory<T> memory)
        {
            return new LongArray<T>(ref MemoryMarshal.GetReference(memory.Span), memory.Length);
        }

        /// <summary>
        /// <see cref="LongArray{T}"/>へ変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="span">変換する値</param>
        /// <returns>変換後の値</returns>
        public static LongArray<T> ToLongArray<T>(this Span<T> span)
        {
            return new LongArray<T>(ref MemoryMarshal.GetReference(span), span.Length);
        }

        /// <summary>
        /// <see cref="LongArray{T}"/>へ変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="span">変換する値</param>
        /// <returns>変換後の値</returns>
        public static LongArray<T> ToLongArray<T>(this ReadOnlySpan<T> span)
        {
            return new LongArray<T>(ref MemoryMarshal.GetReference(span), span.Length);
        }

        /// <summary>
        /// <see cref="LongArray{T}"/>へ変換します。
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/>が<see langword="null"/></exception>
        public static LongArray<char> ToLongArray(this string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.Length == 0) return LongArray<char>.Empty;

            ref char reference = ref MemoryMarshal.GetReference<char>(value);
            return new LongArray<char>(ref reference, value.Length);
        }
    }
}
