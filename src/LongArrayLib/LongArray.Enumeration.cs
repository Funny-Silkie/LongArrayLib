using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LongArrayLib
{
    public partial class LongArray<T>
    {
        /// <inheritdoc/>
        public Enumerator GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// 各要素に対して処理を実行します。
        /// </summary>
        /// <param name="action">実行する処理</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/>が<see langword="null"/></exception>
        public void ForEach(Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            ref T reference = ref GetReference();
            for (long i = 0; i < Length; i++)
            {
                action.Invoke(reference);
                reference = ref Unsafe.Add(ref reference, 1);
            }
        }

        #region Explicit Interface Implementation

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion IEnumerable

        #region IEnumerable`1

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        #endregion IEnumerable`1

        #endregion Explicit Interface Implementation

        /// <summary>
        /// <see cref="LongArray{T}"/>の列挙をサポートする構造体です。
        /// </summary>
        [Serializable]
        public struct Enumerator : IEnumerator<T>
        {
            private readonly LongArray<T> source;
            private long index;

            /// <inheritdoc/>
            public T Current { get; private set; }

            readonly object IEnumerator.Current
            {
                get
                {
                    if (index == 0 || index == source.Length + 1) ThrowHelper.ThrowAsNoEnumeration();
                    return Current!;
                }
            }

            /// <summary>
            /// <see cref="Enumerator"/>の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="source">読み取る配列</param>
            public Enumerator(LongArray<T> source)
            {
                this.source = source;
                Current = default!;
            }

#pragma warning disable IDE0251 // メンバーを 'readonly' にする

            /// <inheritdoc/>
            public void Dispose()
#pragma warning restore IDE0251 // メンバーを 'readonly' にする
            {
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                if (index < source.Length)
                {
                    Current = source[index++];
                    return true;
                }

                Current = default!;
                index = source.Length + 1;
                return false;
            }

            void IEnumerator.Reset()
            {
                Current = default!;
                index = 0;
            }
        }
    }
}
