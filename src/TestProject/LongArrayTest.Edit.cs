using LongArrayLib;
using System;
using System.Linq;

namespace TestProject
{
    public partial class LongArrayTest
    {
        /// <summary>
        /// <see cref="LongArray.Resize{T}(ref LongArray{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ResizeTest()
        {
            LongArray.Resize(ref array2, 5);
            Assert.Multiple(() =>
            {
                Assert.That(array2, Has.Length.EqualTo(5));
                Assert.That(array2[0], Is.EqualTo("hoge"));
                Assert.That(array2[1], Is.EqualTo("fuga"));
                Assert.That(array2[2], Is.EqualTo("piyo"));
                Assert.That(array2[3], Is.EqualTo("hoge"));
                Assert.That(array2[4], Is.EqualTo(default(string)));
            });

            LongArray.Resize(ref array2, 5);
            Assert.Multiple(() =>
            {
                Assert.That(array2, Has.Length.EqualTo(5));
                Assert.That(array2[0], Is.EqualTo("hoge"));
                Assert.That(array2[1], Is.EqualTo("fuga"));
                Assert.That(array2[2], Is.EqualTo("piyo"));
                Assert.That(array2[3], Is.EqualTo("hoge"));
                Assert.That(array2[4], Is.EqualTo(default(string)));
            });

            LongArray.Resize(ref array2, 3);
            Assert.Multiple(() =>
            {
                Assert.That(array2, Has.Length.EqualTo(3));
                Assert.That(array2[0], Is.EqualTo("hoge"));
                Assert.That(array2[1], Is.EqualTo("fuga"));
                Assert.That(array2[2], Is.EqualTo("piyo"));
            });

            LongArray.Resize(ref array1, 0);
            Assert.That(array1, Is.Empty);

            {
                LongArray<int> array = null!;
                LongArray.Resize(ref array, 10);

                Assert.Multiple(() =>
                {
                    Assert.That(array, Has.Length.EqualTo(10));
                    for (long i = 0; i < array.Length; i++) Assert.That(array[i], Is.EqualTo(default(int)));
                });
            }
            Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Resize(ref array1, -1));
        }

        /// <summary>
        /// <see cref="LongArray.Clear{T}(LongArray{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ClearTest1()
        {
            var array = (LongArray<string>)array2.Clone();
            LongArray.Clear(array);

            Assert.Multiple(() =>
            {
                for (long i = 0; i < array.Length; i++) Assert.That(array[i], Is.EqualTo(default(string)));
            });

            Assert.Throws<ArgumentNullException>(() => LongArray.Clear<int>(null!));
        }

        /// <summary>
        /// <see cref="LongArray.Clear{T}(LongArray{T}, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ClearTest2()
        {
            var array = (LongArray<string>)array2.Clone();
            LongArray.Clear(array, 0, 0);
            Assert.That(array.SequenceEqual(array2), Is.True);

            LongArray.Clear(array, 1, 2);

            Assert.Multiple(() =>
            {
                Assert.That(array[0], Is.Not.EqualTo(default(string)));
                Assert.That(array[1], Is.EqualTo(default(string)));
                Assert.That(array[2], Is.EqualTo(default(string)));
                Assert.That(array[3], Is.Not.EqualTo(default(string)));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Clear<int>(null!, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Clear(array2, -1, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Clear(array2, 5, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Clear(array2, 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Clear(array2, 1, 4));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Fill{T}(LongArray{T}, T)"/>を検証します。
        /// </summary>
        [Test]
        public void Fill1()
        {
            var array = (LongArray<string>)array2.Clone();
            LongArray.Fill(array, "Filled");
            Assert.Multiple(() =>
            {
                for (long i = 0; i < array.Length; i++) Assert.That(array[i], Is.EqualTo("Filled"));
            });

            Assert.Throws<ArgumentNullException>(() => LongArray.Fill(null!, 1));

            // byte size fill
            LongArray.Fill(array3, byte.MaxValue);
            Assert.Multiple(() =>
            {
                for (long i = 0; i < array3.Length; i++) Assert.That(array3[i], Is.EqualTo(byte.MaxValue));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Fill{T}(LongArray{T}, T, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void Fill2()
        {
            var array = (LongArray<string>)array2.Clone();

            LongArray.Fill(array, "Filled", 0, 0);
            Assert.That(array.SequenceEqual(array2), Is.True);

            LongArray.Fill(array, "Filled", 1, 2);
            Assert.Multiple(() =>
            {
                Assert.That(array[0], Is.Not.EqualTo("Filled"));
                Assert.That(array[1], Is.EqualTo("Filled"));
                Assert.That(array[2], Is.EqualTo("Filled"));
                Assert.That(array[3], Is.Not.EqualTo("Filled"));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Fill<int>(null!, 1, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Fill(array2, "Filled", -1, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Fill(array2, "Filled", 5, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Fill(array2, "Filled", 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Fill(array2, "Filled", 1, 4));
            });
        }
    }
}
