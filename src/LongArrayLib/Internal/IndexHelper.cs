using System;

namespace LongArrayLib.Internal
{
    /// <summary>
    /// インデックス処理のヘルパークラスです。
    /// </summary>
    internal static class IndexHelper
    {
        /// <summary>
        /// <see cref="Index"/>から実際のインデックスを取得します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="length">対象の配列長</param>
        /// <returns>実際のインデックス</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/>が無効</exception>
        public static long GetLongOffset(this Index index, long length)
        {
            return GetLongOffsetCore(index.Value, index.IsFromEnd, length);
        }

        /// <summary>
        /// 実際のインデックスを取得します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="fromEnd">末尾からのインデックスかどうか</param>
        /// <param name="length">対象の配列長</param>
        /// <returns>実際のインデックス</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/>が無効</exception>
        private static long GetLongOffsetCore(int index, bool fromEnd, long length)
        {
            if (fromEnd)
            {
                if (index <= 0 || length < index) ThrowHelper.ThrowAsInvalidIndex();
                return length - index;
            }

            if ((uint)index >= (uint)length) ThrowHelper.ThrowAsInvalidIndex();

            return index;
        }

        /// <summary>
        /// <see cref="Range"/>から実際のインデックスを取得します。
        /// </summary>
        /// <param name="range">範囲</param>
        /// <param name="length">対象の配列長</param>
        /// <returns>実際のインデックス</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="range"/>が無効</exception>
        public static (long start, long end) GetLongOffsets(this Range range, long length)
        {
            long start = GetLongOffsetCore(range.Start.Value, range.Start.IsFromEnd, length);
            long end = GetLongOffsetCore(range.End.IsFromEnd ? (range.End.Value + 1) : (range.End.Value - 1), range.End.IsFromEnd, length);

            return (start, end);
        }
    }
}
