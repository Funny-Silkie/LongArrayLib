using LongArrayLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    public partial class LongArrayTest
    {
        /// <summary>
        /// <see cref="LongArray{T}.Contains(T)"/>を検証します。
        /// </summary>
        [Test]
        public void ContainsTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1, Does.Contain(0));
                Assert.That(array1, Does.Contain(-1));
                Assert.That(array1, Does.Contain(-2));
                Assert.That(array1, Does.Not.Contain(1));
                Assert.That(array1, Does.Not.Contain(-3));

                Assert.That(array2, Does.Contain("hoge"));
                Assert.That(array2, Does.Contain("fuga"));
                Assert.That(array2, Does.Contain("piyo"));
                Assert.That(array2, Does.Not.Contain(""));
                Assert.That(array2, Does.Not.Contain(null));
                Assert.That(array2, Does.Not.Contain("HOGE"));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.IndexOf(T)"/>を検証します。
        /// </summary>
        [Test]
        public void ForwardIndexOfTest1()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.IndexOf(0L), Is.EqualTo(0L));
                Assert.That(array1.IndexOf(-1L), Is.EqualTo(1L));
                Assert.That(array1.IndexOf(-2L), Is.EqualTo(2L));
                Assert.That(array1.IndexOf(1L), Is.EqualTo(-1L));
                Assert.That(array1.IndexOf(-3L), Is.EqualTo(-1L));

                Assert.That(array2.IndexOf("hoge"), Is.EqualTo(0L));
                Assert.That(array2.IndexOf("fuga"), Is.EqualTo(1L));
                Assert.That(array2.IndexOf("piyo"), Is.EqualTo(2L));
                Assert.That(array2.IndexOf(""), Is.EqualTo(-1L));
                Assert.That(array2.IndexOf(null!), Is.EqualTo(-1L));
                Assert.That(array2.IndexOf("HOGE"), Is.EqualTo(-1L));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.IndexOf(T, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ForwardIndexOfTest2()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.IndexOf("hoge", 0L), Is.EqualTo(0L));
                Assert.That(array2.IndexOf("hoge", 1L), Is.EqualTo(3L));
                Assert.That(array2.IndexOf("fuga", 2L), Is.EqualTo(-1L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array1.IndexOf(-2, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array1.IndexOf(-2, 3));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.IndexOf(T, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ForwardIndexOfTest3()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.IndexOf("hoge", 0, 4), Is.EqualTo(0L));
                Assert.That(array2.IndexOf("hoge", 1, 3), Is.EqualTo(3L));
                Assert.That(array2.IndexOf("hoge", 1, 2), Is.EqualTo(-1L));
                Assert.That(array2.IndexOf("hoge", 2, 2), Is.EqualTo(3L));
                Assert.That(array2.IndexOf("hoge", 0, 0), Is.EqualTo(-1L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array2.IndexOf("hoge", -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.IndexOf("hoge", 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.IndexOf("hoge", 2, 4));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.LastIndexOf(T)"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseIndexOfTest1()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.LastIndexOf(0L), Is.EqualTo(0L));
                Assert.That(array1.LastIndexOf(-1L), Is.EqualTo(1L));
                Assert.That(array1.LastIndexOf(-2L), Is.EqualTo(2L));
                Assert.That(array1.LastIndexOf(1L), Is.EqualTo(-1L));
                Assert.That(array1.LastIndexOf(-3L), Is.EqualTo(-1L));

                Assert.That(array2.LastIndexOf("hoge"), Is.EqualTo(3L));
                Assert.That(array2.LastIndexOf("fuga"), Is.EqualTo(1L));
                Assert.That(array2.LastIndexOf("piyo"), Is.EqualTo(2L));
                Assert.That(array2.LastIndexOf(""), Is.EqualTo(-1L));
                Assert.That(array2.LastIndexOf(null!), Is.EqualTo(-1L));
                Assert.That(array2.LastIndexOf("HOGE"), Is.EqualTo(-1L));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.LastIndexOf(T, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseIndexOfTest2()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.LastIndexOf("hoge", 2L), Is.EqualTo(0L));
                Assert.That(array2.LastIndexOf("hoge", 3L), Is.EqualTo(3L));
                Assert.That(array2.LastIndexOf("fuga", 0L), Is.EqualTo(-1L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array1.LastIndexOf(-2, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array1.LastIndexOf(-2, 3));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.LastIndexOf(T, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseIndexOfTest3()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.LastIndexOf("hoge", 3, 4), Is.EqualTo(3L));
                Assert.That(array2.LastIndexOf("hoge", 2, 3), Is.EqualTo(0L));
                Assert.That(array2.LastIndexOf("hoge", 2, 2), Is.EqualTo(-1L));
                Assert.That(array2.LastIndexOf("hoge", 1, 2), Is.EqualTo(0L));
                Assert.That(array2.LastIndexOf("hoge", 0, 0), Is.EqualTo(-1L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array2.LastIndexOf("hoge", -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.LastIndexOf("hoge", 4, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.IndexOf("hoge", 2, 4));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.FindIndex(Predicate{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ForwardFindIndexTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.FindIndex(x => x == 0L), Is.EqualTo(0L));
                Assert.That(array1.FindIndex(x => x == -1L), Is.EqualTo(1L));
                Assert.That(array1.FindIndex(x => x == -2L), Is.EqualTo(2L));
                Assert.That(array1.FindIndex(x => x == 1L), Is.EqualTo(-1L));
                Assert.That(array1.FindIndex(x => x == -3L), Is.EqualTo(-1L));

                Assert.That(array2.FindIndex(x => x == "hoge"), Is.EqualTo(0L));
                Assert.That(array2.FindIndex(x => x == "fuga"), Is.EqualTo(1L));
                Assert.That(array2.FindIndex(x => x == "piyo"), Is.EqualTo(2L));
                Assert.That(array2.FindIndex(x => x == ""), Is.EqualTo(-1L));
                Assert.That(array2.FindIndex(x => x == null!), Is.EqualTo(-1L));
                Assert.That(array2.FindIndex(x => x == "HOGE"), Is.EqualTo(-1L));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.FindIndex(Predicate{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ForwardFindIndexTest2()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.FindIndex(x => x == "hoge", 0L), Is.EqualTo(0L));
                Assert.That(array2.FindIndex(x => x == "hoge", 1L), Is.EqualTo(3L));
                Assert.That(array2.FindIndex(x => x == "fuga", 2L), Is.EqualTo(-1L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array1.FindIndex(x => x == -2, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array1.FindIndex(x => x == -2, 3));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.FindIndex(Predicate{T}, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ForwardFindIndexTest3()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.FindIndex(x => x == "hoge", 0, 4), Is.EqualTo(0L));
                Assert.That(array2.FindIndex(x => x == "hoge", 1, 3), Is.EqualTo(3L));
                Assert.That(array2.FindIndex(x => x == "hoge", 1, 2), Is.EqualTo(-1L));
                Assert.That(array2.FindIndex(x => x == "hoge", 2, 2), Is.EqualTo(3L));
                Assert.That(array2.FindIndex(x => x == "hoge", 0, 0), Is.EqualTo(-1L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array2.FindIndex(x => x == "hoge", -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.FindIndex(x => x == "hoge", 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.FindIndex(x => x == "hoge", 2, 4));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.FindLastIndex(Predicate{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseLastIndexTest1()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.FindLastIndex(x => x == 0L), Is.EqualTo(0L));
                Assert.That(array1.FindLastIndex(x => x == -1L), Is.EqualTo(1L));
                Assert.That(array1.FindLastIndex(x => x == -2L), Is.EqualTo(2L));
                Assert.That(array1.FindLastIndex(x => x == 1L), Is.EqualTo(-1L));
                Assert.That(array1.FindLastIndex(x => x == -3L), Is.EqualTo(-1L));

                Assert.That(array2.FindLastIndex(x => x == "hoge"), Is.EqualTo(3L));
                Assert.That(array2.FindLastIndex(x => x == "fuga"), Is.EqualTo(1L));
                Assert.That(array2.FindLastIndex(x => x == "piyo"), Is.EqualTo(2L));
                Assert.That(array2.FindLastIndex(x => x == ""), Is.EqualTo(-1L));
                Assert.That(array2.FindLastIndex(x => x == null!), Is.EqualTo(-1L));
                Assert.That(array2.FindLastIndex(x => x == "HOGE"), Is.EqualTo(-1L));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.FindLastIndex(Predicate{T}, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseLastIndexTest2()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.FindLastIndex(x => x == "hoge", 2L), Is.EqualTo(0L));
                Assert.That(array2.FindLastIndex(x => x == "hoge", 3L), Is.EqualTo(3L));
                Assert.That(array2.FindLastIndex(x => x == "fuga", 0L), Is.EqualTo(-1L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array1.FindLastIndex(x => x == -2, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array1.FindLastIndex(x => x == -2, 3));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.FindLastIndex(Predicate{T}, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseLastIndexTest3()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array2.FindLastIndex(x => x == "hoge", 3, 4), Is.EqualTo(3L));
                Assert.That(array2.FindLastIndex(x => x == "hoge", 2, 3), Is.EqualTo(0L));
                Assert.That(array2.FindLastIndex(x => x == "hoge", 2, 2), Is.EqualTo(-1L));
                Assert.That(array2.FindLastIndex(x => x == "hoge", 1, 2), Is.EqualTo(0L));
                Assert.That(array2.FindLastIndex(x => x == "hoge", 0, 0), Is.EqualTo(-1L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array2.FindLastIndex(x => x == "hoge", -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.FindLastIndex(x => x == "hoge", 4, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array2.IndexOf("hoge", 2, 4));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.Find(Predicate{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ForwardFindElementTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.Find(x => x < 0), Is.EqualTo(-1L));
                Assert.That(array1.Find(x => Math.Abs(x) > 10), Is.EqualTo(default(long)));

                Assert.That(array2.Find(x => x[0] is 'f' or 'p'), Is.EqualTo("fuga"));
                Assert.That(array2.Find(x => x.Length > 4), Is.EqualTo(default(string)));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.FindLast(Predicate{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseFindElementTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.FindLast(x => x < 0), Is.EqualTo(-2L));
                Assert.That(array1.FindLast(x => Math.Abs(x) > 10), Is.EqualTo(default(long)));

                Assert.That(array2.FindLast(x => x[0] is 'f' or 'p'), Is.EqualTo("piyo"));
                Assert.That(array2.FindLast(x => x.Length > 4), Is.EqualTo(default(string)));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.FindAll(Predicate{T})"/>を検証します。
        /// </summary>
        [Test]
        public void FindAllTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.FindAll(x => x <= 0).SequenceEqual(array1), Is.True);
                Assert.That(array1.FindAll(x => x < 0).SequenceEqual(new[] { -1L, -2L }), Is.True);
                Assert.That(array1.FindAll(x => x > 0), Is.Empty);
                Assert.That(array2.FindAll(x => x[2] == 'g').SequenceEqual(new[] { "hoge", "fuga", "hoge" }), Is.True);
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.Exists(Predicate{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ExistsTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.Exists(x => x <= 0), Is.True);
                Assert.That(array1.Exists(x => x < 0), Is.True);
                Assert.That(array1.Exists(x => x > 0), Is.False);

                Assert.That(array2.Exists(x => x.Length == 4), Is.True);
                Assert.That(array2.Exists(x => x == "hoge"), Is.True);
                Assert.That(array2.Exists(x => char.IsUpper(x[0])), Is.False);
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.TrueForAll(Predicate{T})"/>を検証します。
        /// </summary>
        [Test]
        public void TrueForAllTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(array1.TrueForAll(x => x <= 0), Is.True);
                Assert.That(array1.TrueForAll(x => x < 0), Is.False);
                Assert.That(array1.TrueForAll(x => x > 0), Is.False);

                Assert.That(array2.TrueForAll(x => x.Length == 4), Is.True);
                Assert.That(array2.TrueForAll(x => x == "hoge"), Is.False);
                Assert.That(array2.TrueForAll(x => char.IsUpper(x[0])), Is.False);
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.BinarySearch(T)"/>を検証します。
        /// </summary>
        [Test]
        public void BinarySearchTest1()
        {
            LongArray<int> array = LongArray.Create([1, 2, 4, 7, 8, 10]);

            Assert.Multiple(() =>
            {
                Assert.That(array.BinarySearch(2), Is.EqualTo(1));
                Assert.That(array.BinarySearch(8), Is.EqualTo(4));
                Assert.That(array.BinarySearch(3), Is.EqualTo(~2));
                Assert.That(array.BinarySearch(-1), Is.EqualTo(~0));
                Assert.That(array.BinarySearch(11), Is.EqualTo(~6));

                Assert.Throws<InvalidOperationException>(() => LongArray.Create(new object(), new object()).BinarySearch(new object()));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.BinarySearch(long, long, T)"/>を検証します。
        /// </summary>
        [Test]
        public void BinarySearchTest2()
        {
            LongArray<int> array = LongArray.Create([1, 2, 4, 7, 8, 10]);

            Assert.Multiple(() =>
            {
                Assert.That(array.BinarySearch(0L, 6L, 2), Is.EqualTo(1));
                Assert.That(array.BinarySearch(0L, 6L, 8), Is.EqualTo(4));
                Assert.That(array.BinarySearch(0L, 6L, 3), Is.EqualTo(~2));
                Assert.That(array.BinarySearch(0L, 6L, -1), Is.EqualTo(~0));
                Assert.That(array.BinarySearch(0L, 6L, 11), Is.EqualTo(~6));
                Assert.That(array.BinarySearch(1L, 3L, 4), Is.EqualTo(2L));
                Assert.That(array.BinarySearch(1L, 3L, 1), Is.EqualTo(~1L));
                Assert.That(array.BinarySearch(1L, 3L, 10), Is.EqualTo(~4L));
                Assert.That(array.BinarySearch(3L, 0L, 4), Is.EqualTo(~3L));
                Assert.That(array.BinarySearch(2L, 2L, 3), Is.EqualTo(~2L));
                Assert.That(array.BinarySearch(2L, 2L, 5), Is.EqualTo(~3L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(-1L, 0L, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(6L, 0L, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(0L, -1L, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(0L, 7L, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(1L, 6L, 1));
                Assert.Throws<InvalidOperationException>(() => LongArray.Create(new object(), new object(), new object(), new object(), new object(), new object()).BinarySearch(0L, 6L, new object()));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.BinarySearch(T, IComparer{T}?)"/>を検証します。
        /// </summary>
        [Test]
        public void BinarySearchTest3()
        {
            LongArray<int> array = LongArray.Create([1, 2, 4, 7, 8, 10]);

            Assert.Multiple(() =>
            {
                Assert.That(array.BinarySearch(2, null), Is.EqualTo(1));
                Assert.That(array.BinarySearch(8, Comparer<int>.Default), Is.EqualTo(4));
                Assert.That(array.BinarySearch(3, Comparer<int>.Default), Is.EqualTo(~2));
                Assert.That(array.BinarySearch(-1, Comparer<int>.Default), Is.EqualTo(~0));
                Assert.That(array.BinarySearch(11, Comparer<int>.Default), Is.EqualTo(~6));

                Assert.Throws<InvalidOperationException>(() => LongArray.Create(new object(), new object()).BinarySearch(new object(), null));
            });
        }

        /// <summary>
        /// <see cref="LongArray{T}.BinarySearch(long, long, T, IComparer{T}?)"/>を検証します。
        /// </summary>
        [Test]
        public void BinarySearchTest4()
        {
            LongArray<int> array = LongArray.Create([1, 2, 4, 7, 8, 10]);

            Assert.Multiple(() =>
            {
                Assert.That(array.BinarySearch(0L, 6L, 2, null), Is.EqualTo(1));
                Assert.That(array.BinarySearch(0L, 6L, 8, Comparer<int>.Default), Is.EqualTo(4));
                Assert.That(array.BinarySearch(0L, 6L, 3, Comparer<int>.Default), Is.EqualTo(~2));
                Assert.That(array.BinarySearch(0L, 6L, -1, Comparer<int>.Default), Is.EqualTo(~0));
                Assert.That(array.BinarySearch(0L, 6L, 11, Comparer<int>.Default), Is.EqualTo(~6));
                Assert.That(array.BinarySearch(1L, 3L, 4, Comparer<int>.Default), Is.EqualTo(2L));
                Assert.That(array.BinarySearch(1L, 3L, 1, Comparer<int>.Default), Is.EqualTo(~1L));
                Assert.That(array.BinarySearch(1L, 3L, 10, Comparer<int>.Default), Is.EqualTo(~4L));
                Assert.That(array.BinarySearch(3L, 0L, 4, Comparer<int>.Default), Is.EqualTo(~3L));
                Assert.That(array.BinarySearch(2L, 2L, 3, Comparer<int>.Default), Is.EqualTo(~2L));
                Assert.That(array.BinarySearch(2L, 2L, 5, Comparer<int>.Default), Is.EqualTo(~3L));

                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(-1L, 0L, 1, Comparer<int>.Default));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(6L, 0L, 1, Comparer<int>.Default));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(0L, -1L, 1, Comparer<int>.Default));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(0L, 7L, 1, Comparer<int>.Default));
                Assert.Throws<ArgumentOutOfRangeException>(() => array.BinarySearch(1L, 6L, 1, Comparer<int>.Default));
                Assert.Throws<InvalidOperationException>(() => LongArray.Create(new object(), new object(), new object(), new object(), new object(), new object()).BinarySearch(0L, 6L, new object(), null));
            });
        }
    }
}
