using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LongArrayLib.Internal
{
    /// <summary>
    /// ソート処理のヘルパークラスです。
    /// </summary>
    internal static class SortHelper
    {
        /// <summary>
        /// イントロソートから挿入ソートへ切り替える配列長の閾値です。
        /// </summary>
        private const int Intro2InsertThreshold = 16;

        /// <summary>
        /// イントロソートを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">要素の比較演算子</param>
        /// <exception cref="ArgumentException"><paramref name="comparer"/>の比較が無効</exception>
        public static void IntroSort<T>(LongArray<T> array, long start, long count, IComparer<T> comparer)
        {
            if (count <= 1) return;
            try
            {
                IntroSortCore(array, start, count, comparer, 2 * (long.Log2(count) + 1));
            }
            catch (IndexOutOfRangeException)
            {
                ThrowHelper.ThrowAsInvalidComparer(nameof(comparer));
            }
            catch (ArgumentException)
            {
                ThrowHelper.ThrowAsNotCompareble();
            }
        }

        /// <summary>
        /// イントロソートを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">要素の比較演算子</param>
        /// <param name="recursionLimit">再帰の上限</param>
        private static void IntroSortCore<T>(LongArray<T> array, long start, long count, IComparer<T> comparer, long recursionLimit)
        {
            if (count <= Intro2InsertThreshold)
            {
                if (count <= 1) return;
                if (count == 2)
                {
                    ref T ref1 = ref array[start];
                    ref T ref2 = ref Unsafe.Add(ref ref1, 1);

                    if (comparer.Compare(ref1, ref2) > 0) (ref1, ref2) = (ref2, ref1);
                    return;
                }

                InsertionSort(array, start, count, comparer);
                return;
            }

            if (recursionLimit == 0)
            {
                HeapSort(array, start, count, comparer);
                return;
            }

            long headIndex = start;
            long tailIndex = start + count - 1L;
            T pivot = SelectPivot(array[headIndex], array[(headIndex + tailIndex) / 2L], array[tailIndex], comparer);

            headIndex--;
            tailIndex++;

            while (headIndex < tailIndex)
            {
                while (comparer.Compare(array[++headIndex], pivot) < 0) ;
                while (comparer.Compare(array[--tailIndex], pivot) > 0) ;
                if (headIndex >= tailIndex) break;

                ref T headReference = ref array[headIndex];
                ref T tailReference = ref array[tailIndex];
                (headReference, tailReference) = (tailReference, headReference);
            }

            IntroSortCore(array, start, headIndex - start, comparer, --recursionLimit);
            IntroSortCore(array, headIndex, start + count - headIndex, comparer, recursionLimit);
        }

        /// <summary>
        /// ヒープソートを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">要素の比較演算子</param>
        public static void HeapSort<T>(LongArray<T> array, long start, long count, IComparer<T> comparer)
        {
            /*
             * Heap
             *
             *     0
             *   1   2
             *  3 4 5 6
             *
             *      parent: (i - 1) / 2
             *  left child: i * 2 + 1
             * right child: i * 2 + 2
             */
            if (count <= 1) return;

            long end = start + count;

            // make heap
            for (long i = start + 1; i < end; i++) UpHeap(array, start, i, comparer);

            // get from heap
            ref T head = ref array[start];
            ref T tail = ref array[end - 1];
            while (count > 0)
            {
                (head, tail) = (tail, head);
                DownHeap(array, start, --count, comparer);
                tail = ref Unsafe.Subtract(ref tail, 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void UpHeap(LongArray<T> array, long heapStart, long targetIndex, IComparer<T> comparer)
            {
                long currentIndex = targetIndex;
                while (currentIndex > heapStart)
                {
                    long parentIndex = (currentIndex - heapStart - 1) / 2 + heapStart;
                    ref T current = ref array[currentIndex];
                    ref T parent = ref array[parentIndex];

                    if (comparer.Compare(current, parent) > 0) (current, parent) = (parent, current);

                    currentIndex = parentIndex;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void DownHeap(LongArray<T> array, long start, long count, IComparer<T> comparer)
            {
                long currentIndex = start;
                long end = start + count;

                while (true)
                {
                    ref T current = ref array[currentIndex];
                    long leftChildIndex = (currentIndex - start) * 2 + 1 + start;

                    if (leftChildIndex >= end) return;

                    ref T leftChild = ref array[leftChildIndex];

                    if (leftChildIndex + 1 == end)
                    {
                        if (comparer.Compare(current, leftChild) < 0) (current, leftChild) = (leftChild, current);
                        return;
                    }
                    else
                    {
                        ref T rightChild = ref Unsafe.Add(ref leftChild, 1);

                        if (comparer.Compare(leftChild, rightChild) > 0)
                        {
                            if (comparer.Compare(current, leftChild) > 0) return;
                            (current, leftChild) = (leftChild, current);
                            currentIndex = leftChildIndex;
                        }
                        else
                        {
                            if (comparer.Compare(current, rightChild) > 0) return;
                            (current, rightChild) = (rightChild, current);
                            currentIndex = leftChildIndex + 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 挿入ソートを行います。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">処理対象の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">要素の比較演算子</param>
        public static void InsertionSort<T>(LongArray<T> array, long start, long count, IComparer<T> comparer)
        {
            if (count <= 1) return;

            long end = start + count;
            for (long i = start + 1; i < end; i++)
            {
                long currentIndex = i;
                ref T ref1 = ref array[i - 1];
                ref T ref2 = ref Unsafe.Add(ref ref1, 1);
                while ((--currentIndex) >= start && comparer.Compare(ref1, ref2) > 0)
                {
                    (ref1, ref2) = (ref2, ref1);
                    ref1 = ref Unsafe.Subtract(ref ref1, 1);
                    ref2 = ref Unsafe.Subtract(ref ref2, 1);
                }
            }
        }

        /// <summary>
        /// イントロソートを行います。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="keys">ソートの基準となるキーの配列</param>
        /// <param name="values">ソートされる値の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">キーの比較演算子</param>
        /// <exception cref="ArgumentException"><paramref name="comparer"/>の比較が無効</exception>
        public static void IntroSort<TKey, TValue>(LongArray<TKey> keys, LongArray<TValue> values, long start, long count, IComparer<TKey> comparer)
        {
            if (count <= 1) return;
            try
            {
                IntroSortCore(keys, values, start, count, comparer, 2 * (long.Log2(count) + 1));
            }
            catch (IndexOutOfRangeException)
            {
                ThrowHelper.ThrowAsInvalidComparer(nameof(comparer));
            }
            catch (ArgumentException)
            {
                ThrowHelper.ThrowAsNotCompareble();
            }
        }

        /// <summary>
        /// イントロソートを行います。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="keys">ソートの基準となるキーの配列</param>
        /// <param name="values">ソートされる値の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">キーの比較演算子</param>
        /// <param name="recursionLimit">再帰の上限</param>
        private static void IntroSortCore<TKey, TValue>(LongArray<TKey> keys, LongArray<TValue> values, long start, long count, IComparer<TKey> comparer, long recursionLimit)
        {
            if (count <= Intro2InsertThreshold)
            {
                if (count <= 1) return;
                if (count == 2)
                {
                    ref TKey keyRef1 = ref keys[start];
                    ref TKey keyRef2 = ref Unsafe.Add(ref keyRef1, 1);
                    ref TValue valueRef1 = ref values[start];
                    ref TValue valueRef2 = ref Unsafe.Add(ref valueRef1, 1);

                    if (comparer.Compare(keyRef1, keyRef2) > 0)
                    {
                        (keyRef1, keyRef2) = (keyRef2, keyRef1);
                        (valueRef1, valueRef2) = (valueRef2, valueRef1);
                    }

                    return;
                }

                InsertionSort(keys, values, start, count, comparer);
                return;
            }

            if (recursionLimit == 0)
            {
                HeapSort(keys, values, start, count, comparer);
                return;
            }

            long headIndex = start;
            long tailIndex = start + count - 1L;
            TKey pivot = SelectPivot(keys[headIndex], keys[(headIndex + tailIndex) / 2L], keys[tailIndex], comparer);

            headIndex--;
            tailIndex++;

            while (headIndex < tailIndex)
            {
                while (comparer.Compare(keys[++headIndex], pivot) < 0) ;
                while (comparer.Compare(keys[--tailIndex], pivot) > 0) ;
                if (headIndex >= tailIndex) break;

                ref TKey headKeyReference = ref keys[headIndex];
                ref TKey tailKeyReference = ref keys[tailIndex];
                (headKeyReference, tailKeyReference) = (tailKeyReference, headKeyReference);

                ref TValue headValueReference = ref values[headIndex];
                ref TValue tailValueReference = ref values[tailIndex];
                (headValueReference, tailValueReference) = (tailValueReference, headValueReference);
            }

            IntroSortCore(keys, values, start, headIndex - start, comparer, --recursionLimit);
            IntroSortCore(keys, values, headIndex, start + count - headIndex, comparer, recursionLimit);
        }

        /// <summary>
        /// ヒープソートを行います。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="keys">ソートの基準となるキーの配列</param>
        /// <param name="values">ソートされる値の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">キーの比較演算子</param>
        public static void HeapSort<TKey, TValue>(LongArray<TKey> keys, LongArray<TValue> values, long start, long count, IComparer<TKey> comparer)
        {
            /*
             * Heap
             *
             *     0
             *   1   2
             *  3 4 5 6
             *
             *      parent: (i - 1) / 2
             *  left child: i * 2 + 1
             * right child: i * 2 + 2
             */
            if (count <= 1) return;

            long end = start + count;

            // make heap
            for (long i = start + 1; i < end; i++) UpHeap(keys, values, start, i, comparer);

            // get from heap
            ref TKey headKey = ref keys[start];
            ref TKey tailKey = ref keys[end - 1];
            ref TValue headValue = ref values[start];
            ref TValue tailValue = ref values[end - 1];
            while (count > 0)
            {
                (headKey, tailKey) = (tailKey, headKey);
                DownHeap(keys, values, start, --count, comparer);
                tailKey = ref Unsafe.Subtract(ref tailKey, 1);
                tailValue = ref Unsafe.Subtract(ref tailValue, 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void UpHeap(LongArray<TKey> keys, LongArray<TValue> values, long heapStart, long targetIndex, IComparer<TKey> comparer)
            {
                long currentIndex = targetIndex;
                while (currentIndex > heapStart)
                {
                    long parentIndex = (currentIndex - heapStart - 1) / 2 + heapStart;
                    ref TKey currentKey = ref keys[currentIndex];
                    ref TKey parentKey = ref keys[parentIndex];
                    ref TValue currentValue = ref values[currentIndex];
                    ref TValue parentValue = ref values[parentIndex];

                    if (comparer.Compare(currentKey, parentKey) > 0)
                    {
                        (currentKey, parentKey) = (parentKey, currentKey);
                        (currentValue, parentValue) = (parentValue, currentValue);
                    }

                    currentIndex = parentIndex;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void DownHeap(LongArray<TKey> keys, LongArray<TValue> values, long start, long count, IComparer<TKey> comparer)
            {
                long currentIndex = start;
                long end = start + count;

                while (true)
                {
                    ref TKey currentKey = ref keys[currentIndex];
                    ref TValue currentValue = ref values[currentIndex];
                    long leftChildIndex = (currentIndex - start) * 2 + 1 + start;

                    if (leftChildIndex >= end) return;

                    ref TKey leftChildKey = ref keys[leftChildIndex];
                    ref TValue leftChildValue = ref values[leftChildIndex];

                    if (leftChildIndex + 1 == end)
                    {
                        if (comparer.Compare(currentKey, leftChildKey) < 0)
                        {
                            (currentKey, leftChildKey) = (leftChildKey, currentKey);
                            (currentValue, leftChildValue) = (leftChildValue, currentValue);
                        }

                        return;
                    }
                    else
                    {
                        ref TKey rightChildKey = ref Unsafe.Add(ref leftChildKey, 1);
                        ref TValue rightChildValue = ref Unsafe.Add(ref leftChildValue, 1);

                        if (comparer.Compare(leftChildKey, rightChildKey) > 0)
                        {
                            if (comparer.Compare(currentKey, leftChildKey) > 0) return;
                            (currentKey, leftChildKey) = (leftChildKey, currentKey);
                            (currentValue, leftChildValue) = (leftChildValue, currentValue);
                            currentIndex = leftChildIndex;
                        }
                        else
                        {
                            if (comparer.Compare(currentKey, rightChildKey) > 0) return;
                            (currentKey, rightChildKey) = (rightChildKey, currentKey);
                            (currentValue, rightChildValue) = (rightChildValue, currentValue);
                            currentIndex = leftChildIndex + 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 挿入ソートを行います。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="keys">ソートの基準となるキーの配列</param>
        /// <param name="values">ソートされる値の配列</param>
        /// <param name="start">処理範囲の開始インデックス</param>
        /// <param name="count">処理範囲の要素数</param>
        /// <param name="comparer">キーの比較演算子</param>
        public static void InsertionSort<TKey, TValue>(LongArray<TKey> keys, LongArray<TValue> values, long start, long count, IComparer<TKey> comparer)
        {
            if (count <= 1) return;

            long end = start + count;
            for (long i = start + 1; i < end; i++)
            {
                long currentIndex = i;
                ref TKey keyRef1 = ref keys[i - 1];
                ref TKey keyRef2 = ref Unsafe.Add(ref keyRef1, 1);
                ref TValue valueRef1 = ref values[i - 1];
                ref TValue valueRef2 = ref Unsafe.Add(ref valueRef1, 1);

                while ((--currentIndex) >= start && comparer.Compare(keyRef1, keyRef2) > 0)
                {
                    (keyRef1, keyRef2) = (keyRef2, keyRef1);
                    (valueRef1, valueRef2) = (valueRef2, valueRef1);
                    keyRef1 = ref Unsafe.Subtract(ref keyRef1, 1);
                    keyRef2 = ref Unsafe.Subtract(ref keyRef2, 1);
                    valueRef1 = ref Unsafe.Subtract(ref valueRef1, 1);
                    valueRef2 = ref Unsafe.Subtract(ref valueRef2, 1);
                }
            }
        }

        /// <summary>
        /// 3つの値からクイックソート用のピボットを選択します。
        /// </summary>
        /// <typeparam name="T">値の型</typeparam>
        /// <param name="val1">候補値</param>
        /// <param name="val2">候補値</param>
        /// <param name="val3">候補値</param>
        /// <param name="comparer">使用する比較演算子</param>
        /// <returns>三つの値の中央値</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T SelectPivot<T>(T val1, T val2, T val3, IComparer<T> comparer)
        {
            if (comparer.Compare(val1, val2) > 0) (val1, val2) = (val2, val1);
            if (comparer.Compare(val1, val3) > 0) (val1, val3) = (val3, val1);
            if (comparer.Compare(val2, val3) > 0) (val2, val3) = (val3, val2);
            return val2;
        }
    }
}
