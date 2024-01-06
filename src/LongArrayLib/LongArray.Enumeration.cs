using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LongArrayLib
{
    public partial class LongArray<T>
    {
        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public Enumerator GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// <see cref="ReadOnlySpan{T}"/>による列挙をサポートするオブジェクトを取得します。
        /// </summary>
        /// <param name="chunkSize">列挙する<see cref="ReadOnlySpan{T}"/>のサイズ</param>
        /// <returns><see cref="ReadOnlySpan{T}"/>による列挙をサポートするオブジェクト</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="chunkSize"/>が0以下</exception>
        public SpanEnumerator GetSpanEnumerator(int chunkSize)
        {
            ThrowHelper.ThrowIfNegativeOrZero(chunkSize);

            return new SpanEnumerator(this, chunkSize);
        }

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

        /// <summary>
        /// 各要素のまとまりに対して処理を実行します。
        /// </summary>
        /// <typeparam name="TArg">引数の型</typeparam>
        /// <param name="action">要素のまとまりに対する処理</param>
        /// <param name="argument"><paramref name="action"/>に適用する引数</param>
        /// <param name="chunkSize">要素をまとめる個数，最後の要素はこの値より小さい場合がある</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="chunkSize"/>が0以下</exception>
        public void ForEachChunk<TArg>(ReadOnlySpanAction<T, TArg> action, TArg argument, int chunkSize)
        {
            ArgumentNullException.ThrowIfNull(action);
            ThrowHelper.ThrowIfNegativeOrZero(chunkSize);

            ref T reference = ref GetReference();
            for (long offset = 0L; offset < Length; offset += chunkSize)
            {
                int size = (int)Math.Min(Length - offset, chunkSize);
                Span<T> span = MemoryMarshal.CreateSpan(ref reference, size);
                action.Invoke(span, argument);
                reference = ref Unsafe.Add(ref reference, size);
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

        /// <summary>
        /// <see cref="ReadOnlySpan{T}"/>による列挙をサポートする構造体です。
        /// </summary>
        [Serializable]
        public struct SpanEnumerator
        {
            private readonly LongArray<T> source;
            private readonly int chunkSize;
            private long offSet;
            private int spanLength;

            /// <inheritdoc cref="IEnumerator.Current"/>
            public readonly ReadOnlySpan<T> Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => source.AsSpan(offSet, spanLength);
            }

            /// <summary>
            /// <see cref="SpanEnumerator"/>の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="source">列挙対象の配列</param>
            /// <param name="chunkSize">チャンクのサイズ</param>
            internal SpanEnumerator(LongArray<T> source, int chunkSize)
            {
                this.source = source;
                this.chunkSize = chunkSize;
                offSet = -chunkSize;
                spanLength = chunkSize;
            }

            /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
            public readonly SpanEnumerator GetEnumerator() => this;

            /// <summary>
            /// <inheritdoc cref="IEnumerator.MoveNext"/>
            /// </summary>
            /// <returns><inheritdoc cref="IEnumerator.MoveNext"/></returns>
            public bool MoveNext()
            {
                offSet += chunkSize;
                if (offSet >= source.Length) return false;
                long restLength = source.Length - offSet;
                if (restLength < chunkSize) spanLength = (int)restLength;

                return true;
            }

            /// <summary>
            /// <inheritdoc cref="IEnumerator.Reset"/>
            /// </summary>
            public void Reset()
            {
                offSet = -chunkSize;
                spanLength = chunkSize;
            }
        }
    }
}
