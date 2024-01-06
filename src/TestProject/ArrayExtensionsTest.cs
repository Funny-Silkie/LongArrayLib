using LongArrayLib;
using System;
using System.Collections.Generic;

namespace TestProject
{
    /// <summary>
    /// <see cref="ArrayExtensions"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class ArrayExtensionsTest
    {
        /// <summary>
        /// <see cref="ArrayExtensions.CopyTo{T}(T[], LongArray{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void CopyToTest()
        {
            var source = new[] { 0L, -1L, -2L };
            var destination = new LongArray<long>(4);

            source.CopyTo(destination, 1L);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < source.Length; i++) Assert.That(destination[i + 1], Is.EqualTo(source[i]));

                Assert.Throws<ArgumentNullException>(() => ArrayExtensions.CopyTo(null!, destination, 0L));
                Assert.Throws<ArgumentNullException>(() => source.CopyTo((LongArray<long>)null!, 0L));
                Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(new LongArray<long>(3), -1L));
                Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(3), 1L));
                Assert.Throws<ArgumentException>(() => source.CopyTo(new LongArray<long>(10), 9L));
            });
        }
    }
}
