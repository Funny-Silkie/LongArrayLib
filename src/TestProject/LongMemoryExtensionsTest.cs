using LongArrayLib;
using System;

namespace TestProject
{
    /// <summary>
    /// <see cref="LongMemoryExtensions"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class LongMemoryExtensionsTest
    {
        /// <summary>
        /// <see cref="LongMemoryExtensions.ToLongArray{T}(Memory{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ToLongArrayFromMemory()
        {
            var memory = new Memory<string>(["hoge", "fuga", "piyo"]);
            LongArray<string> array = memory.ToLongArray();

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(3));
                Assert.That(array[0], Is.EqualTo("hoge"));
                Assert.That(array[1], Is.EqualTo("fuga"));
                Assert.That(array[2], Is.EqualTo("piyo"));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.ToLongArray{T}(ReadOnlyMemory{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ToLongArrayFromReadOnlyMemory()
        {
            var memory = new ReadOnlyMemory<string>(["hoge", "fuga", "piyo"]);
            LongArray<string> array = memory.ToLongArray();

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(3));
                Assert.That(array[0], Is.EqualTo("hoge"));
                Assert.That(array[1], Is.EqualTo("fuga"));
                Assert.That(array[2], Is.EqualTo("piyo"));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.ToLongArray{T}(Span{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ToLongArrayFromSpan()
        {
            var span = new Span<string>(["hoge", "fuga", "piyo"]);
            LongArray<string> array = span.ToLongArray();

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(3));
                Assert.That(array[0], Is.EqualTo("hoge"));
                Assert.That(array[1], Is.EqualTo("fuga"));
                Assert.That(array[2], Is.EqualTo("piyo"));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.ToLongArray{T}(ReadOnlySpan{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ToLongArrayFromReadOnlySpan()
        {
            var span = new ReadOnlySpan<string>(["hoge", "fuga", "piyo"]);
            LongArray<string> array = span.ToLongArray();

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(3));
                Assert.That(array[0], Is.EqualTo("hoge"));
                Assert.That(array[1], Is.EqualTo("fuga"));
                Assert.That(array[2], Is.EqualTo("piyo"));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.ToLongArray{T}(string)"/>を検証します。
        /// </summary>
        [Test]
        public void ToLongArrayFromString()
        {
            var text = "abcdefg1234567ABCDEFG";
            LongArray<char> array = text.ToLongArray();

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(text.Length));
                for (int i = 0; i < text.Length; i++) Assert.That(array[i], Is.EqualTo(text[i]));
            });
        }

#pragma warning disable NUnit2045 // Use Assert.Multiple

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsSpan{T}(LongArray{T}?)"/>を検証します。
        /// </summary>
        [Test]
        public void AsSpanTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            Span<long> span = array.AsSpan();
            Assert.That(span.Length, Is.EqualTo(array.Length));
            for (int i = 0; i < span.Length; i++) Assert.That(span[i], Is.EqualTo(array[i]));

            Assert.That(LongArray<int>.Empty.AsSpan().IsEmpty, Is.True);

            Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 1L).AsSpan());
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsSpan{T}(LongArray{T}?, int)"/>を検証します。
        /// </summary>
        [Test]
        public void AsSpanWithInt32StartTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            Span<long> span = array.AsSpan(1);
            Assert.That(span.Length, Is.EqualTo(3));
            Assert.That(span[0], Is.EqualTo(-1L));
            Assert.That(span[1], Is.EqualTo(-2L));
            Assert.That(span[2], Is.EqualTo(3L));

            Assert.Multiple(() =>
            {
                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 2L).AsSpan(1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(-1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(4));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsSpan{T}(LongArray{T}?, long)"/>を検証します。
        /// </summary>
        [Test]
        public void AsSpanWithInt64StartTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            Span<long> span = array.AsSpan(1L);
            Assert.That(span.Length, Is.EqualTo(3));
            Assert.That(span[0], Is.EqualTo(-1L));
            Assert.That(span[1], Is.EqualTo(-2L));
            Assert.That(span[2], Is.EqualTo(3L));

            Assert.Multiple(() =>
            {
                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 2L).AsSpan(1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(-1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(4L));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsSpan{T}(LongArray{T}?, Index)"/>を検証します。
        /// </summary>
        [Test]
        public void AsSpanWithIndexStartTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            // forward
            {
                Span<long> span = array.AsSpan((Index)1);
                Assert.That(span.Length, Is.EqualTo(3));
                Assert.That(span[0], Is.EqualTo(-1L));
                Assert.That(span[1], Is.EqualTo(-2L));
                Assert.That(span[2], Is.EqualTo(3L));
            }
            // reverse
            {
                Span<long> span = array.AsSpan(^2);
                Assert.That(span.Length, Is.EqualTo(2));
                Assert.That(span[0], Is.EqualTo(-2L));
                Assert.That(span[1], Is.EqualTo(3L));
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 2L).AsSpan((Index)1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan((Index)(-1)));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan((Index)4));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(^0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(^5));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsSpan{T}(LongArray{T}?, long, int)"/>を検証します。
        /// </summary>
        [Test]
        public void AsSpanWithIntRangeTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            Span<long> span = array.AsSpan(1, 2);
            Assert.That(span.Length, Is.EqualTo(2));
            Assert.That(span[0], Is.EqualTo(-1L));
            Assert.That(span[1], Is.EqualTo(-2L));

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(-1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(4, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(0, 5));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(3, 2));
            });
        }

#pragma warning restore NUnit2045 // Use Assert.Multiple

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsSpan{T}(LongArray{T}?, Range)"/>を検証します。
        /// </summary>
        [Test]
        public void AsSpanWithStructRangeTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L]);

