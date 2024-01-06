using LongArrayLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.System.Linq
{
    /// <summary>
    /// <see cref="LinqExtensionsTest"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class LinqExtensionsTest
    {
        /// <summary>
        /// <see cref="LinqExtensions.ToLongArray{T}(IEnumerable{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ToArrayTest()
        {
            {
                LongArray<int> source = LongArray.Create(1, 2, 3);
                LongArray<int> array = LinqExtensions.ToLongArray(source);

                Assert.Multiple(() =>
                {
                    Assert.That(array, Has.Length.EqualTo(source.Length));
                    Assert.That(source.SequenceEqual(array), Is.True);
                });
            }

            {
                var source = new[] { 1, 2, 3 };
                LongArray<int> array = LinqExtensions.ToLongArray(source);

                Assert.Multiple(() =>
                {
                    Assert.That(array, Has.Length.EqualTo(source.Length));
                    Assert.That(source.SequenceEqual(array), Is.True);
                });
            }

            {
                var source = new List<int> { 1, 2, 3 };
                LongArray<int> array = LinqExtensions.ToLongArray(source);

                Assert.Multiple(() =>
                {
                    Assert.That(array, Has.Length.EqualTo(source.Count));
                    Assert.That(source.SequenceEqual(array), Is.True);
                });
            }

            {
                IEnumerable<int> source = Iterator();
                LongArray<int> array = LinqExtensions.ToLongArray(source);

                Assert.That(source.SequenceEqual(array), Is.True);
            }

            Assert.Throws<ArgumentNullException>(() => LinqExtensions.ToLongArray<int>(null!));

            static IEnumerable<int> Iterator()
            {
                for (int i = 0; i < 1000; i++) yield return i;
            }
        }
    }
}
