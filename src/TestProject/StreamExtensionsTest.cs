using LongArrayLib;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using TestProject.Utils;

namespace TestProject
{
    /// <summary>
    /// <see cref="StreamExtensions"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class StreamExtensionsTest
    {
        private byte[] data;
        private Stream stream;

        /// <summary>
        /// セットアップ処理を記述します。
        /// </summary>
        [OneTimeSetUp]
        public void SetupOnce()
        {
            data = Enumerable.Range(0, 100).Select(x => (byte)x).ToArray();
        }

        /// <summary>
        /// セットアップ処理を記述します。
        /// </summary>
        [SetUp]
        public void Setup()
        {
            stream = new TestStream();
        }

        /// <summary>
        /// データを書き込みます。
        /// </summary>
        private void WriteData()
        {
            stream.Write(data);
            stream.Flush();
        }

        /// <summary>
        /// クリーンアップ処理を記述します。
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            stream.Dispose();
        }

        /// <summary>
        /// <see cref="StreamExtensions.Read(Stream, LongArray{byte}, long, long, int)"/>を検証します。
        /// </summary>
        [Test]
        public void ReadTest()
        {
            WriteData();

            using (var buffer = new LongArray<byte>(50L))
            {
                stream.Position = 0L;

                long readBytes = stream.Read(buffer, 0L, 50L);
                Assert.Multiple(() =>
                {
                    Assert.That(readBytes, Is.EqualTo(50L));
                    Assert.That(buffer.SequenceEqual(data.Take(50)), Is.True);
                });

                readBytes = stream.Read(buffer, 0L, 50L, 5);
                Assert.Multiple(() =>
                {
                    Assert.That(readBytes, Is.EqualTo(50L));
                    Assert.That(buffer.SequenceEqual(data.Skip(50).Take(50)), Is.True);
                });
            };

            using (var buffer = new LongArray<byte>(stream.Length + 1))
            {
                stream.Position = 0L;

                long readBytes = stream.Read(buffer, 0L, buffer.Length);

                Assert.Multiple(() =>
                {
                    Assert.That(readBytes, Is.EqualTo(stream.Length));
                    Assert.That(readBytes, Is.LessThan(buffer.Length));
                    Assert.That(buffer.Take(100).SequenceEqual(data), Is.True);
                });
            }

            Assert.Multiple(() =>
            {
                using var buffer = new LongArray<byte>(50L);

                Assert.Throws<ArgumentNullException>(() => StreamExtensions.Read(null!, new LongArray<byte>(1L), 0L, 1L));
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.Read(stream, null!, 0L, 1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(buffer, -1L, 1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(buffer, 0L, -1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(buffer, 0L, 1L, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(buffer, 0L, 1L, 0));
                Assert.Throws<ArgumentException>(() => stream.Read(buffer, 30L, 30L));
                Assert.Throws<NotSupportedException>(() => new TestStream(FileAccess.Write).Read(buffer, 0L, 50L));
                Assert.Throws<ObjectDisposedException>(() =>
                {
                    using var str = new TestStream();
                    str.Dispose();
                    str.Read(buffer, 0L, 5L);
                });
            });
        }

        /// <summary>
        /// <see cref="StreamExtensions.ReadAsync(Stream, LongArray{byte}, long, long, int, CancellationToken)"/>を検証します。
        /// </summary>
        [Test]
        public void ReadAsyncTest()
        {
            WriteData();
            using (var buffer = new LongArray<byte>(50L))
            {
                stream.Position = 0L;

                long readBytes = stream.ReadAsync(buffer, 0L, 50L).Result;
                Assert.Multiple(() =>
                {
                    Assert.That(readBytes, Is.EqualTo(50L));
                    Assert.That(buffer.SequenceEqual(data.Take(50)), Is.True);
                });

                readBytes = stream.ReadAsync(buffer, 0L, 50L).Result;
                Assert.Multiple(() =>
                {
                    Assert.That(readBytes, Is.EqualTo(50L));
                    Assert.That(buffer.SequenceEqual(data.Skip(50).Take(50)), Is.True);
                });
            }

            using (var buffer = new LongArray<byte>(stream.Length + 1))
            {
                stream.Position = 0L;

                long readBytes = stream.ReadAsync(buffer, 0L, buffer.Length).Result;

                Assert.Multiple(() =>
                {
                    Assert.That(readBytes, Is.EqualTo(stream.Length));
                    Assert.That(readBytes, Is.LessThan(buffer.Length));
                    Assert.That(buffer.Take(100).SequenceEqual(data), Is.True);
                });
            }

            Assert.Multiple(() =>
            {
                using var buffer = new LongArray<byte>(50L);

                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadAsync(null!, new LongArray<byte>(1L), 0L, 1L).Wait());
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadAsync(stream, null!, 0L, 1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(buffer, -1L, 1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(buffer, 0L, -1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(buffer, 0L, 1L, 0).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(buffer, 0L, 1L, -1).Wait());
                Assert.Throws<ArgumentException>(() => stream.ReadAsync(buffer, 30L, 30L).Wait());
                TestHelper.AggregateThrows<NotSupportedException>(() => new TestStream(FileAccess.Write).ReadAsync(buffer, 0L, 50L).Wait());
                TestHelper.AggregateThrows<ObjectDisposedException>(() =>
                {
                    using var str = new TestStream();
                    str.Dispose();
                    str.ReadAsync(buffer, 0L, 5L).Wait();
                });
                //TestHelper.AggregateThrows<TaskCanceledException>(() =>
                //{
                //    using var stream = new TestStream(int.MaxValue);
                //    using var buffer = new LongArray<byte>(int.MaxValue);
                //    using var tokenSource = new CancellationTokenSource();
                //    tokenSource.CancelAfter(100);
                //    Task<long> task = stream.ReadAsync(buffer, 0L, buffer.Length, 1, tokenSource.Token);
                //    task.Wait();
                //});
            });
        }

        /// <summary>
        /// <see cref="StreamExtensions.ReadExactly(Stream, LongArray{byte}, long, long, int)"/>を検証します。
        /// </summary>
        [Test]
        public void ReadExactlyTest()
        {
            WriteData();

            using (var buffer = new LongArray<byte>(50L))
            {
                stream.Position = 0L;

                stream.ReadExactly(buffer, 0L, 50L);
                Assert.That(buffer.SequenceEqual(data.Take(50)), Is.True);

                stream.ReadExactly(buffer, 0L, 50L, 5);
                Assert.That(buffer.SequenceEqual(data.Skip(50).Take(50)), Is.True);
            };

            Assert.Multiple(() =>
            {
                using var buffer = new LongArray<byte>(50L);

                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadExactly(null!, new LongArray<byte>(1L), 0L, 1L));
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadExactly(stream, null!, 0L, 1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadExactly(buffer, -1L, 1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadExactly(buffer, 0L, -1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadExactly(buffer, 0L, 1L, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadExactly(buffer, 0L, 1L, 0));
                Assert.Throws<ArgumentException>(() => stream.ReadExactly(buffer, 30L, 30L));
                Assert.Throws<NotSupportedException>(() => new TestStream(FileAccess.Write).ReadExactly(buffer, 0L, 50L));
                Assert.Throws<ObjectDisposedException>(() =>
                {
                    using var str = new TestStream();
                    str.Dispose();
                    str.ReadExactly(buffer, 0L, 5L);
                });
            });

            using (var buffer = new LongArray<byte>(stream.Length + 1))
            {
                stream.Position = 0L;

                TestHelper.AggregateThrows<EndOfStreamException>(() => stream.ReadExactly(buffer, 0L, buffer.Length));
            }
        }

        /// <summary>
        /// <see cref="StreamExtensions.ReadExactlyAsync(Stream, LongArray{byte}, long, long, int, CancellationToken)"/>を検証します。
        /// </summary>
        [Test]
        public void ReadExactlyAsyncTest()
        {
            WriteData();
            using (var buffer = new LongArray<byte>(50L))
            {
                stream.Position = 0L;

                stream.ReadExactlyAsync(buffer, 0L, 50L).Wait();
                Assert.That(buffer.SequenceEqual(data.Take(50)), Is.True);

                stream.ReadExactlyAsync(buffer, 0L, 50L).Wait();
                Assert.That(buffer.SequenceEqual(data.Skip(50).Take(50)), Is.True);
            }

            Assert.Multiple(() =>
            {
                using var buffer = new LongArray<byte>(50L);

                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadExactlyAsync(null!, new LongArray<byte>(1L), 0L, 1L).Wait());
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadExactlyAsync(stream, null!, 0L, 1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadExactlyAsync(buffer, -1L, 1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadExactlyAsync(buffer, 0L, -1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadExactlyAsync(buffer, 0L, 1L, 0).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadExactlyAsync(buffer, 0L, 1L, -1).Wait());
                Assert.Throws<ArgumentException>(() => stream.ReadExactlyAsync(buffer, 30L, 30L).Wait());
                TestHelper.AggregateThrows<NotSupportedException>(() => new TestStream(FileAccess.Write).ReadExactlyAsync(buffer, 0L, 50L).Wait());
                TestHelper.AggregateThrows<ObjectDisposedException>(() =>
                {
                    using var str = new TestStream();
                    str.Dispose();
                    str.ReadExactlyAsync(buffer, 0L, 5L).Wait();
                });
                //TestHelper.AggregateThrows<TaskCanceledException>(() =>
                //{
                //    using var stream = new TestStream(int.MaxValue);
                //    using var buffer = new LongArray<byte>(int.MaxValue);
                //    using var tokenSource = new CancellationTokenSource();
                //    tokenSource.CancelAfter(100);
                //    Task task = stream.ReadExactlyAsync(buffer, 0L, buffer.Length, 1, tokenSource.Token);
                //    task.Wait();
                //});
            });

            using (var buffer = new LongArray<byte>(stream.Length + 1))
            {
                stream.Position = 0L;

                TestHelper.AggregateThrows<EndOfStreamException>(() => stream.ReadExactlyAsync(buffer, 0L, buffer.Length).Wait());
            }
        }

        /// <summary>
        /// <see cref="StreamExtensions.Write(Stream, LongArray{byte}, long, long, int)"/>を検証します。
        /// </summary>
        [Test]
        public void WriteTest()
        {
            using LongArray<byte> writtenData = LongArray.Create(data);

            stream.Write(writtenData, 0L, 50L);
            {
                stream.Position = 0L;
                var buffer = new byte[50];
                stream.Read(buffer);
                Assert.That(buffer.SequenceEqual(data.Take(50)), Is.True);
            }

            stream.Write(writtenData, 50L, 50L);
            {
                stream.Position = 0L;
                var buffer = new byte[100];
                stream.Read(buffer);
                Assert.That(buffer.SequenceEqual(data), Is.True);
            }

            Assert.Multiple(() =>
            {
                using var buffer = new LongArray<byte>(50L);

                Assert.Throws<ArgumentNullException>(() => StreamExtensions.Write(null!, new LongArray<byte>(1L), 0L, 1L));
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.Write(stream, null!, 0L, 1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.Write(buffer, -1L, 1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.Write(buffer, 0L, -1L));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.Write(buffer, 0L, 1L, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.Write(buffer, 0L, 1L, 0));
                Assert.Throws<ArgumentException>(() => stream.Write(buffer, 30L, 30L));
                Assert.Throws<NotSupportedException>(() => new TestStream(FileAccess.Read).Write(buffer, 0L, 50L));
                Assert.Throws<ObjectDisposedException>(() =>
                {
                    using var str = new TestStream();
                    str.Dispose();
                    str.Write(buffer, 0L, 5L);
                });
            });
        }

        /// <summary>
        /// <see cref="StreamExtensions.WriteAsync(Stream, LongArray{byte}, long, long, int, CancellationToken)"/>を検証します。
        /// </summary>
        [Test]
        public void WriteAsyncTest()
        {
            using LongArray<byte> writtenData = LongArray.Create(data);

            stream.WriteAsync(writtenData, 0L, 50L).Wait();
            {
                stream.Position = 0L;
                var buffer = new byte[50];
                stream.Read(buffer);
                Assert.That(buffer.SequenceEqual(data.Take(50)), Is.True);
            }

            stream.WriteAsync(writtenData, 50L, 50L).Wait();
            {
                stream.Position = 0L;
                var buffer = new byte[100];
                stream.Read(buffer);
                Assert.That(buffer.SequenceEqual(data), Is.True);
            }

            Assert.Multiple(() =>
            {
                using var buffer = new LongArray<byte>(50L);

                Assert.Throws<ArgumentNullException>(() => StreamExtensions.WriteAsync(null!, new LongArray<byte>(1L), 0L, 1L).Wait());
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.WriteAsync(stream, null!, 0L, 1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.WriteAsync(buffer, -1L, 1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.WriteAsync(buffer, 0L, -1L).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.WriteAsync(buffer, 0L, 1L, -1).Wait());
                Assert.Throws<ArgumentOutOfRangeException>(() => stream.WriteAsync(buffer, 0L, 1L, 0).Wait());
                Assert.Throws<ArgumentException>(() => stream.WriteAsync(buffer, 30L, 30L));
                TestHelper.AggregateThrows<NotSupportedException>(() => new TestStream(FileAccess.Read).WriteAsync(buffer, 0L, 50L).Wait());
                TestHelper.AggregateThrows<ObjectDisposedException>(() =>
                {
                    using var str = new TestStream();
                    str.Dispose();
                    str.WriteAsync(buffer, 0L, 5L).Wait();
                });
            });
        }
    }
}
