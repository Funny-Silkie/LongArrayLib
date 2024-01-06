using LongArrayLib.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LongArrayLib
{
    public partial class LongArray
    {
        /// <summary>
        /// �z��̃T�C�Y��ύX���܂��B
        /// </summary>
        /// <typeparam name="T">�v�f�̌^</typeparam>
        /// <param name="array">�Ώۂ̔z��</param>
        /// <param name="size">�ύX��̃T�C�Y</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/>��0����</exception>
        public static void Resize<T>([NotNull] ref LongArray<T>? array, long size)
        {
            if (array is null)
            {
                array = size == 0 ? LongArray<T>.Empty : new LongArray<T>(size);
                return;
            }

            ThrowHelper.ThrowIfNegative(size);

            if (size == array.Length) return;
            if (size == 0)
            {
                array = LongArray<T>.Empty;
                return;
            }

            var newArray = new LongArray<T>(size, false);
            if (size < array.Length) Copy(array, newArray, size);
            else
            {
                Copy(array, newArray, array.Length);
                ClearCore(newArray, array.Length, size - array.Length);
            }
            array = newArray;
        }

        /// <summary>
        /// ����l�Ŗ������܂��B
        /// </summary>
        /// <typeparam name="T">�v�f�̌^</typeparam>
        /// <param name="array">��������z��</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>��<see langword="null"/></exception>
        public static void Clear<T>(LongArray<T> array)
        {
            ArgumentNullException.ThrowIfNull(array);

            ClearCore(array, 0, array.Length);
        }

        /// <summary>
        /// ����l�Ŗ������܂��B
        /// </summary>
        /// <typeparam name="T">�v�f�̌^</typeparam>
        /// <param name="array">��������z��</param>
        /// <param name="start">����l�Ŗ������̈�̊J�n�C���f�b�N�X</param>
        /// <param name="count">����l�Ŗ������̈�̗v�f��</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>��<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>�܂���<paramref name="count"/>���͈͊O</exception>
        public static void Clear<T>(LongArray<T> array, long start, long count)
        {
            ArgumentNullException.ThrowIfNull(array);
            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > array.Length) ThrowHelper.ThrowAsLargerLength(nameof(count));

            ClearCore(array, start, count);
        }

        /// <summary>
        /// ����l�Ŗ������܂��B
        /// </summary>
        /// <typeparam name="T">�v�f�̌^</typeparam>
        /// <param name="array">��������z��</param>
        /// <param name="start">����l�Ŗ������̈�̊J�n�C���f�b�N�X</param>
        /// <param name="count">����l�Ŗ������̈�̗v�f��</param>
        private static unsafe void ClearCore<T>(LongArray<T> array, long start, long count)
        {
            if (count == 0) return;

            void* ptr = UnsafeHelper.Increment<T>(array.AsPointer(), start);
            nuint bytesToClear = checked((nuint)(count * Unsafe.SizeOf<T>()));
            NativeMemory.Clear(ptr, bytesToClear);
        }

        /// <summary>
        /// �w�肵���v�f�Ŗ������܂��B
        /// </summary>
        /// <typeparam name="T">�v�f�̌^</typeparam>
        /// <param name="array">��������z��</param>
        /// <param name="value">���߂�l</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>��<see langword="null"/></exception>
        public static void Fill<T>(LongArray<T> array, T value)
        {
            ArgumentNullException.ThrowIfNull(array);

            FillCore(array, value, 0, array.Length);
        }

        /// <summary>
        /// �w�肵���v�f�Ŗ������܂��B
        /// </summary>
        /// <typeparam name="T">�v�f�̌^</typeparam>
        /// <param name="array">��������z��</param>
        /// <param name="value">���߂�l</param>
        /// <param name="start">���߂�̈�̊J�n�C���f�b�N�X</param>
        /// <param name="count"><paramref name="value"/>�Ŗ��߂�v�f��</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>��<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/>�܂���<paramref name="count"/>���͈͊O</exception>
        public static void Fill<T>(LongArray<T> array, T value, long start, long count)
        {
            ArgumentNullException.ThrowIfNull(array);
            if ((ulong)start >= (ulong)array.Length) ThrowHelper.ThrowAsInvalidIndex(nameof(start));
            ThrowHelper.ThrowIfNegative(count);
            if (start + count > array.Length) ThrowHelper.ThrowAsLargerLength(nameof(count));

            FillCore(array, value, start, count);
        }

        /// <summary>
        /// �w�肵���v�f�Ŗ������܂��B
        /// </summary>
        /// <typeparam name="T">�v�f�̌^</typeparam>
        /// <param name="array">��������z��</param>
        /// <param name="value">���߂�l</param>
        /// <param name="start">���߂�̈�̊J�n�C���f�b�N�X</param>
        /// <param name="count"><paramref name="value"/>�Ŗ��߂�v�f��</param>
        private static unsafe void FillCore<T>(LongArray<T> array, T value, long start, long count)
        {
            if (count == 0) return;

            void* arrayPtr = UnsafeHelper.Increment<T>(array.AsPointer(), start);
            if (Unsafe.SizeOf<T>() == 1)
            {
                NativeMemory.Fill(arrayPtr, checked((nuint)count), Unsafe.As<T, byte>(ref value));
                return;
            }

            ref T arrayReference = ref Unsafe.AsRef<T>(arrayPtr);
            for (long i = 0; i < count; i++)
            {
                arrayReference = value;
                arrayReference = ref Unsafe.Add(ref arrayReference, 1);
            }
        }
    }

    public partial class LongArray<T>
    {
        #region Explicit Interface Implementation

        #region ICollection`1

        void ICollection<T>.Add(T item) => ThrowHelper.ThrowAsFixedSize();

        void ICollection<T>.Clear() => ThrowHelper.ThrowAsFixedSize();

        bool ICollection<T>.Remove(T item)
        {
            ThrowHelper.ThrowAsFixedSize();
            return false;
        }

        #endregion ICollection`1

        #region IList

        int IList.Add(object? value)
        {
            ThrowHelper.ThrowAsFixedSize();
            return -1;
        }

        void IList.Clear() => ThrowHelper.ThrowAsFixedSize();

        void IList.Insert(int index, object? value) => ThrowHelper.ThrowAsFixedSize();

        void IList.Remove(object? value) => ThrowHelper.ThrowAsFixedSize();

        void IList.RemoveAt(int index) => ThrowHelper.ThrowAsFixedSize();

        #endregion IList

        #region IList`1

        void IList<T>.Insert(int index, T item) => ThrowHelper.ThrowAsFixedSize();

        void IList<T>.RemoveAt(int index) => ThrowHelper.ThrowAsFixedSize();

        #endregion IList`1

        #endregion Explicit Interface Implementation
    }
}
