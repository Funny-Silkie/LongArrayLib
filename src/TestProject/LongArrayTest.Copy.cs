using LongArrayLib;
using System;
using System.Linq;

namespace TestProject
{
    public partial class LongArrayTest
    {
        /// <summary>
        /// <see cref="LongArray{T}.Clone"/>を検証します。
        /// </summary>
        [Test]
        public void CloneTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.SequenceEqual((LongArray<long>)array1.Clone()), Is.True);
                Assert.That(array2.SequenceEqual((LongArray<string>)array2.Clone()), Is.True);
            });
        }

        /// <summary>
        /// <see cref="LongArray.Copy{T}(T[], LongArray{T}, int)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyTest1()
        {
            string[] source = [.. array2];
            var array = new LongArray<string>(10);
            LongArray.Copy(source, array, 3);

            Assert.Multiple(() =>
            {
                for (long i = 0; i < 3; i++) Assert.That(array[i], Is.EqualTo(source[i]));
                for (long i = 3; i < array.Length; i++) Assert.That(array[i], Is.EqualTo(default(string)));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(null!, array1, 3));
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(new long[3], null!, 3));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(source, new LongArray<string>(1), -1));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(source, new LongArray<string>(10), (int)(source.Length + 1)));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(source, new LongArray<string>(1), 2));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Copy{T}(T[], int, LongArray{T}, long, int)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyTest2()
        {
            string[] source = [.. array2];
            var array = new LongArray<string>(10);
            LongArray.Copy(source, 1, array, 1, 3);

            Assert.Multiple(() =>
            {
                Assert.That(array[0], Is.EqualTo(default(string)));
                for (long i = 1; i < 4; i++) Assert.That(array[i], Is.EqualTo(source[i]));
                for (long i = 4; i < array.Length; i++) Assert.That(array[i], Is.EqualTo(default(string)));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(null!, 0, array1, 3L, 3));
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(new long[3], 0, null!, 3L, 3));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(source, 0, new LongArray<string>(1), 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(source, -1, new LongArray<string>(10), 0, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(source, 5, new LongArray<string>(10), 0, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(source, 0, new LongArray<string>(10), -1, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(source, 0, new LongArray<string>(10), 10, 4));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(source, 0, new LongArray<string>(10), 0, 5));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(source, 0, new LongArray<string>(1), 0, 2));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Copy{T}(LongArray{T}, T[], int)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyTest3()
        {
            var array = new string[10];
            LongArray.Copy(array2, array, 3);

            Assert.Multiple(() =>
            {
                for (long i = 0; i < 3; i++) Assert.That(array[i], Is.EqualTo(array2[i]));
                for (long i = 3; i < array.Length; i++) Assert.That(array[i], Is.EqualTo(default(string)));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(null!, new long[3], 3));
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(array1, null!, 3));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, new string[1], -1));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(array2, new string[10], (int)(array2.Length + 1)));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(array2, new string[1], 2));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Copy{T}(LongArray{T}, long, T[], int, int)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyTest4()
        {
            var array = new string[10];
            LongArray.Copy(array2, 1, array, 1, 3);

            Assert.Multiple(() =>
            {
                Assert.That(array[0], Is.EqualTo(default(string)));
                for (long i = 1; i < 4; i++) Assert.That(array[i], Is.EqualTo(array2[i]));
                for (long i = 4; i < array.Length; i++) Assert.That(array[i], Is.EqualTo(default(string)));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(null!, 0L, new long[3], 3, 3));
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(array1, 0L, null!, 3, 3));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, 0, new string[1], 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, -1, new string[10], 0, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, 5, new string[10], 0, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, 0, new string[10], -1, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, 0, new string[10], 10, 4));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(array2, 0, new string[10], 0, 5));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(array2, 0, new string[1], 0, 2));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Copy{T}(LongArray{T}, LongArray{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyTest5()
        {
            var array = new LongArray<string>(10);
            LongArray.Copy(array2, array, 3);

            Assert.Multiple(() =>
            {
                for (long i = 0; i < 3; i++) Assert.That(array[i], Is.EqualTo(array2[i]));
                for (long i = 3; i < array.Length; i++) Assert.That(array[i], Is.EqualTo(default(string)));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(null!, array1, 3L));
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(array1, null!, 3L));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, new LongArray<string>(1), -1));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(array2, new LongArray<string>(10), (int)(array2.Length + 1)));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(array2, new LongArray<string>(1), 2));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Copy{T}(LongArray{T}, long, LongArray{T}, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyTest6()
        {
            var array = new LongArray<string>(10);
            LongArray.Copy(array2, 1, array, 1, 3);

            Assert.Multiple(() =>
            {
                Assert.That(array[0], Is.EqualTo(default(string)));
                for (long i = 1; i < 4; i++) Assert.That(array[i], Is.EqualTo(array2[i]));
                for (long i = 4; i < array.Length; i++) Assert.That(array[i], Is.EqualTo(default(string)));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(null!, 0L, array1, 3L, 3L));
                Assert.Throws<ArgumentNullException>(() => LongArray.Copy(array1, 0L, null!, 3L, 3L));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, 0, new LongArray<string>(1), 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, -1, new LongArray<string>(10), 0, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, 5, new LongArray<string>(10), 0, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, 0, new LongArray<string>(10), -1, 4));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Copy(array2, 0, new LongArray<string>(10), 10, 4));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(array2, 0, new LongArray<string>(10), 0, 5));
                Assert.Throws<ArgumentException>(() => LongArray.Copy(array2, 0, new LongArray<string>(1), 0, 2));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.CopyTo(T[], int)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToTest1()
        {
            var array = new long[4];
            array1.CopyTo(array, 1);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < array1.Length; i++) Assert.That(array[i + 1], Is.EqualTo(array1[i]));

                Assert.Throws<ArgumentNullException>(() => array1.CopyTo(null!, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array1.CopyTo(new long[3], -1));
                Assert.Throws<ArgumentException>(() => array1.CopyTo(new long[3], 1));
                Assert.Throws<ArgumentException>(() => array1.CopyTo(new long[10], 9));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.CopyTo(LongArray{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToTest2()
        {
            var array = new LongArray<long>(4);
            array1.CopyTo(array, 1);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < array1.Length; i++) Assert.That(array[i + 1], Is.EqualTo(array1[i]));

                Assert.Throws<ArgumentNullException>(() => array1.CopyTo(null!, 0L));
                Assert.Throws<ArgumentOutOfRangeException>(() => array1.CopyTo(new LongArray<long>(3), -1L));
                Assert.Throws<ArgumentException>(() => array1.CopyTo(new LongArray<long>(3), 1L));
                Assert.Throws<ArgumentException>(() => array1.CopyTo(new LongArray<long>(10), 9L));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.CopyTo(Memory{T})"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToTest3()
        {
            var memory = new Memory<long>(new long[4]);
            array1.CopyTo(memory);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < array1.Length; i++) Assert.That(memory.Span[i], Is.EqualTo(array1[i]));

                Assert.Throws<ArgumentException>(() => array1.CopyTo(new Memory<long>(new long[1])));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.CopyTo(Span{T})"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToTest4()
        {
            var span = new Span<long>(new long[4]);
            array1.CopyTo(span);

            for (int i = 0; i < array1.Length; i++) Assert.That(span[i], Is.EqualTo(array1[i]));
            Assert.Throws<ArgumentException>(() => array1.CopyTo(new Span<long>(new long[1])));
        }
    }
}
