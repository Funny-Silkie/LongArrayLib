using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace LongArrayLib.Internal
{
    /// <summary>
    /// <see cref="LongArray{T}"/>用の<see cref="MemoryManager{T}"/>の実装です。
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    internal sealed class LongArrayMemoryManager<T> : MemoryManager<T>
    {
        private readonly LongArray<T> array;
        private readonly long start;
        private readonly int length;

        /// <summary>
        /// <see cref="LongArrayMemoryManager{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="array">使用する配列</param>
        public LongArrayMemoryManager(LongArray<T> array)
            : this(array, 0, checked((int)array.Length))
        {
        }

        /// <summary>
        /// <see cref="LongArrayMemoryManager{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="array">使用する配列</param>
        /// <param name="start">開始インデックス</param>
        /// <param name="length">メモリのサイズ</param>
        public LongArrayMemoryManager(LongArray<T> array, long start, int length)
        {
            this.array = array;
            this.start = start;
            this.length = length;
        }

        /// <inheritdoc/>
        public override Span<T> GetSpan() => length == 0 ? Span<T>.Empty : array.AsSpan(start, length);

        /// <inheritdoc/>
        public override unsafe MemoryHandle Pin(int elementIndex = 0)
        {
            GCHandle gcHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            void* arrayPtr = UnsafeHelper.Increment<T>(array.AsPointer(), start);
            return new MemoryHandle(arrayPtr, gcHandle, this);
        }

        /// <inheritdoc/>
        public override void Unpin()
        {
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
        }
    }
}
