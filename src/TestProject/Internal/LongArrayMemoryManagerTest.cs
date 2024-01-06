using LongArrayLib;
using LongArrayLib.Internal;
using System;
using System.Buffers;

namespace TestProject.Internal
{
    /// <summary>
    /// <see cref="LongArrayMemoryManager{T}"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class LongArrayMemoryManagerTest
    {
        private LongArrayMemoryManager<string> manager;

        /// <summary>
        /// セットアップ処理を記述します。
        /// </summary>
        [SetUp]
        public void Setup()
        {
            manager = new LongArrayMemoryManager<string>(LongArray.Create(["hoge", "fuga", "piyo", "hoge"]), 1, 3);
        }

        /// <summary>
        /// <see cref="LongArrayMemoryManager{T}.GetSpan"/>を検証します。
        /// </summary>
        [Test]
        public void GetMemory()
        {
            Memory<string> memory = manager.Memory;

            Assert.Multiple(() =>
            {
                Assert.That(memory.Length, Is.EqualTo(3));
                Assert.That(memory.Span[0], Is.EqualTo("fuga"));
                Assert.That(memory.Span[1], Is.EqualTo("piyo"));
                Assert.That(memory.Span[2], Is.EqualTo("hoge"));
            });
        }

#pragma warning disable NUnit2045 // Use Assert.Multiple

        /// <summary>
        /// <see cref="MemoryManager{T}.Memory"/>を検証します。
        /// </summary>
        [Test]
        public void GetSpan()
        {
            Span<string> span = manager.GetSpan();

            Assert.That(span.Length, Is.EqualTo(3));
            Assert.That(span[0], Is.EqualTo("fuga"));
            Assert.That(span[1], Is.EqualTo("piyo"));
            Assert.That(span[2], Is.EqualTo("hoge"));
        }

#pragma warning restore NUnit2045 // Use Assert.Multiple
    }
}
