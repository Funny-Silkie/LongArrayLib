using System;
using System.Buffers;

namespace LongArrayLib.Internal
{
    /// <summary>
    /// <see cref="LongArray{T}"/>用の<see cref="ReadOnlySequenceSegment{T}"/>の実装です。
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    internal sealed class LongArraySegment<T> : ReadOnlySequenceSegment<T>
    {
        /// <summary>
        /// <see cref="LongArraySegment{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        private LongArraySegment()
        {
        }

        /// <summary>
        /// <see cref="LongArray{T}"/>に対応する<see cref="LongArraySegment{T}"/>の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="array">使用する配列のインスタンス</param>
        /// <param name="maxChunkSize">チャンクの最大サイズ</param>
        /// <returns><paramref name="array"/>の要素を参照する<see cref="LongArraySegment{T}"/>の新しいインスタンス</returns>
        public static (LongArraySegment<T> start, LongArraySegment<T> end) Create(LongArray<T> array, int maxChunkSize)
        {
            int currentLength = (int)Math.Min(array.Length, maxChunkSize);

            var start = new LongArraySegment<T>()
            {
                Memory = array.AsMemory(0L, currentLength),
                RunningIndex = 0,
            };
            if (array.Length == currentLength) return (start, start);

            LongArraySegment<T> end = start;
            long offSet = currentLength;
            int index = 0;
            while (offSet < array.Length)
            {
                currentLength = (int)Math.Min(array.Length - offSet, maxChunkSize);
                var next = new LongArraySegment<T>()
                {
                    Memory = array.AsMemory(offSet, currentLength),
                    RunningIndex = ++index,
                };
                offSet += currentLength;
                end.Next = next;
                end = next;
            }
            return (start, end);
        }
    }
}