            Assert.Multiple(() =>
            {
                Assert.That(array.AsSpan(0..3).SequenceEqual([0L, -1L, -2L]), Is.True);
                Assert.That(array.AsSpan(0..^0).SequenceEqual([0L, -1L, -2L]), Is.True);
                Assert.That(array.AsSpan(1..3).SequenceEqual([-1L, -2L]), Is.True);
                Assert.That(array.AsSpan(1..^0).SequenceEqual([-1L, -2L]), Is.True);
                Assert.That(array.AsSpan(1..2).SequenceEqual([-1L]), Is.True);
                Assert.That(array.AsSpan(1..^1).SequenceEqual([-1L]), Is.True);
                Assert.That(array.AsSpan(1..1).Length, Is.EqualTo(0));
                Assert.That(array.AsSpan(1..^2).Length, Is.EqualTo(0));
                Assert.That(array.AsSpan(^2..1).Length, Is.EqualTo(0));
                Assert.That(array.AsSpan(^2..^2).Length, Is.EqualTo(0));

                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(4..));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(..5));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(^0..));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsSpan(..^5));
                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 3L).AsSpan(0..^0));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsMemory{T}(LongArray{T}?)"/>を検証します。
        /// </summary>
        [Test]
        public void AsMemoryTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            Memory<long> memory = array.AsMemory();
            Assert.Multiple(() =>
            {
                Assert.That(memory.Length, Is.EqualTo(array.Length));
                for (int i = 0; i < memory.Length; i++) Assert.That(memory.Span[i], Is.EqualTo(array[i]));
            });

            Assert.That(LongArray<int>.Empty.AsMemory().IsEmpty, Is.True);

            Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 1L).AsMemory());
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsMemory{T}(LongArray{T}?, int)"/>を検証します。
        /// </summary>
        [Test]
        public void AsMemoryWithInt32StartTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            Memory<long> memory = array.AsMemory(1);
            Assert.Multiple(() =>
            {
                Assert.That(memory.Length, Is.EqualTo(3));
                Assert.That(memory.Span[0], Is.EqualTo(-1L));
                Assert.That(memory.Span[1], Is.EqualTo(-2L));
                Assert.That(memory.Span[2], Is.EqualTo(3L));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 2L).AsMemory(1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(-1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(4));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsMemory{T}(LongArray{T}?, long)"/>を検証します。
        /// </summary>
        [Test]
        public void AsMemoryWithInt64StartTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            Memory<long> memory = array.AsMemory(1L);
            Assert.Multiple(() =>
            {
                Assert.That(memory.Length, Is.EqualTo(3));
                Assert.That(memory.Span[0], Is.EqualTo(-1L));
                Assert.That(memory.Span[1], Is.EqualTo(-2L));
                Assert.That(memory.Span[2], Is.EqualTo(3L));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 2L).AsMemory(1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(-1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(4L));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsMemory{T}(LongArray{T}?, Index)"/>を検証します。
        /// </summary>
        [Test]
        public void AsMemoryWithIndexStartTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            // forward
            {
                Memory<long> memory = array.AsMemory((Index)1);
                Assert.Multiple(() =>
                {
                    Assert.That(memory.Length, Is.EqualTo(3));
                    Assert.That(memory.Span[0], Is.EqualTo(-1L));
                    Assert.That(memory.Span[1], Is.EqualTo(-2L));
                    Assert.That(memory.Span[2], Is.EqualTo(3L));
                });
            }

            // reverse
            {
                Memory<long> memory = array.AsMemory(^2);
                Assert.Multiple(() =>
                {
                    Assert.That(memory.Length, Is.EqualTo(2));
                    Assert.That(memory.Span[0], Is.EqualTo(-2L));
                    Assert.That(memory.Span[1], Is.EqualTo(3L));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 2L).AsMemory((Index)1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory((Index)(-1)));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory((Index)4));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(^0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(^5));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsMemory{T}(LongArray{T}?, long, int)"/>を検証します。
        /// </summary>
        [Test]
        public void AsMemoryWithIntRangeTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L, 3L]);

            Memory<long> memory = array.AsMemory(1, 2);
            Assert.Multiple(() =>
            {
                Assert.That(memory.Length, Is.EqualTo(2));
                Assert.That(memory.Span[0], Is.EqualTo(-1L));
                Assert.That(memory.Span[1], Is.EqualTo(-2L));
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(-1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(4, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(0, 5));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(3, 2));
            });
        }

        /// <summary>
        /// <see cref="LongMemoryExtensions.AsMemory{T}(LongArray{T}?, Range)"/>を検証します。
        /// </summary>
        [Test]
        public void AsMemoryWithStructRangeTest()
        {
            using LongArray<long> array = LongArray.Create([0L, -1L, -2L]);

            Assert.Multiple(() =>
            {
                Assert.That(array.AsMemory(0..3).Span.SequenceEqual([0L, -1L, -2L]), Is.True);
                Assert.That(array.AsMemory(0..^0).Span.SequenceEqual([0L, -1L, -2L]), Is.True);
                Assert.That(array.AsMemory(1..3).Span.SequenceEqual([-1L, -2L]), Is.True);
                Assert.That(array.AsMemory(1..^0).Span.SequenceEqual([-1L, -2L]), Is.True);
                Assert.That(array.AsMemory(1..2).Span.SequenceEqual([-1L]), Is.True);
                Assert.That(array.AsMemory(1..^1).Span.SequenceEqual([-1L]), Is.True);
                Assert.That(array.AsMemory(1..1).Length, Is.EqualTo(0));
                Assert.That(array.AsMemory(1..^2).Length, Is.EqualTo(0));
                Assert.That(array.AsMemory(^2..1).Length, Is.EqualTo(0));
                Assert.That(array.AsMemory(^2..^2).Length, Is.EqualTo(0));

                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(4..));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(..5));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(^0..));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.AsMemory(..^5));
                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 3L).AsMemory(0..^0));
            });
        }
    }
}
