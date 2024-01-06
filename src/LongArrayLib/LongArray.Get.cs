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
        /// 指定したインデックスの参照を取得します。
        /// </summary>
        /// <param name="index">要素のインデックス</param>
        /// <returns><paramref name="index"/>に対応した要素の参照</returns>
        /// <exception cref="IndexOutOfRangeException">インデックスが範囲外</exception>
        public unsafe ref T this[long index]
        {
            get
            {
                if ((ulong)index >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex();

                return ref Unsafe.AsRef<T>(UnsafeHelper.Increment<T>(items, index));
            }
        }

        /// <inheritdoc cref="this[long]"/>
        public ref T this[int index] => ref this[(long)index];

        /// <summary>
        /// 指定したインデックスの参照を取得します。
        /// </summary>
        /// <param name="index">要素のインデックス</param>
        /// <returns><paramref name="index"/>に対応した要素の参照</returns>
        /// <exception cref="IndexOutOfRangeException">インデックスが範囲外</exception>
        public ref T this[Index index] => ref this[index.GetLongOffset(Length)];

        /// <summary>
        /// 指定した範囲の配列を取得します。
        /// </summary>
        /// <param name="range">範囲</param>
        /// <returns><paramref name="range"/>に対応した要素の配列</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="range"/>が無効</exception>
        public LongArray<T> this[Range range]
        {
            get
            {
                (long start, long end) = range.GetLongOffsets(Length);

                if (start > end) return Empty;
                return GetRangeCore(start, end - start + 1);
            }
        }

        /// <summary>
        /// 指定した範囲の配列を取得します。
        /// </summary>
        /// <param name="start">範囲の開始インデックス</param>
        /// <param name="count">範囲の要素数</param>
        /// <returns><paramref name="start"/>と<paramref name="count"/>に対応する範囲</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>または<paramref name="count"/>が範囲外</exception>
        public LongArray<T> GetRange(long start, long count)
        {
            if ((ulong)start >= (ulong)Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(start);
            if (start + count > Length) ThrowHelper.ThrowAsLargerLength(nameof(count));

            return GetRangeCore(start, count);
        }

        /// <summary>
        /// 指定した範囲の配列を取得します。
        /// </summary>
        /// <param name="start">範囲の開始インデックス</param>
        /// <param name="count">範囲の要素数</param>
        /// <returns><paramref name="start"/>と<paramref name="count"/>に対応する範囲</returns>
        internal LongArray<T> GetRangeCore(long start, long count)
        {
            if (count == 0) return Empty;

            var result = new LongArray<T>(count, false);
            LongArray.Copy(this, start, result, 0, count);
            return result;
        }

        /// <summary>
        /// 先頭の要素の参照を取得します。
        /// </summary>
        /// <returns>先頭の要素の参照</returns>
        internal unsafe ref T GetReference() => ref Unsafe.AsRef<T>(items);

        #region Explicit Interface Implementation

        #region IList

        object? IList.this[int index]
        {
            get => this[index];
            set => this[index] = AsT(value);
        }

        #endregion IList

        #region IReadOnlyList`1

        T IReadOnlyList<T>.this[int index] => this[index];

        #endregion IReadOnlyList`1

        #region IList`1

        T IList<T>.this[int index]
        {
            get => this[index];
            set => this[index] = value;
        }

        #endregion IList`1

        #endregion Explicit Interface Implementation
    }
}
