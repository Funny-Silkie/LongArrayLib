using LongArrayLib.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LongArrayLib
{
    public partial class LongArray
    {
        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public static void Sort<T>(LongArray<T> array)
        {
            ArgumentNullException.ThrowIfNull(array);

            SortHelper.IntroSort(array, 0L, array.Length, Comparer<T>.Default);
        }

        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public static void Sort<T>(LongArray<T> array, long start, long count)
        {
            ArgumentNullException.ThrowIfNull(array);
            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > array.Length) ThrowHelper.ThrowAsShortArray(nameof(array));

            SortHelper.IntroSort(array, start, count, Comparer<T>.Default);
        }

        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="comparer">要素の比較演算子</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="comparer"/>の比較が無効</exception>
        /// <exception cref="InvalidOperationException"><paramref name="comparer"/>が<see langword="null"/>のとき，<typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public static void Sort<T>(LongArray<T> array, IComparer<T>? comparer)
        {
            ArgumentNullException.ThrowIfNull(array);

            SortHelper.IntroSort(array, 0L, array.Length, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">要素の比較演算子</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/>のサイズが不足または<paramref name="comparer"/>の比較が無効</exception>
        /// <exception cref="InvalidOperationException"><paramref name="comparer"/>が<see langword="null"/>のとき，<typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public static void Sort<T>(LongArray<T> array, long start, long count, IComparer<T>? comparer)
        {
            ArgumentNullException.ThrowIfNull(array);
            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > array.Length) ThrowHelper.ThrowAsShortArray(nameof(array));

            SortHelper.IntroSort(array, start, count, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="comparison">比較関数</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="comparison"/>の比較が無効</exception>
        public static void Sort<T>(LongArray<T> array, Comparison<T> comparison)
        {
            ArgumentNullException.ThrowIfNull(array);
            ArgumentNullException.ThrowIfNull(comparison);

            SortHelper.IntroSort(array, 0L, array.Length, Comparer<T>.Create(comparison));
        }

        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="keys">ソートに用いるキーの配列</param>
        /// <param name="values">ソートされる値の配列</param>
        /// <exception cref="ArgumentNullException"><paramref name="keys"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="values"/>のサイズが不足</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="TKey"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public static void Sort<TKey, TValue>(LongArray<TKey> keys, LongArray<TValue>? values)
        {
            if (values is null)
            {
                Sort(keys);
                return;
            }

            ArgumentNullException.ThrowIfNull(keys);
            if (values.Length < keys.Length) ThrowHelper.ThrowAsShortArray(nameof(values));

            SortHelper.IntroSort(keys, values, 0L, keys.Length, Comparer<TKey>.Default);
        }

        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="keys">ソートに用いるキーの配列</param>
        /// <param name="values">ソートされる値の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="keys"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="keys"/>や<paramref name="values"/>のサイズが不足</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="TKey"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public static void Sort<TKey, TValue>(LongArray<TKey> keys, LongArray<TValue>? values, long start, long count)
        {
            if (values is null)
            {
                Sort(keys, start, count);
                return;
            }

            ArgumentNullException.ThrowIfNull(keys);
            if ((ulong)start >= (ulong)keys.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            if ((ulong)start >= (ulong)values.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > keys.Length) ThrowHelper.ThrowAsShortArray(nameof(keys));
            if (start + count > values.Length) ThrowHelper.ThrowAsShortArray(nameof(values));

            SortHelper.IntroSort(keys, values, start, count, Comparer<TKey>.Default);
        }

        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="keys">ソートに用いるキーの配列</param>
        /// <param name="values">ソートされる値の配列</param>
        /// <param name="comparer">要素の比較演算子</param>
        /// <exception cref="ArgumentNullException"><paramref name="keys"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="values"/>のサイズが不足または<paramref name="comparer"/>の比較が無効</exception>
        /// <exception cref="InvalidOperationException"><paramref name="comparer"/>が<see langword="null"/>のとき，<typeparamref name="TKey"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public static void Sort<TKey, TValue>(LongArray<TKey> keys, LongArray<TValue>? values, IComparer<TKey>? comparer)
        {
            if (values is null)
            {
                Sort(keys, comparer);
                return;
            }

            ArgumentNullException.ThrowIfNull(keys);
            if (values.Length < keys.Length) ThrowHelper.ThrowAsShortArray(nameof(values));

            SortHelper.IntroSort(keys, values, 0L, keys.Length, comparer ?? Comparer<TKey>.Default);
        }

        /// <summary>
        /// 並び替えを行います。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="keys">ソートに用いるキーの配列</param>
        /// <param name="values">ソートされる値の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">要素の比較演算子</param>
        /// <exception cref="ArgumentNullException"><paramref name="keys"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        /// <exception cref="ArgumentException"><paramref name="keys"/>や<paramref name="values"/>のサイズが不足または<paramref name="comparer"/>の比較が無効</exception>
        /// <exception cref="InvalidOperationException"><paramref name="comparer"/>が<see langword="null"/>のとき，<typeparamref name="TKey"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public static void Sort<TKey, TValue>(LongArray<TKey> keys, LongArray<TValue>? values, long start, long count, IComparer<TKey>? comparer)
        {
            if (values is null)
            {
                Sort(keys, start, count, comparer);
                return;
            }

            ArgumentNullException.ThrowIfNull(keys);
            if ((ulong)start >= (ulong)keys.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            if ((ulong)start >= (ulong)values.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > keys.Length) ThrowHelper.ThrowAsShortArray(nameof(keys));
            if (start + count > values.Length) ThrowHelper.ThrowAsShortArray(nameof(values));

            SortHelper.IntroSort(keys, values, start, count, comparer ?? Comparer<TKey>.Default);
        }

        /// <summary>
        /// 順序を逆転させます。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        public static void Reverse<T>(LongArray<T> array)
        {
            ArgumentNullException.ThrowIfNull(array);

            ReverseCore(array, 0L, array.Length);
        }

        /// <summary>
        /// 順序を逆転させます。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>のサイズが不足</exception>
        public static void Reverse<T>(LongArray<T> array, long start, long count)
        {
            ArgumentNullException.ThrowIfNull(array);
            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > array.Length) ThrowHelper.ThrowAsShortArray(nameof(array));

            ReverseCore(array, start, count);
        }

        /// <summary>
        /// 順序を逆転させます。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        private static void ReverseCore<T>(LongArray<T> array, long start, long count)
        {
            if (count <= 1) return;

            ref T forwardReference = ref array[start];
            ref T reverseReference = ref array[start + count - 1];

            do
            {
                (reverseReference, forwardReference) = (forwardReference, reverseReference);

                forwardReference = ref Unsafe.Add(ref forwardReference, 1);
                reverseReference = ref Unsafe.Subtract(ref reverseReference, 1);
            }
            while (Unsafe.IsAddressLessThan(ref forwardReference, ref reverseReference));
        }
    }
}
