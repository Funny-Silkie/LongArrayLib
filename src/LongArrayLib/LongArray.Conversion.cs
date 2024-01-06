using System;
using System.Runtime.CompilerServices;

namespace LongArrayLib
{
    public partial class LongArray<T>
    {
        /// <summary>
        /// 全ての要素を変換します。
        /// </summary>
        /// <typeparam name="TOut">変換後の型</typeparam>
        /// <param name="converter">変換関数</param>
        /// <returns>変換後の要素の格納された配列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="converter"/>が<see langword="null"/></exception>
        public LongArray<TOut> ConverAll<TOut>(Converter<T, TOut> converter)
        {
            ArgumentNullException.ThrowIfNull(converter);

            if (Length == 0) return LongArray<TOut>.Empty;

            var result = new LongArray<TOut>(Length, false);
            ref T reference = ref GetReference();
            for (long i = 0; i < Length; i++)
            {
                result[i] = converter.Invoke(reference);
                reference = ref Unsafe.Add(ref reference, 1);
            }

            return result;
        }

        /// <summary>
        /// 配列に変換します。
        /// </summary>
        /// <returns>変換後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">配列長が<see cref="Array.MaxLength"/>を超える</exception>
        public T[] ToArray()
        {
            if (Length == 0) return Array.Empty<T>();
            if (Length > Array.MaxLength) ThrowHelper.ThrowAsOutOfArraySize();

            var result = new T[Length];
            CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// ポインターに変換します。
        /// </summary>
        /// <returns>変換後のポインター</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void* AsPointer() => items;

        /// <summary>
        /// 配列へ明示的に変換します。
        /// </summary>
        /// <param name="array">変換する値</param>
        /// <exception cref="InvalidOperationException">配列長が<see cref="Array.MaxLength"/>を超える</exception>
        public static explicit operator T[](LongArray<T> array) => array.ToArray();

        /// <summary>
        /// <see cref="Memory{T}"/>へ明示的に変換します。
        /// </summary>
        /// <param name="array">変換する値</param>
        /// <exception cref="OverflowException">配列長が<see cref="int.MaxValue"/>を超える</exception>
        public static explicit operator Memory<T>(LongArray<T>? array) => array.AsMemory();

        /// <summary>
        /// <see cref="ReadOnlyMemory{T}"/>へ明示的に変換します。
        /// </summary>
        /// <param name="array">変換する値</param>
        /// <exception cref="OverflowException">配列長が<see cref="int.MaxValue"/>を超える</exception>
        public static explicit operator ReadOnlyMemory<T>(LongArray<T>? array) => array.AsMemory();

        /// <summary>
        /// <see cref="Span{T}"/>へ明示的に変換します。
        /// </summary>
        /// <param name="array">変換する値</param>
        /// <exception cref="OverflowException">配列長が<see cref="int.MaxValue"/>を超える</exception>
        public static explicit operator Span<T>(LongArray<T>? array) => array.AsSpan();

        /// <summary>
        /// <see cref="ReadOnlySpan{T}"/>へ明示的に変換します。
        /// </summary>
        /// <param name="array">変換する値</param>
        /// <exception cref="OverflowException">配列長が<see cref="int.MaxValue"/>を超える</exception>
        public static explicit operator ReadOnlySpan<T>(LongArray<T>? array) => array.AsSpan();
    }
}
