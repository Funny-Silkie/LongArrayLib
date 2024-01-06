using LongArrayLib;
using System;
using System.Linq;

namespace TestProject
{
    public partial class LongArrayTest
    {
        /// <summary>
        /// 長大サイズのインスタンスを検証します。
        /// </summary>
        [Test]
        public void LongAllocation()
        {
            var array = new LongArray<long>((long)int.MaxValue + 10);
            for (long i = int.MaxValue; i < array.Length; i++) array[i] = i - int.MaxValue;

            Assert.That(array.AsSpan(int.MaxValue, 10).ToArray().SequenceEqual(Enumerable.Range(0, 10).Select(x => (long)x)), Is.True);
        }

        /// <summary>
        /// <see cref="LongArray.Create{T}(T[])"/>を検証します。
        /// </summary>
        [Test]
        public void CreateTest()
        {
            LongArray<string> array = LongArray.Create(["A", "B", "C", "D", "abc"]);

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(5L));
                Assert.That(array[0], Is.EqualTo("A"));
                Assert.That(array[1], Is.EqualTo("B"));
                Assert.That(array[2], Is.EqualTo("C"));
                Assert.That(array[3], Is.EqualTo("D"));
                Assert.That(array[4], Is.EqualTo("abc"));
            });
        }
    }
}
