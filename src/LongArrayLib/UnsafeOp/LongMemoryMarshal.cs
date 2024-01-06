using System;

namespace LongArrayLib.UnsafeOp
{
    /// <summary>
    /// メモリ関連の処理を記述します。
    /// </summary>
    public static class LongMemoryMarshal
    {
        /// <summary>
        /// 配列の要素の参照を取得します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="array">対象の配列</param>
        /// <returns><paramref name="array"/>の参照</returns>
        /// <exception cref="NullReferenceException"><paramref name="array"/>が<see langword="null"/></exception>
        public static ref T GetLongArrayDataReference<T>(LongArray<T> array)
        {
            return ref array.GetReference();
        }
    }
}
