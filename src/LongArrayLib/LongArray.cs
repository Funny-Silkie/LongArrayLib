using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LongArrayLib
{
    /// <summary>
    /// 長大配列を表します。
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    [DebuggerTypeProxy(typeof(LongArrayDebuggerView<>))]
    [DebuggerDisplay("Length = {Length}")]
    public sealed unsafe partial class LongArray<T> : IList<T>, IReadOnlyList<T>, IList, IDisposable, ICloneable
    {
        private readonly void* items;
        private bool isDisposed;

        /// <summary>
        /// 空配列を取得します。
        /// </summary>
        public static LongArray<T> Empty { get; } = new LongArray<T>(0);

        /// <summary>
        /// 配列長を取得します。
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// <typeparamref name="T"/>へキャストします。
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <returns>キャスト後の値</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/>が無効な型</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T AsT(object? value)
        {
            if (value is null && default(T) is null) return default!;
            if (value is T t) return t;

            ThrowHelper.ThrowAsInvalidType(nameof(value));
            return default;
        }

        #region IDisposable

        /// <summary>
        /// <see cref="LongArray{T}"/>のインスタンスのクリーンアップを行います。
        /// </summary>
        ~LongArray()
        {
            Dispose(false);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

#pragma warning disable IDE0060 // 未使用のパラメーターを削除します

        /// <summary>
        /// インスタンスを破棄します。
        /// </summary>
        /// <param name="disposing">マネージドオブジェクトも破棄するかどうか</param>
        private void Dispose(bool disposing)
#pragma warning restore IDE0060 // 未使用のパラメーターを削除します
        {
            if (!isDisposed)
            {
                //if (disposing)
                //{
                //}

                NativeMemory.Free(items);
                isDisposed = true;
            }
        }

        #endregion IDisposable

        #region Explicit Interface Implementation

        #region ICollection

        int ICollection.Count => checked((int)Length);

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        #endregion ICollection

        #region IReadOnlyCollection`1

        int IReadOnlyCollection<T>.Count => checked((int)Length);

        #endregion IReadOnlyCollection`1

        #region ICollection`1

        int ICollection<T>.Count => checked((int)Length);

        bool ICollection<T>.IsReadOnly => true;

        #endregion ICollection`1

        #region IList

        bool IList.IsFixedSize => true;

        bool IList.IsReadOnly => true;

        #endregion IList

        #endregion Explicit Interface Implementation
    }

    /// <summary>
    /// <see cref="LongArray{T}"/>の処理を記述します。
    /// </summary>
    public static partial class LongArray
    {
    }

    /// <summary>
    /// <see cref="LongArray{T}"/>用のデバッガービューのクラスです。
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    internal class LongArrayDebuggerView<T>
    {
        private readonly LongArray<T> array;

        /// <summary>
        /// 表示用の配列を取得します。
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => array.ToArray();

        /// <summary>
        /// <see cref="LongArrayDebuggerView{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="array">対象の配列</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>が<see langword="null"/></exception>
        public LongArrayDebuggerView(LongArray<T> array)
        {
            ArgumentNullException.ThrowIfNull(array);

            this.array = array;
        }
    }
}
