using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace LongArrayLib
{
    // !!REMARKS!!
    // Binary serialize is obsolete way of serialization.
    // This code will be removed.

    [Serializable]
    public partial class LongArray<T> : ISerializable
    {
        private const string ItemsSerializationName = "Items";
        private const string LengthSerializationName = "Length";

        /// <summary>
        /// <see cref="LongArray{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info">シリアライズするオブジェクトの情報を格納する<see cref="SerializationInfo"/>のインスタンス</param>
        /// <param name="context">使用する<see cref="StreamingContext"/>のインスタンス</param>
        /// <exception cref="ArgumentNullException"><paramref name="info"/>が<see langword="null"/></exception>
        private unsafe LongArray(SerializationInfo info, StreamingContext context)
        {
            ArgumentNullException.ThrowIfNull(info);

            Length = info.GetInt32(LengthSerializationName);
            if (Length == 0) return;

            var list = (List<T[]>)info.GetValue(ItemsSerializationName, typeof(List<T[]>))!;
            long length = list.Sum(x => (long)x.Length);

            items = NativeMemory.Alloc(checked((nuint)length), (nuint)Unsafe.SizeOf<T>());
            long offset = 0L;

            foreach (T[] array in list)
            {
                LongArray.Copy(array, 0, this, offset, array.Length);
                offset += array.Length;
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ArgumentNullException.ThrowIfNull(info);

            info.AddValue(LengthSerializationName, Length);
            if (Length > 0)
            {
                var list = new List<T[]>();
                long offset = 0L;
                for (long length = Length; length > 0; length -= Array.MaxLength)
                {
                    int arraySize = length >= Array.MaxLength ? Array.MaxLength : (int)length;
                    var array = new T[arraySize];
                    LongArray.Copy(this, offset, array, 0, arraySize);
                    offset += arraySize;
                    list.Add(array);
                }
                info.AddValue(ItemsSerializationName, list);
            }
        }
    }
}
