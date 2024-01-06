using LongArrayLib;
using System;
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
    }
}
