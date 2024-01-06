using LongArrayLib.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LongArrayLib
{
    public partial class LongArray<T>
    {
        /// <summary>
        /// 指定した要素の有無を検証します。
        /// </summary>
        /// <param name="item">検証する要素</param>
        /// <returns><paramref name="item"/>が格納されていたら<see langword="true"/>，それ以外で<see langword="false"/></returns>
        public bool Contains(T item) => IndexOf(item) >= 0;

        /// <summary>
        /// 指定した要素のうち最初に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <returns><paramref name="item"/>のうち最初に出現するもののインデックス，見つからなかったら-1</returns>
        public long IndexOf(T item) => IndexOfCore(item, 0L, Length);

        /// <summary>
        /// 指定した要素のうち最初に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <param name="start">検索開始インデックス</param>
        /// <returns><paramref name="item"/>のうち最初に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または0未満</exception>
        public long IndexOf(T item, long start)
        {
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            return IndexOfCore(item, start, Length - start);
        }

        /// <summary>
        /// 指定した要素のうち最初に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="count">検索範囲</param>
        /// <returns><paramref name="item"/>のうち最初に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        public long IndexOf(T item, long start, long count)
        {
            ThrowHelper.ThrowIfNegative(start);
            ThrowHelper.ThrowIfNegative(count);
            if ((ulong)start + (ulong)count > (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            return IndexOfCore(item, start, count);
        }

        /// <summary>
        /// 指定した要素のうち最初に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="count">検索範囲</param>
        /// <returns><paramref name="item"/>のうち最初に出現するもののインデックス，見つからなかったら-1</returns>
        private long IndexOfCore(T item, long start, long count)
        {
            if (count == 0) return -1;

            ref T reference = ref this[start];

            if (item is null)
            {
                for (long i = start; i < start + count; i++)
                {
                    if (reference is null) return i;
                    reference = ref Unsafe.Add(ref reference, 1);
                }

                return -1;
            }

            for (long i = start; i < start + count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(reference, item)) return i;
                reference = ref Unsafe.Add(ref reference, 1);
            }

            return -1;
        }

        /// <summary>
        /// 指定した要素のうち最後に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <returns><paramref name="item"/>のうち最後に出現するもののインデックス，見つからなかったら-1</returns>
        public long LastIndexOf(T item)
        {
            return LastIndexOfCore(item, Length - 1, Length);
        }

        /// <summary>
        /// 指定した要素のうち最後に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <param name="start">検索開始インデックス</param>
        /// <returns><paramref name="item"/>のうち最後に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または0未満</exception>
        public long LastIndexOf(T item, long start)
        {
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            return LastIndexOfCore(item, start, start + 1);
        }

        /// <summary>
        /// 指定した要素のうち最後に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="count">検索範囲</param>
        /// <returns><paramref name="item"/>のうち最後に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        public long LastIndexOf(T item, long start, long count)
        {
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start - count + 1 < 0) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            return LastIndexOfCore(item, start, count);
        }

        /// <summary>
        /// 指定した要素のうち最後に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="item">検索する要素</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="count">検索範囲</param>
        /// <returns><paramref name="item"/>のうち最後に出現するもののインデックス，見つからなかったら-1</returns>
        private long LastIndexOfCore(T item, long start, long count)
        {
            if (count == 0) return -1;

            ref T reference = ref this[start];

            if (item is null)
            {
                for (long i = start; i >= start - count + 1; i--)
                {
                    if (reference is null) return i;
                    reference = ref Unsafe.Subtract(ref reference, 1);
                }

                return -1;
            }

            for (long i = start; i >= start - count + 1; i--)
            {
                if (EqualityComparer<T>.Default.Equals(reference, item)) return i;
                reference = ref Unsafe.Subtract(ref reference, 1);
            }

            return -1;
        }

        /// <summary>
        /// 指定した条件に適合する要素の有無を検証します。
        /// </summary>
        /// <param name="match">検証する要素の条件</param>
        /// <returns><paramref name="match"/>に適合する要素が格納されていたら<see langword="true"/>，それ以外で<see langword="false"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        public bool Exists(Predicate<T> match) => FindIndex(match) >= 0;

        /// <summary>
        /// 全ての要素が指定した条件に適合かどうかを検証します。
        /// </summary>
        /// <param name="match">検証する要素の条件</param>
        /// <returns>全ての要素が<paramref name="match"/>に適合していたら<see langword="true"/>，それ以外で<see langword="false"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        public bool TrueForAll(Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(match);

            if (Length == 0) return true;

            ref T reference = ref GetReference();

            for (long i = 0; i < Length; i++)
            {
                if (!match.Invoke(reference)) return false;
                reference = ref Unsafe.Add(ref reference, 1);
            }

            return true;
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最初に出現するものを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <returns><paramref name="match"/>に適合するもののうち最初に出現するもの，見つからなかったら既定値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        public T? Find(Predicate<T> match)
        {
            long index = FindIndex(match);
            return index < 0 ? default : this[index];
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最初に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <returns><paramref name="match"/>に適合するもののうち最初に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        public long FindIndex(Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(match);

            return FindIndexCore(match, 0L, Length);
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最初に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <param name="start">検索開始インデックス</param>
        /// <returns><paramref name="match"/>に適合するもののうち最初に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外</exception>
        public long FindIndex(Predicate<T> match, long start)
        {
            ArgumentNullException.ThrowIfNull(match);
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            return FindIndexCore(match, start, Length - start);
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最初に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="count">検索範囲</param>
        /// <returns><paramref name="match"/>に適合するもののうち最初に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        public long FindIndex(Predicate<T> match, long start, long count)
        {
            ArgumentNullException.ThrowIfNull(match);
            ThrowHelper.ThrowIfNegative(start);
            ThrowHelper.ThrowIfNegative(count);
            if ((ulong)start + (ulong)count > (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            return FindIndexCore(match, start, count);
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最初に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="count">検索範囲</param>
        /// <returns><paramref name="match"/>に適合するもののうち最初に出現するもののインデックス，見つからなかったら-1</returns>
        private long FindIndexCore(Predicate<T> match, long start, long count)
        {
            if (count == 0) return -1;

            ref T reference = ref this[start];

            for (long i = start; i < start + count; i++)
            {
                if (match.Invoke(reference)) return i;
                reference = ref Unsafe.Add(ref reference, 1);
            }

            return -1;
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最後に出現するものを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <returns><paramref name="match"/>に適合するもののうち最後に出現するもの，見つからなかったら既定値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        public T? FindLast(Predicate<T> match)
        {
            long index = FindLastIndex(match);
            return index < 0 ? default : this[index];
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最後に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <returns><paramref name="match"/>に適合するもののうち最後に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        public long FindLastIndex(Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(match);

            return FindLastIndexCore(match, Length - 1, Length);
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最後に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <param name="start">検索開始インデックス</param>
        /// <returns><paramref name="match"/>に適合するもののうち最後に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外</exception>
        public long FindLastIndex(Predicate<T> match, long start)
        {
            ArgumentNullException.ThrowIfNull(match);
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            return FindLastIndexCore(match, start, start + 1);
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最後に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="count">検索範囲</param>
        /// <returns><paramref name="match"/>に適合するもののうち最後に出現するもののインデックス，見つからなかったら-1</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>が範囲外または<paramref name="count"/>が0未満</exception>
        public long FindLastIndex(Predicate<T> match, long start, long count)
        {
            ArgumentNullException.ThrowIfNull(match);
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start - count + 1 < 0) ThrowHelper.ThrowAsInvalidIndex(nameof(start));

            return FindLastIndexCore(match, start, count);
        }

        /// <summary>
        /// 指定した条件に適合する要素を全て取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <returns><paramref name="match"/>に適合する全ての要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/>が<see langword="null"/></exception>
        public LongArray<T> FindAll(Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(match);

            if (Length == 0) return Empty;

            using var builder = new LongArrayBuilder<T>(Length);

            ref T reference = ref GetReference();
            for (long i = 0; i < Length; i++)
            {
                if (match.Invoke(reference)) builder.Add(reference);
                reference = ref Unsafe.Add(ref reference, 1);
            }

            return builder.ToArray();
        }

        /// <summary>
        /// 指定した条件に適合する要素のうち最後に出現するもののインデックスを取得します。
        /// </summary>
        /// <param name="match">検索する要素の条件</param>
        /// <param name="start">検索開始インデックス</param>
        /// <param name="count">検索範囲</param>
        /// <returns><paramref name="match"/>に適合するもののうち最後に出現するもののインデックス，見つからなかったら-1</returns>
        private long FindLastIndexCore(Predicate<T> match, long start, long count)
        {
            if (count == 0) return -1;

            ref T reference = ref this[start];

            for (long i = start; i >= start - count + 1; i--)
            {
                if (match.Invoke(reference)) return i;
                reference = ref Unsafe.Subtract(ref reference, 1);
            }

            return -1;
        }

        /// <summary>
        /// 二分岐探索を行います。
        /// </summary>
        /// <param name="value">探索する値</param>
        /// <returns>要素のインデックス。見つからない場合は負の値</returns>
        /// <exception cref="InvalidOperationException"><typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public long BinarySearch(T value)
        {
            return BinarySearchCore(0, Length - 1, value, Comparer<T>.Default);
        }

        /// <summary>
        /// 二分岐探索を行います。
        /// </summary>
        /// <param name="value">探索する値</param>
        /// <param name="comparer">使用する比較演算子</param>
        /// <returns>要素のインデックス。見つからない場合は負の値</returns>
        /// <exception cref="InvalidOperationException"><paramref name="comparer"/>が<see langword="null"/>のとき，<typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public long BinarySearch(T value, IComparer<T>? comparer)
        {
            return BinarySearchCore(0, Length - 1, value, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// 二分岐探索を行います。
        /// </summary>
        /// <param name="start">探索開始インデックス</param>
        /// <param name="count">探索領域</param>
        /// <param name="value">探索する値</param>
        /// <returns>要素のインデックス。見つからない場合は負の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>または<paramref name="count"/>が範囲外</exception>
        /// <exception cref="InvalidOperationException"><see langword="null"/>のとき，<typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public long BinarySearch(long start, long count, T value)
        {
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > Length) ThrowHelper.ThrowAsOutOfArraySize(nameof(count));

            return BinarySearchCore(start, start + count - 1, value, Comparer<T>.Default);
        }

        /// <summary>
        /// 二分岐探索を行います。
        /// </summary>
        /// <param name="start">探索開始インデックス</param>
        /// <param name="count">探索領域</param>
        /// <param name="value">探索する値</param>
        /// <param name="comparer">使用する比較演算子</param>
        /// <returns>要素のインデックス。見つからない場合は負の値</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>または<paramref name="count"/>が範囲外</exception>
        /// <exception cref="InvalidOperationException"><paramref name="comparer"/>が<see langword="null"/>のとき，<typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        public long BinarySearch(long start, long count, T value, IComparer<T>? comparer)
        {
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > Length) ThrowHelper.ThrowAsOutOfArraySize(nameof(count));

            return BinarySearchCore(start, start + count - 1, value, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// 二分岐探索を行います。
        /// </summary>
        /// <param name="start">探索開始インデックス</param>
        /// <param name="end">探索終了インデックス</param>
        /// <param name="value">探索する値</param>
        /// <param name="comparer">使用する比較演算子</param>
        /// <returns>要素のインデックス。見つからない場合は負の値</returns>
        /// <exception cref="InvalidOperationException"><paramref name="comparer"/>が<see cref="Comparer{T}.Default"/>のとき，<typeparamref name="T"/>が<see cref="IComparable{T}"/>を実装していない</exception>
        private long BinarySearchCore(long start, long end, T value, IComparer<T> comparer)
        {
            try
            {
                while (start <= end)
                {
                    long midIndex = (start + end) / 2;
                    T mid = this[midIndex];

                    int comp;
                    comp = comparer.Compare(mid, value);
                    if (comp == 0) return midIndex;
                    if (comp < 0) start = midIndex + 1;
                    else end = midIndex - 1;
                }
                return ~start;
            }
            catch (ArgumentException)
            {
                ThrowHelper.ThrowAsNotCompareble();
                throw;
            }
        }

        #region Explicit Interface Implementation

        #region IList

        bool IList.Contains(object? value)
        {
            if (value is null && default(T) is null) return Contains(default!);
            if (value is T t) return Contains(t);
            return false;
        }

        int IList.IndexOf(object? value)
        {
            if (value is null && default(T) is null) return checked((int)IndexOf(default!));
            if (value is T t) return checked((int)IndexOf(t));
            return -1;
        }

        #endregion IList

        #region IList`1

        int IList<T>.IndexOf(T item) => checked((int)IndexOf(item));

        #endregion IList`1

        #endregion Explicit Interface Implementation
    }
}
