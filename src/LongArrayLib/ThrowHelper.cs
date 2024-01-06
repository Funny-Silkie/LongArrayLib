using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace LongArrayLib
{
    /// <summary>
    /// 例外スローのヘルパークラスです。
    /// </summary>
    internal static class ThrowHelper
    {
        /// <summary>
        /// 無効な型が渡された旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentException"></exception>
        [DoesNotReturn]
        public static void ThrowAsInvalidType(string argName)
        {
            throw new ArgumentException("無効な型が渡されました", argName);
        }

        /// <summary>
        /// 無効な配列の型が渡された旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentException"></exception>
        [DoesNotReturn]
        public static void ThrowAsInvalidArray(string argName)
        {
            throw new ArgumentException("無効な配列が渡されました", argName);
        }

        /// <summary>
        /// 配列のサイズが不足している旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentException"></exception>
        [DoesNotReturn]
        public static void ThrowAsShortArray(string argName)
        {
            throw new ArgumentException("配列のサイズが足りません", argName);
        }

        /// <summary>
        /// メモリ領域のサイズが不足している旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentException"></exception>
        [DoesNotReturn]
        public static void ThrowAsShortMemory(string argName)
        {
            throw new ArgumentException("メモリ領域のサイズが足りません", argName);
        }

        /// <summary>
        /// 配列長を超える要素数である旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DoesNotReturn]
        public static void ThrowAsLargerLength(string argName)
        {
            throw new ArgumentOutOfRangeException(argName, "配列長を超えます");
        }

        /// <summary>
        /// コレクションのサイズを超える範囲である旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentException"></exception>
        [DoesNotReturn]
        public static void ThrowAsInvalidRangeOfCollection(string argName)
        {
            throw new ArgumentException("コレクションのサイズを超えます", argName);
        }

        /// <summary>
        /// 配列の最大サイズを超える旨の例外をスローします。
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [DoesNotReturn]
        public static void ThrowAsOutOfArraySize()
        {
            throw new InvalidOperationException("配列の最大サイズを超えます");
        }

        /// <summary>
        /// 配列の最大サイズを超える旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DoesNotReturn]
        public static void ThrowAsOutOfArraySize(string? argName)
        {
            throw new ArgumentOutOfRangeException(nameof(argName), "配列の最大サイズを超えます");
        }

        /// <summary>
        /// 列挙中のコレクションが変更された旨の例外をスローします。
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [DoesNotReturn]
        public static void ThrowAsCollectionChanged()
        {
            throw new InvalidOperationException("列挙中にコレクションが変更されました");
        }

        /// <summary>
        /// 列挙が開始されていない旨の例外をスローします。
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [DoesNotReturn]
        public static void ThrowAsNoEnumeration()
        {
            throw new InvalidOperationException("列挙中ではありません");
        }

        /// <summary>
        /// 固定長のコレクションである旨の例外をスローします。
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        [DoesNotReturn]
        public static void ThrowAsFixedSize()
        {
            throw new NotSupportedException("固定長のコレクションです");
        }

        /// <summary>
        /// インデックスが無効である旨の例外をスローします。
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [DoesNotReturn]
        public static void ThrowAsInvalidIndex()
        {
            throw new IndexOutOfRangeException("インデックスが範囲外です");
        }

        /// <summary>
        /// インデックスが無効である旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DoesNotReturn]
        public static void ThrowAsInvalidIndex(string? argName)
        {
            throw new ArgumentOutOfRangeException(argName, "インデックスが範囲外です");
        }

        /// <summary>
        /// 要素が比較不能である旨の例外をスローします。
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [DoesNotReturn]
        public static void ThrowAsNotCompareble()
        {
            throw new InvalidOperationException($"要素を比較できませんでした。要素が{typeof(IComparable<>)}を実装しません");
        }

        /// <summary>
        /// 比較演算子の比較が無効である旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentException"></exception>
        [DoesNotReturn]
        public static void ThrowAsInvalidComparer(string? argName)
        {
            throw new ArgumentException("比較演算子の比較が無効です", argName);
        }

        /// <summary>
        /// 範囲が無効である旨の例外をスローします。
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DoesNotReturn]
        public static void ThrowAsInvalidRange(string? argName)
        {
            throw new ArgumentOutOfRangeException(argName, "範囲が無効です");
        }

        /// <summary>
        /// オブジェクトが破棄されている旨の例外をスローします。
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        [DoesNotReturn]
        public static void ThrowAsDisposed()
        {
            throw new ObjectDisposedException(null);
        }

        /// <summary>
        /// ストリームの終端である旨の例外をスローします。
        /// </summary>
        /// <exception cref="EndOfStreamException"></exception>
        [DoesNotReturn]
        public static void ThrowAsEndOfStream()
        {
            throw new EndOfStreamException();
        }

        /// <summary>
        /// 値が0未満の際に例外をスローします。
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegative(int value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(argName, "値が0未満です");
        }

        /// <summary>
        /// 値が0未満の際に例外をスローします。
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegative(long value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(argName, "値が0未満です");
        }

        /// <summary>
        /// 値が0以下の際に例外をスローします。
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegativeOrZero(int value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(argName, "値が0未満です");
        }

        /// <summary>
        /// 値が0以下の際に例外をスローします。
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <param name="argName">引数名</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegativeOrZero(long value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(argName, "値が0未満です");
        }
    }
}
