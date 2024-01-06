using LongArrayLib;
using LongArrayLib.Internal;
using System;

namespace TestProject.Internal
{
    /// <summary>
    /// <see cref="LongArrayBuilder{T}"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class ArrayBuilderTest
    {
        /// <summary>
        /// 空の配列の生成を検証します。
        /// </summary>
        [Test]
        public void BuildEmptyArray()
        {
            using var builder = new LongArrayBuilder<int>(0);
            LongArray<int> array = builder.ToArray();

            Assert.That(array, Is.Empty);
        }

        /// <summary>
        /// 短いサイズの配列の生成を検証します。
        /// </summary>
        [Test]
        public void BuildShortSizeArray()
        {
            using var builder = new LongArrayBuilder<int>(10);
            for (int i = 0; i < 10; i++) builder.Add(i);

            LongArray<int> array = builder.ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(10));
                for (int i = 0; i < 10; i++) Assert.That(array[i], Is.EqualTo(i));
            });
        }

        /// <summary>
        /// 長いサイズの配列の生成を検証します。
        /// </summary>
        [Test]
        public void BuildLongSizeArray()
        {
            using var builder = new LongArrayBuilder<int>(10);
            for (int i = 0; i < 1000; i++) builder.Add(i);

            LongArray<int> array = builder.ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(array, Has.Length.EqualTo(1000));
                for (int i = 0; i < 1000; i++) Assert.That(array[i], Is.EqualTo(i));
            });
        }

        /// <summary>
        /// 破棄を検証します。
        /// </summary>
        [Test]
        public void DisposeTest()
        {
            var builder = new LongArrayBuilder<string>(3);
            builder.Add("hoge");

            builder.Dispose();

            Assert.Multiple(() =>
            {
                Assert.Throws<ObjectDisposedException>(() => builder.Add("fuga"));
                Assert.Throws<ObjectDisposedException>(() => builder.ToArray());
            });
        }
    }
}
