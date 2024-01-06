using LongArrayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Utils;

namespace TestProject
{
    public partial class LongArrayTest
    {
        /// <summary>
        /// <see cref="LongArray.Sort{T}(LongArray{T})"/>を検証します。
        /// </summary>
        [Test]
        public void SortTest1()
        {
            LongArray<string> array = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
            LongArray.Sort(array);

            Assert.That(array.SequenceEqual(["a", "b", "c", "d", "e", "f"]));

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort<int>(null!));
                Assert.Throws<InvalidOperationException>(() => LongArray.Sort(LongArray.Create(new object(), new object())));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Sort{T}(LongArray{T}, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void SortTest2()
        {
            LongArray<string> array = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
            LongArray.Sort(array, 2, 3);

            Assert.That(array.SequenceEqual(["d", "a", "c", "e", "f", "b"]));

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort<int>(null!, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, 3, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, 0, -1));
                Assert.Throws<ArgumentException>(() => LongArray.Sort(array1, 1, 3));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Sort{T}(LongArray{T}, IComparer{T}?)"/>を検証します。
        /// </summary>
        [Test]
        public void SortTest3()
        {
            // with exlicit comparer
            {
                LongArray<string> array = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray.Sort(array, Comparer<string>.Default);

                Assert.That(array.SequenceEqual(["a", "b", "c", "d", "e", "f"]));
            }

            // witu default comparer
            {
                LongArray<string> array = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray.Sort(array, comparer: null);
                Assert.That(array.SequenceEqual(["a", "b", "c", "d", "e", "f"]));
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort(null!, Comparer<int>.Default));
                Assert.Throws<InvalidOperationException>(() => LongArray.Sort(LongArray.Create(new object(), new object()), comparer: null));
                Assert.Throws<ArgumentException>(() => LongArray.Sort(new LongArray<int>(100), new CrunkComparer<int>()));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Sort{T}(LongArray{T}, long, long, IComparer{T}?)"/>を検証します。
        /// </summary>
        [Test]
        public void SortTest4()
        {
            // with exlicit comparer
            {
                LongArray<string> array = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray.Sort(array, 2, 3, Comparer<string>.Default);

                Assert.That(array.SequenceEqual(["d", "a", "c", "e", "f", "b"]));
            }

            // witu default comparer
            {
                LongArray<string> array = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray.Sort(array, 2, 3, null);

                Assert.That(array.SequenceEqual(["d", "a", "c", "e", "f", "b"]));
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort(null!, 0, 0, Comparer<int>.Default));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, -1, 0, Comparer<long>.Default));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, 3, 0, Comparer<long>.Default));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, 0, -1, Comparer<long>.Default));
                Assert.Throws<ArgumentException>(() => LongArray.Sort(array1, 1, 3, Comparer<long>.Default));
                Assert.Throws<InvalidOperationException>(() => LongArray.Sort(LongArray.Create(new object(), new object()), 0, 2, null));
                Assert.Throws<ArgumentException>(() => LongArray.Sort(new LongArray<int>(100), 0, 100, new CrunkComparer<int>()));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Sort{T}(LongArray{T}, Comparison{T})"/>を検証します。
        /// </summary>
        [Test]
        public void SortWithDelegateTest()
        {
            LongArray<string> array = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
            LongArray.Sort(array, (x, y) => y.CompareTo(x));

            Assert.That(array.SequenceEqual(["f", "e", "d", "c", "b", "a"]));

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort<int>(null!, (x, y) => x.CompareTo(y)));
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort(array1, comparison: null!));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Sort{TKey, TValue}(LongArray{TKey}, LongArray{TValue}?)"/>を検証します。
        /// </summary>
        [Test]
        public void SortByKeyTest1()
        {
            // with value array
            {
                LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray<int> values = LongArray.Create([0, 1, 2, 3, 4, 5]);
                LongArray.Sort(keys, values);

                Assert.Multiple(() =>
                {
                    Assert.That(keys.SequenceEqual(["a", "b", "c", "d", "e", "f"]));
                    Assert.That(values.SequenceEqual([1, 5, 3, 0, 2, 4]));
                });
            }

            // withot value array
            {
                LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray.Sort(keys, (LongArray<int>?)null);

                Assert.That(keys.SequenceEqual(["a", "b", "c", "d", "e", "f"]));
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort((LongArray<int>)null!, array1));
                Assert.Throws<InvalidOperationException>(() => LongArray.Sort(LongArray.Create(new object(), new object()), array2));
                Assert.Throws<ArgumentException>(() => LongArray.Sort(new LongArray<int>(100), array2, new CrunkComparer<int>()));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Sort{TKey, TValue}(LongArray{TKey}, LongArray{TValue}?, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void SortByKeyTest2()
        {
            // with value array
            {
                LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray<int> values = LongArray.Create([0, 1, 2, 3, 4, 5]);
                LongArray.Sort(keys, values, 2, 3);

                Assert.Multiple(() =>
                {
                    Assert.That(keys.SequenceEqual(["d", "a", "c", "e", "f", "b"]));
                    Assert.That(values.SequenceEqual([0, 1, 3, 2, 4, 5]));
                });
            }

            // without value array
            {
                LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray.Sort(keys, (LongArray<int>?)null, 2, 3);

                Assert.That(keys.SequenceEqual(["d", "a", "c", "e", "f", "b"]));
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort((LongArray<int>)null!, array1, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, array2, -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, array2, 4, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, array2, 0, -1));
                Assert.Throws<ArgumentException>(() => LongArray.Sort(array1, array2, 1, 3));
                Assert.Throws<InvalidOperationException>(() => LongArray.Sort(LongArray.Create(new object(), new object()), array2, 0, 2));
                Assert.Throws<ArgumentException>(() => LongArray.Sort(new LongArray<int>(100), new LongArray<string>(100), 0, 100, new CrunkComparer<int>()));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Sort{TKey, TValue}(LongArray{TKey}, LongArray{TValue}?, IComparer{TKey}?)"/>を検証します。
        /// </summary>
        [Test]
        public void SortByKeyTest3()
        {
            // with value array
            {
                // with explicit comparer
                {
                    LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                    LongArray<int> values = LongArray.Create([0, 1, 2, 3, 4, 5]);
                    LongArray.Sort(keys, values, Comparer<string>.Default);

                    Assert.Multiple(() =>
                    {
                        Assert.That(keys.SequenceEqual(["a", "b", "c", "d", "e", "f"]));
                        Assert.That(values.SequenceEqual([1, 5, 3, 0, 2, 4]));
                    });
                }

                // with default comparer
                {
                    LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                    LongArray<int> values = LongArray.Create([0, 1, 2, 3, 4, 5]);
                    LongArray.Sort(keys, values, null);

                    Assert.Multiple(() =>
                    {
                        Assert.That(keys.SequenceEqual(["a", "b", "c", "d", "e", "f"]));
                        Assert.That(values.SequenceEqual([1, 5, 3, 0, 2, 4]));
                    });
                }
            }

            // without value array
            {
                LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray.Sort(keys, (LongArray<int>?)null, Comparer<string>.Default);

                Assert.That(keys.SequenceEqual(["a", "b", "c", "d", "e", "f"]));
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort(null!, array1, Comparer<int>.Default));
                Assert.Throws<InvalidOperationException>(() => LongArray.Sort(LongArray.Create(new object(), new object()), array2, null));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Sort{TKey, TValue}(LongArray{TKey}, LongArray{TValue}?, long, long, IComparer{TKey}?)"/>を検証します。
        /// </summary>
        [Test]
        public void SortByKeyTest4()
        {
            // with value array
            {
                // with explicit comparer
                {
                    LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                    LongArray<int> values = LongArray.Create([0, 1, 2, 3, 4, 5]);
                    LongArray.Sort(keys, values, 2, 3, Comparer<string>.Default);

                    Assert.Multiple(() =>
                    {
                        Assert.That(keys.SequenceEqual(["d", "a", "c", "e", "f", "b"]));
                        Assert.That(values.SequenceEqual([0, 1, 3, 2, 4, 5]));
                    });
                }

                // with default comparer
                {
                    LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                    LongArray<int> values = LongArray.Create([0, 1, 2, 3, 4, 5]);
                    LongArray.Sort(keys, values, 2, 3, null);

                    Assert.Multiple(() =>
                    {
                        Assert.That(keys.SequenceEqual(["d", "a", "c", "e", "f", "b"]));
                        Assert.That(values.SequenceEqual([0, 1, 3, 2, 4, 5]));
                    });
                }
            }

            // without value array
            {
                LongArray<string> keys = LongArray.Create(["d", "a", "e", "c", "f", "b"]);
                LongArray.Sort(keys, (LongArray<int>?)null, 2, 3, Comparer<string>.Default);

                Assert.That(keys.SequenceEqual(["d", "a", "c", "e", "f", "b"]));
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Sort(null!, array1, 0, 0, Comparer<int>.Default));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, array2, -1, 0, null));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, array2, 4, 0, null));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Sort(array1, array2, 0, -1, null));
                Assert.Throws<ArgumentException>(() => LongArray.Sort(array1, array2, 1, 3, null));
                Assert.Throws<InvalidOperationException>(() => LongArray.Sort(LongArray.Create(new object(), new object(), new object()), array2, 1, 2, null));
            });
        }

        /// <summary>
        /// <see cref="LongArray.Reverse{T}(LongArray{T})"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseTest1()
        {
            {
                var array = (LongArray<byte>)array3.Clone();
                LongArray.Reverse(array);

                Assert.Multiple(() =>
                {
                    for (long i = 0; i < array.Length; i++) Assert.That(array[i], Is.EqualTo((byte)(19 - i)));
                });
            }
            Assert.Throws<ArgumentNullException>(() => LongArray.Reverse<int>(null!));
        }

        /// <summary>
        /// <see cref="LongArray.Reverse{T}(LongArray{T}, long, long)"/>を検証します。
        /// </summary>
        [Test]
        public void ReverseTest2()
        {
            // non-empty range
            {
                var array = (LongArray<string>)array2.Clone();
                LongArray.Reverse(array, 1, 3);

                Assert.Multiple(() =>
                {
                    Assert.That(array[0], Is.EqualTo("hoge"));
                    Assert.That(array[1], Is.EqualTo("hoge"));
                    Assert.That(array[2], Is.EqualTo("piyo"));
                    Assert.That(array[3], Is.EqualTo("fuga"));
                });
            }

            // empty range
            {
                var array = (LongArray<string>)array2.Clone();
                LongArray.Reverse(array, 1, 0);

                Assert.That(array.SequenceEqual(array2), Is.True);
            }

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => LongArray.Reverse<int>(null!, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Reverse(array1, -1, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Reverse(array1, 5, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => LongArray.Reverse(array1, 0, -1));
                Assert.Throws<ArgumentException>(() => LongArray.Reverse(array1, 1, 3));
            });
        }
    }
}
