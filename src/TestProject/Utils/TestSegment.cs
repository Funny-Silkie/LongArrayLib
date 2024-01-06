using System;
using System.Buffers;

namespace TestProject.Utils
{
    /// <summary>
    /// テスト用の<see cref="ReadOnlySequenceSegment{T}"/>の実装です。
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    internal sealed class TestSegment<T> : ReadOnlySequenceSegment<T>
    {
        /// <summary>
        /// <see cref="TestSegment{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="memory">使用する<see cref="Memory{T}"/>のインスタンス</param>
        /// <param name="index">インデックス</param>
        public TestSegment(Memory<T> memory, int index)
        {
            Memory = memory;
            RunningIndex = index;
        }

        /// <summary>
        /// ペアのインスタンスを生成します。
        /// </summary>
        /// <param name="value1">最初のインスタンスの持つ値</param>
        /// <param name="value2">次のインスタンスの持つ値</param>
        /// <returns>ペアのインスタンス</returns>
        public static (TestSegment<T> start, TestSegment<T> end) CreateCouple(T value1, T value2)
        {
            var start = new TestSegment<T>(new Memory<T>([value1]), 0);
            var end = new TestSegment<T>(new Memory<T>([value2]), 1);
            start.Next = end;

            return (start, end);
        }
    }
}
