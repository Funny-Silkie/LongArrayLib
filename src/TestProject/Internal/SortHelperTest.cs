using LongArrayLib;
using LongArrayLib.Internal;
using System;
using System.Collections.Generic;

namespace TestProject.Internal
{
    /// <summary>
    /// <see cref="SortHelper"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class SortHelperTest
    {
        private LongArray<int> shortSizeArray;
        private LongArray<int> longSizeArray;

        /// <summary>
        /// セットアップ処理を記述します。
        /// </summary>
        [SetUp]
        public void Setup()
        {
            List<int> source = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

            shortSizeArray = new LongArray<int>(30);
            LongArray.Fill(shortSizeArray, -1, 0L, 10L);
            for (int i = 10; i < 20; i++)
            {
                int index = Random.Shared.Next(source.Count);
                shortSizeArray[i] = source[index];
                source.RemoveAt(index);
            }
            LongArray.Fill(shortSizeArray, -1, 20L, 10L);

            longSizeArray = new LongArray<int>(100);
            for (long i = 0; i < longSizeArray.Length; i++) longSizeArray[i] = Random.Shared.Next();
        }

        /// <summary>
        /// イントロを検証します。
        /// </summary>
        [Test]
        [Repeat(30)]
        public void IntroSortTest()
        {
            SortHelper.IntroSort(longSizeArray, 0L, longSizeArray.Length, Comparer<int>.Default);

            Assert.Multiple(() =>
            {
                int prev = longSizeArray[0];
                for (long i = 1; i < longSizeArray.Length; i++)
                {
                    int current = longSizeArray[i];
                    Assert.That(current, Is.GreaterThanOrEqualTo(prev));
                    prev = current;
                }
            });
        }

        /// <summary>
        /// ヒープソートを検証します。
        /// </summary>
        [Test]
        [Repeat(30)]
        public void HeapSortTestShort()
        {
            SortHelper.HeapSort(shortSizeArray, 10L, 10L, Comparer<int>.Default);

            Assert.Multiple(() =>
            {
                for (long i = 0; i < 10; i++)
                {
                    Assert.That(shortSizeArray[i], Is.EqualTo(-1L));
                    Assert.That(shortSizeArray[i + 20], Is.EqualTo(-1L));
                }
                for (int i = 0; i < 10; i++) Assert.That(shortSizeArray[i + 10], Is.EqualTo(i));
            });
        }

        /// <summary>
        /// 挿入ソートを検証します。
        /// </summary>
        [Test]
        [Repeat(30)]
        public void InsertionSortTestShort()
        {
            SortHelper.InsertionSort(shortSizeArray, 10L, 10L, Comparer<int>.Default);

            Assert.Multiple(() =>
            {
                for (long i = 0; i < 10; i++)
                {
                    Assert.That(shortSizeArray[i], Is.EqualTo(-1L));
                    Assert.That(shortSizeArray[i + 20], Is.EqualTo(-1L));
                }
                for (int i = 0; i < 10; i++) Assert.That(shortSizeArray[i + 10], Is.EqualTo(i));
            });
        }
    }
}
