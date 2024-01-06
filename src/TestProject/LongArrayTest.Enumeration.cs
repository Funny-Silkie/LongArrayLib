using LongArrayLib;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    public partial class LongArrayTest
    {
        /// <summary>
        /// 列挙をテストします。
        /// </summary>
        [Test]
        public void EnumerationTest()
        {
            Assert.Multiple(() =>
            {
                {
                    long expected = 0L;
                    foreach (long actual in array1) Assert.That(expected--, Is.EqualTo(actual));
                }
                {
                    using var enumerator = array2.GetEnumerator();

                    Assert.That(enumerator.Current, Is.EqualTo(null));

                    Assert.That(enumerator.MoveNext(), Is.True);
                    Assert.That(enumerator.Current, Is.EqualTo("hoge"));

                    Assert.That(enumerator.MoveNext(), Is.True);
                    Assert.That(enumerator.Current, Is.EqualTo("fuga"));

                    Assert.That(enumerator.MoveNext(), Is.True);
                    Assert.That(enumerator.Current, Is.EqualTo("piyo"));

                    Assert.That(enumerator.MoveNext(), Is.True);
                    Assert.That(enumerator.Current, Is.EqualTo("hoge"));

                    Assert.That(enumerator.MoveNext(), Is.False);
                    Assert.That(enumerator.Current, Is.EqualTo(null));
                }
            });
        }

        /// <summary>
        /// <see cref="ReadOnlySpan{T}"/>による列挙をテストします。
        /// </summary>
        [Test]
        public void SpanEnumerationTest()
        {
            long index = 0L;
            foreach (ReadOnlySpan<byte> span in array3.GetSpanEnumerator(2))
            {
                Assert.That(span.Length, Is.EqualTo(2));
                Assert.That(span[0], Is.EqualTo(array3[index++]));
                Assert.That(span[1], Is.EqualTo(array3[index++]));
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.GetSpanEnumerator(0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.GetSpanEnumerator(-1));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.ForEach(Action{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ForEachTest()
        {
            {
                var list = new List<string>();
                array2.ForEach(list.Add);

                Assert.That(array2.SequenceEqual(list), Is.True);
            }

            Assert.Throws<ArgumentNullException>(() => array1.ForEach(null!));
        }

        /// <summary>
        /// <see cref="LongArray{T}.ForEachChunk{TArg}(ReadOnlySpanAction{T, TArg}, TArg, int)"/>を検証します。
        /// </summary>
        [Test]
        public void ForEachChunkTest()
        {
            string text = string.Empty;
            array3.ForEachChunk((x, y) => text += string.Join(y, x.ToArray()) + "\n", "+", 3);

            Assert.That(text, Is.EqualTo("10+11+12\n13+14+15\n16+17+18\n19\n"));

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => array1.ForEachChunk(null!, 1, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array1.ForEachChunk((x, y) => { }, 1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array1.ForEachChunk((x, y) => { }, 1, -1));
            });
        }
    }
}
