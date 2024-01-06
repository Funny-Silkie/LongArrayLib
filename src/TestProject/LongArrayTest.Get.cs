using LongArrayLib;
using System;
using System.Linq;

namespace TestProject
{
    public partial class LongArrayTest
    {
        /// <summary>
        /// <see cref="LongArray{T}.get_Item(long)"/>を検証します。
        /// </summary>
        [Test]
        public void IndexerByInt64()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1[0L], Is.EqualTo(0L));
                Assert.That(array1[1L], Is.EqualTo(-1L));
                Assert.That(array1[2L], Is.EqualTo(-2L));
                Assert.Throws<IndexOutOfRangeException>(() => _ = array1[3L]);
                Assert.Throws<IndexOutOfRangeException>(() => _ = array1[-1L]);
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.get_Item(int)"/>を検証します。
        /// </summary>
        [Test]
        public void IndexerByInt32()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2[0], Is.EqualTo("hoge"));
                Assert.That(array2[1], Is.EqualTo("fuga"));
                Assert.That(array2[2], Is.EqualTo("piyo"));
                Assert.That(array2[3], Is.EqualTo("hoge"));
                Assert.Throws<IndexOutOfRangeException>(() => _ = array2[4]);
                Assert.Throws<IndexOutOfRangeException>(() => _ = array2[-1]);
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.get_Item(Index)"/>を検証します。
        /// </summary>
        [Test]
        public void IndexerByIndexStruct()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2[(Index)0], Is.EqualTo("hoge"));
                Assert.That(array2[(Index)1], Is.EqualTo("fuga"));
                Assert.That(array2[(Index)2], Is.EqualTo("piyo"));
                Assert.That(array2[(Index)3], Is.EqualTo("hoge"));
                Assert.Throws<IndexOutOfRangeException>(() => _ = array2[(Index)4]);
                Assert.That(array2[^1], Is.EqualTo("hoge"));
                Assert.That(array2[^2], Is.EqualTo("piyo"));
                Assert.That(array2[^3], Is.EqualTo("fuga"));
                Assert.That(array2[^4], Is.EqualTo("hoge"));
                Assert.Throws<IndexOutOfRangeException>(() => _ = array2[^0]);
                Assert.Throws<IndexOutOfRangeException>(() => _ = array2[^5]);
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.get_Item(Range)"/>を検証します。
        /// </summary>
        [Test]
        public void IndexerByRange()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1[0..3].SequenceEqual(array1), Is.True);
                Assert.That(array1[0..^0].SequenceEqual(array1), Is.True);
                Assert.That(array1[1..3].SequenceEqual([-1L, -2L]), Is.True);
                Assert.That(array1[1..^0].SequenceEqual([-1L, -2L]), Is.True);
                Assert.That(array1[1..2].SequenceEqual([-1L]), Is.True);
                Assert.That(array1[1..^1].SequenceEqual([-1L]), Is.True);
                Assert.That(array1[1..1], Is.Empty);
                Assert.That(array1[1..^2], Is.Empty);
                Assert.That(array1[^2..1], Is.Empty);
                Assert.That(array1[^2..^2], Is.Empty);

                Assert.Throws<IndexOutOfRangeException>(() => _ = array1[4..]);
                Assert.Throws<IndexOutOfRangeException>(() => _ = array1[..5]);
                Assert.Throws<IndexOutOfRangeException>(() => _ = array1[^0..]);
                Assert.Throws<IndexOutOfRangeException>(() => _ = array1[..^5]);
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.GetRange(long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void GetRangeTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.GetRange(0, 4).SequenceEqual(array2), Is.True);
                Assert.That(array2.GetRange(1, 2).SequenceEqual(["fuga", "piyo"]), Is.True);
                Assert.That(array2.GetRange(0, 0), Is.Empty);
                Assert.That(array2.GetRange(3, 0), Is.Empty);

                Assert.Throws<ArgumentOutOfRangeException>(() => array2.GetRange(-1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.GetRange(0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.GetRange(0, 5));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.GetRange(4, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.GetRange(2, 4));
            });
        }
    }
}
