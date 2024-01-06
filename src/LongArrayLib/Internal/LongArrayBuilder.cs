using System;

namespace LongArrayLib.Internal
{
    /// <summary>
    /// 長大サイズの配列を生成するクラスです。
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    internal sealed class LongArrayBuilder<T> : IDisposable
    {
        private long count;
        private bool returnedItems;
        private LongArray<T> items;
        private bool isDisposed;

        /// <summary>
        /// <see cref="LongArrayBuilder{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="initialSize">初期容量</param>
        public LongArrayBuilder(long initialSize)
        {
            items = new LongArray<T>(initialSize);
        }

        /// <summary>
        /// 末尾に要素を追加します。
        /// </summary>
        /// <param name="item">追加する要素</param>
        /// <exception cref="InvalidOperationException">既にインスタンスが破棄されている</exception>
        public void Add(T item)
        {
            if (isDisposed) ThrowHelper.ThrowAsDisposed();

            if (count == items.Length)
            {
                LongArray<T> old = items;
                LongArray.Resize(ref items, Math.Min(items.Length * 2, long.MaxValue));
                if (!returnedItems) old.Dispose();
                else returnedItems = false;
            }

            items[count++] = item;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (isDisposed) return;

            count = 0;
            if (!returnedItems) items.Dispose();
            isDisposed = true;
        }

        /// <summary>
        /// <see cref="LongArray{T}"/>を生成します。
        /// </summary>
        /// <returns>生成された配列</returns>
        /// <exception cref="InvalidOperationException">既にインスタンスが破棄されている</exception>
        public LongArray<T> ToArray()
        {
            if (isDisposed) ThrowHelper.ThrowAsDisposed();

            if (count == 0) return LongArray<T>.Empty;
            if (count == items.Length)
            {
                returnedItems = true;
                return items;
            }

            return items.GetRangeCore(0L, count);
        }
    }
}
