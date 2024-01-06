using LongArrayLib;
using System;
using System.Linq;

namespace TestProject
{
    public partial class LongArrayTest
    {
        /// <summary>
        /// <see cref="LongArray{T}.ConverAll{TOut}(Converter{T, TOut})"/>を検証します。
        /// </summary>
        [Test]
        public void ConvertAllTest()
        {
            {
                LongArray<char> array = array2.ConverAll(x => x[0]);
                Assert.Multiple(() =>
                {
                    Assert.That(array, Has.Length.EqualTo(array2.Length));
                    Assert.That(array.SequenceEqual(['h', 'f', 'p', 'h']), Is.True);
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(LongArray<int>.Empty.ConverAll(x => x), Is.Empty);
                Assert.That(array1.ConverAll(x => -x).SequenceEqual([0L, 1L, 2L]));
                Assert.That(array2.ConverAll(x => x.StartsWith("ho")).SequenceEqual([true, false, false, true]), Is.True);

                Assert.Throws<ArgumentNullException>(() => array1.ConverAll<object>(null!));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.ToArray"/>を検証します。
        /// </summary>
        [Test]
        public void ToArrayTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.ToArray().SequenceEqual([0L, -1L, -2L]));
                Assert.That(array2.ToArray().SequenceEqual(["hoge", "fuga", "piyo", "hoge"]));
                Assert.That(LongArray<int>.Empty.ToArray(), Is.Empty);

                Assert.Throws<InvalidOperationException>(() => new LongArray<object>(int.MaxValue).ToArray());
            });
        }

        /// <summary>
        /// 配列への明示的変換を検証します。
        /// </summary>
        [Test]
        public void ExplicitOp2ArrayTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(((long[])array1).SequenceEqual([0L, -1L, -2L]));
                Assert.That(((string[])array2).SequenceEqual(["hoge", "fuga", "piyo", "hoge"]));
                Assert.That((int[])LongArray<int>.Empty, Is.Empty);

                Assert.Throws<InvalidOperationException>(() => _ = (object[])new LongArray<object>(int.MaxValue));
            });
        }

        /// <summary>
        /// <see cref="Span{T}"/>への明示的変換を検証します。
        /// </summary>
        [Test]
        public void ExplicitOp2SpanTest()
        {
            Span<long> span = (Span<long>)array1;
            Assert.That(span.Length, Is.EqualTo(array1.Length));
            for (int i = 0; i < span.Length; i++) Assert.That(span[i], Is.EqualTo(array1[i]));

            Assert.That(((Span<int>)LongArray<int>.Empty).IsEmpty, Is.True);

            Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 1L).AsSpan());
        }

        /// <summary>
        /// <see cref="ReadOnlySpan{T}"/>への明示的変換を検証します。
        /// </summary>
        [Test]
        public void ExplicitOp2ReadOnlySpanTest()
        {
            ReadOnlySpan<long> span = (ReadOnlySpan<long>)array1;
            Assert.That(span.Length, Is.EqualTo(array1.Length));
            for (int i = 0; i < span.Length; i++) Assert.That(span[i], Is.EqualTo(array1[i]));

            Assert.That(((ReadOnlySpan<int>)LongArray<int>.Empty).IsEmpty, Is.True);

            Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 1L).AsSpan());
        }

        /// <summary>
        /// <see cref="Memory{T}"/>への明示的変換を検証します。
        /// </summary>
        [Test]
        public void ExplicitOp2MemoryTest()
        {
            Memory<long> memory = (Memory<long>)array1;
            Assert.That(memory.Length, Is.EqualTo(array1.Length));
            for (int i = 0; i < memory.Length; i++) Assert.That(memory.Span[i], Is.EqualTo(array1[i]));

            Assert.Multiple(() =>
            {
                Assert.That(((Memory<int>)LongArray<int>.Empty).Length, Is.EqualTo(0));

                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 1L).AsMemory());
            });
        }

        /// <summary>
        /// <see cref="ReadOnlyMemory{T}"/>への明示的変換を検証します。
        /// </summary>
        [Test]
        public void ExplicitOp2ReadOnlyMemoryTest()
        {
            ReadOnlyMemory<long> memory = (ReadOnlyMemory<long>)array1;
            Assert.That(memory.Length, Is.EqualTo(array1.Length));
            for (int i = 0; i < memory.Length; i++) Assert.That(memory.Span[i], Is.EqualTo(array1[i]));

            Assert.Multiple(() =>
            {
                Assert.That(((ReadOnlyMemory<int>)LongArray<int>.Empty).Length, Is.EqualTo(0));

                Assert.Throws<OverflowException>(() => new LongArray<byte>(int.MaxValue + 1L).AsMemory());
            });
        }
    }
}
