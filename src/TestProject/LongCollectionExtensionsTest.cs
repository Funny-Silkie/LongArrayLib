using LongArrayLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    /// <summary>
    /// <see cref="LongCollectionExtensions"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class LongCollectionExtensionsTest
    {
        /// <summary>
        /// <see cref="LongCollectionExtensions.ToLongArray{T}(List{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ToLongArrayFromList()
        {
            var list = new List<string>() { "hoge", "fuga", "piyo" };
            LongArray<string> array = list.ToLongArray();

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(3));
                Assert.That(array[0], Is.EqualTo("hoge"));
                Assert.That(array[1], Is.EqualTo("fuga"));
                Assert.That(array[2], Is.EqualTo("piyo"));
            });

            Assert.Throws<ArgumentNullException>(() => LongCollectionExtensions.ToLongArray<int>(null!));
        }

        /// <summary>
        /// <see cref="LongCollectionExtensions.CopyTo{T}(ICollection{T}, LongArray{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToFromICollection()
        {
            ICollection<long> source;

            // with T[]
            {
                source = new[] { 0L, -1L, -2L };
                var destination = new LongArray<long>(4);

                source.CopyTo(destination, 1L);

                Assert.That(destination.Skip(1).Take(3).SequenceEqual(source), Is.True);
            }

            // with LongArray<T>
            {
                source = LongArray.Create([0L, -1L, -2L]);
                var destination = new LongArray<long>(4);

                source.CopyTo(destination, 1L);

                Assert.That(destination.Skip(1).Take(3).SequenceEqual(source), Is.True);
            }

            // with other List<T>
            {
                source = new List<long>() { 0L, -1L, -2L };
                var destination = new LongArray<long>(4);

                source.CopyTo(destination, 1L);

                Assert.That(destination.Skip(1).Take(3).SequenceEqual(source), Is.True);
            }

            // with other ICollection<T>
            {
                source = new HashSet<long>() { 0L, -1L, -2L };
                var destination = new LongArray<long>(4);

                source.CopyTo(destination, 1L);

                Assert.That(destination.Skip(1).Take(3).SequenceEqual(source), Is.True);
            }

            {
                source = new HashSet<long>() { 0L, -1L, -2L };
                var destination = new LongArray<long>(4);

                Assert.Multiple(() =>
                {
                    Assert.Throws<ArgumentNullException>(() => LongCollectionExtensions.CopyTo((ICollection<long>)null!, destination, 0L));
                    Assert.Throws<ArgumentNullException>(() => source.CopyTo(null!, 0L));
                    Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(new LongArray<long>(3), -1L));
                    Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(3), 1L));
                    Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(10), 9L));
                });
            }
        }

        /// <summary>
        /// <see cref="LongCollectionExtensions.CopyTo{T}(List{T}, LongArray{T})"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToFromList1()
        {
            var source = new List<long>() { 0L, -1L, -2L };
            var destination = new LongArray<long>(4);

            source.CopyTo(destination);

            Assert.That(destination.Take(3).SequenceEqual(source), Is.True);

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongCollectionExtensions.CopyTo(null!, destination));
                Assert.Throws<ArgumentNullException>(() => source.CopyTo(null!));
            });
        }

        /// <summary>
        /// <see cref="LongCollectionExtensions.CopyTo{T}(List{T}, LongArray{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToFromList2()
        {
            var source = new List<long>() { 0L, -1L, -2L };
            var destination = new LongArray<long>(4);

            source.CopyTo(destination, 1L);

            Assert.That(destination.Skip(1).Take(3).SequenceEqual(source), Is.True);

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongCollectionExtensions.CopyTo((List<long>)null!, destination, 0L));
                Assert.Throws<ArgumentNullException>(() => source.CopyTo(null!, 0L));
                Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(new LongArray<long>(3), -1L));
                Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(3), 1L));
                Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(10), 9L));
            });
        }

        /// <summary>
        /// <see cref="LongCollectionExtensions.CopyTo{T}(List{T}, int, LongArray{T}, long, int)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToFromList3()
        {
            var source = new List<long>() { 0L, -1L, -2L, 3L };
            var destination = new LongArray<long>(4);

            source.CopyTo(1, destination, 1L, 2);

            Assert.That(destination.Skip(1).Take(2).SequenceEqual(source.Skip(1).Take(2)), Is.True);

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongCollectionExtensions.CopyTo(null!, 0, destination, 0L, 1));
                Assert.Throws<ArgumentNullException>(() => source.CopyTo(0, null!, 0L, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(-1, new LongArray<long>(3), 0L, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(0, new LongArray<long>(3), -1L, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(0, new LongArray<long>(3), 0L, -1));
                Assert.Throws<ArgumentException>(() => source.CopyTo(0, new LongArray<long>(3), 1L, 4));
                Assert.Throws<ArgumentException>(() => source.CopyTo(0, new LongArray<long>(10), 9L, 4));
                Assert.Throws<ArgumentException>(() => source.CopyTo(1, new LongArray<long>(10), 0L, 4));
            });
        }

        /// <summary>
        /// <see cref="LongCollectionExtensions.CopyTo{T}(Stack{T}, LongArray{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToFromStack()
        {
            var source = new Stack<long>();
            source.Push(0L);
            source.Push(-1L);
            source.Push(-2L);

            var destination = new LongArray<long>(4);

            source.CopyTo(destination, 1L);

            Assert.That(destination.Skip(1).Take(3).SequenceEqual(source.Reverse()), Is.True);

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongCollectionExtensions.CopyTo((Stack<long>)null!, destination, 0L));
                Assert.Throws<ArgumentNullException>(() => source.CopyTo(null!, 0L));
                Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(new LongArray<long>(3), -1L));
                Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(3), 1L));
                Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(10), 9L));
            });
        }

        /// <summary>
        /// <see cref="LongCollectionExtensions.CopyTo{T}(Queue{T}, LongArray{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToFromQueue()
        {
            var source = new Queue<long>();
            source.Enqueue(0L);
            source.Enqueue(-1L);
            source.Enqueue(-2L);

            var destination = new LongArray<long>(4);

            source.CopyTo(destination, 1L);

            Assert.That(destination.Skip(1).Take(3).SequenceEqual(source), Is.True);

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongCollectionExtensions.CopyTo((Queue<long>)null!, destination, 0L));
                Assert.Throws<ArgumentNullException>(() => source.CopyTo(null!, 0L));
                Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(new LongArray<long>(3), -1L));
                Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(3), 1L));
                Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(10), 9L));
            });
        }
    }
}
