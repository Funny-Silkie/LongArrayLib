using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LongArrayLib.Internal
{
    /// <summary>
    /// unsafeな処理のヘルパークラスです。
    /// </summary>
    internal static class UnsafeHelper
    {
        /// <summary>
        /// 指定したインデックス分ポインタを進めます。
        /// </summary>
        /// <typeparam name="T">アドレス計算に用いる要素の型</typeparam>
        /// <param name="ptr">対象のポインタ</param>
        /// <param name="index">進める要素数</param>
        /// <returns>進んだポインタ</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* Increment<T>(void* ptr, long index)
        {
            return (byte*)ptr + checked(index * Unsafe.SizeOf<T>());
        }

        /// <summary>
        /// <see cref="Stack{T}"/>の内部配列を<see cref="Span{T}"/>として取得します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="stack">配列を抜き出す<see cref="Stack{T}"/>のインスタンス</param>
        /// <param name="span"><paramref name="stack"/>の内部配列を表す<see cref="Span{T}"/>のインスタンス</param>
        /// <remarks>メモリレイアウト依存なので高速だがバージョン変更に対して脆弱</remarks>
        public static void GetInnerArray<T>(Stack<T> stack, out Span<T> span)
        {
            DummyStack<T> dummy = Unsafe.As<DummyStack<T>>(stack);
            span = dummy.array.AsSpan(0, dummy.size);
        }

        /// <summary>
        /// <see cref="Queue{T}"/>の内部配列を<see cref="Span{T}"/>として取得します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="queue">配列を抜き出す<see cref="Queue{T}"/>のインスタンス</param>
        /// <param name="headSpan"><paramref name="queue"/>の内部配列を表す<see cref="Span{T}"/>のインスタンスのうち，前半の領域を表すもの</param>
        /// <param name="tailSpan"><paramref name="queue"/>の内部配列を表す<see cref="Span{T}"/>のインスタンスのうち，後半の領域を表すもの</param>
        /// <remarks>メモリレイアウト依存なので高速だがバージョン変更に対して脆弱</remarks>
        public static void GetInnerArray<T>(Queue<T> queue, out Span<T> headSpan, out Span<T> tailSpan)
        {
            DummyQueue<T> dummy = Unsafe.As<DummyQueue<T>>(queue);
            if (dummy.tail <= dummy.head)
            {
                headSpan = dummy.array.AsSpan(dummy.head, dummy.array.Length - dummy.head);
                tailSpan = dummy.array.AsSpan(0, dummy.tail);
            }
            else
            {
                headSpan = dummy.array.AsSpan(dummy.head, dummy.size);
                tailSpan = Span<T>.Empty;
            }
        }

#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

        /// <summary>
        /// <see cref="Stack{T}"/>のダミークラスです。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        private sealed class DummyStack<T>
        {
            /// <summary>
            /// 内部配列
            /// </summary>
            internal readonly T[] array;

            /// <summary>
            /// 要素数
            /// </summary>
            internal readonly int size;

            /// <summary>
            /// バージョン
            /// </summary>
            internal readonly int version;
        }

        /// <summary>
        /// <see cref="Queue{T}"/>のダミークラスです。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        private sealed class DummyQueue<T>
        {
            /// <summary>
            /// 内部配列
            /// </summary>
            internal readonly T[] array;

            /// <summary>
            /// <see cref="array"/>における最初の要素のインデックス
            /// </summary>
            internal readonly int head;

            /// <summary>
            /// <see cref="array"/>における次の最後の要素のインデックス
            /// </summary>
            internal readonly int tail;

            /// <summary>
            /// 要素数
            /// </summary>
            internal readonly int size;

            /// <summary>
            /// バージョン
            /// </summary>
            internal readonly int version;
        }

#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    }
}
