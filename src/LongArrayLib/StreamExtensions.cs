using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LongArrayLib
{
    /// <summary>
    /// <see cref="Stream"/>の拡張を記述します。
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// デフォルトのバッファーサイズ
        /// </summary>
        private const int DefaultBufferSize = 81920;

        /// <summary>
        /// データを読み取って配列に書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込み先の配列</param>
        /// <param name="offSet"><paramref name="buffer"/>における書き込み開始インデックス</param>
        /// <param name="count">読み取る最大バイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <returns>読み取ったバイト数</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>または<paramref name="buffer"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="offSet"/>または<paramref name="count"/>が0未満</item>
        /// <item><paramref name="bufferSize"/>が0以下</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="buffer"/>のサイズが不足</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が読み取りをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        public static long Read(this Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize = DefaultBufferSize)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(buffer);
            ThrowHelper.ThrowIfNegative(offSet);
            ThrowHelper.ThrowIfNegative(count);
            ThrowHelper.ThrowIfNegativeOrZero(bufferSize);
            if (offSet + count > buffer.Length) ThrowHelper.ThrowAsShortArray(nameof(buffer));

            long result = 0L;
            byte[] poolBuffer = ArrayPool<byte>.Shared.Rent((int)Math.Min(bufferSize, count));

            try
            {
                while (count > 0)
                {
                    int readBytes = stream.Read(poolBuffer, 0, count > poolBuffer.Length ? poolBuffer.Length : (int)count);
                    if (readBytes == 0) break;

                    result += readBytes;
                    LongArray.Copy(poolBuffer, 0, buffer, offSet, readBytes);
                    offSet += readBytes;
                    count -= readBytes;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(poolBuffer);
            }

            return result;
        }

        /// <summary>
        /// データを読み取って配列に書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込み先の配列</param>
        /// <param name="offSet"><paramref name="buffer"/>における書き込み開始インデックス</param>
        /// <param name="count">読み取る最大バイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <param name="cancellationToken">キャンセル要求を監視するトークン</param>
        /// <returns>読み取ったバイト数</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>または<paramref name="buffer"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="offSet"/>または<paramref name="count"/>が0未満</item>
        /// <item><paramref name="bufferSize"/>が0以下</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="buffer"/>のサイズが不足</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が読み取りをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        /// <exception cref="TaskCanceledException">操作がキャンセルされた</exception>
        public static Task<long> ReadAsync(this Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize = DefaultBufferSize, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(buffer);
            ThrowHelper.ThrowIfNegative(offSet);
            ThrowHelper.ThrowIfNegative(count);
            if (offSet + count > buffer.Length) ThrowHelper.ThrowAsShortArray(nameof(buffer));
            ThrowHelper.ThrowIfNegativeOrZero(bufferSize);

            return ReadAsyncCore(stream, buffer, offSet, count, bufferSize, cancellationToken);
        }

        /// <summary>
        /// データを読み取って配列に書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込み先の配列</param>
        /// <param name="offSet"><paramref name="buffer"/>における書き込み開始インデックス</param>
        /// <param name="count">読み取る最大バイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <param name="cancellationToken">キャンセル要求を監視するトークン</param>
        /// <returns>読み取ったバイト数</returns>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が読み取りをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        /// <exception cref="TaskCanceledException">操作がキャンセルされた</exception>
        private static async Task<long> ReadAsyncCore(Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize, CancellationToken cancellationToken)
        {
            long result = 0L;
            byte[] poolBuffer = ArrayPool<byte>.Shared.Rent((int)Math.Min(bufferSize, count));
            var memory = new Memory<byte>(poolBuffer);

            try
            {
                while (count > 0)
                {
                    int readBytes = memory.Length < count ?
                        await stream.ReadAsync(memory, cancellationToken) :
                        await stream.ReadAsync(poolBuffer.AsMemory(0, (int)count), cancellationToken);
                    if (readBytes == 0) break;

                    result += readBytes;
                    LongArray.Copy(poolBuffer, 0, buffer, offSet, readBytes);
                    offSet += readBytes;
                    count -= readBytes;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(poolBuffer);
            }

            return result;
        }

        /// <summary>
        /// データを読み取って配列に書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込み先の配列</param>
        /// <param name="offSet"><paramref name="buffer"/>における書き込み開始インデックス</param>
        /// <param name="count">読み取るバイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>または<paramref name="buffer"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="offSet"/>または<paramref name="count"/>が0未満</item>
        /// <item><paramref name="bufferSize"/>が0以下</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="buffer"/>のサイズが不足</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が読み取りをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        /// <exception cref="EndOfStreamException"><paramref name="count"/>バイト読み取る前にストリームの終端に到達</exception>
        public static void ReadExactly(this Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize = DefaultBufferSize)
        {
            if (Read(stream, buffer, offSet, count, bufferSize) < count) ThrowHelper.ThrowAsEndOfStream();
        }

        /// <summary>
        /// データを読み取って配列に書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込み先の配列</param>
        /// <param name="offSet"><paramref name="buffer"/>における書き込み開始インデックス</param>
        /// <param name="count">読み取るバイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <param name="cancellationToken">キャンセル要求を監視するトークン</param>
        /// <returns>読み取ったバイト数</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>または<paramref name="buffer"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="offSet"/>または<paramref name="count"/>が0未満</item>
        /// <item><paramref name="bufferSize"/>が0以下</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="buffer"/>のサイズが不足</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が読み取りをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        /// <exception cref="EndOfStreamException"><paramref name="count"/>バイト読み取る前にストリームの終端に到達</exception>
        /// <exception cref="TaskCanceledException">操作がキャンセルされた</exception>
        public static Task ReadExactlyAsync(this Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize = DefaultBufferSize, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(buffer);
            ThrowHelper.ThrowIfNegative(offSet);
            ThrowHelper.ThrowIfNegative(count);
            if (offSet + count > buffer.Length) ThrowHelper.ThrowAsShortArray(nameof(buffer));
            ThrowHelper.ThrowIfNegativeOrZero(bufferSize);

            return ReadExactlyAsyncCore(stream, buffer, offSet, count, bufferSize, cancellationToken);
        }

        /// <summary>
        /// データを読み取って配列に書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込み先の配列</param>
        /// <param name="offSet"><paramref name="buffer"/>における書き込み開始インデックス</param>
        /// <param name="count">読み取る最大バイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <param name="cancellationToken">キャンセル要求を監視するトークン</param>
        /// <returns>読み取ったバイト数</returns>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が読み取りをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        /// <exception cref="EndOfStreamException"><paramref name="count"/>バイト読み取る前にストリームの終端に到達</exception>
        /// <exception cref="TaskCanceledException">操作がキャンセルされた</exception>
        private static async Task ReadExactlyAsyncCore(this Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize = DefaultBufferSize, CancellationToken cancellationToken = default)
        {
            try
            {
                long readbytes = await ReadAsync(stream, buffer, offSet, count, bufferSize, cancellationToken);
                if (readbytes < count) ThrowHelper.ThrowAsEndOfStream();
            }
            catch (AggregateException e)
            {
                if (e.InnerException is not null) throw e.InnerException;
                else throw;
            }
        }

        /// <summary>
        /// データを書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込むデータの配列</param>
        /// <param name="offSet"><paramref name="buffer"/>におけるデータ開始インデックス</param>
        /// <param name="count">書き込むバイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>または<paramref name="buffer"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="offSet"/>または<paramref name="count"/>が0未満</item>
        /// <item><paramref name="bufferSize"/>が0以下</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="buffer"/>のサイズが不足</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が書き込みをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        public static void Write(this Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize = DefaultBufferSize)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(buffer);
            ThrowHelper.ThrowIfNegative(offSet);
            ThrowHelper.ThrowIfNegative(count);
            if (offSet + count > buffer.Length) ThrowHelper.ThrowAsShortArray(nameof(buffer));
            ThrowHelper.ThrowIfNegativeOrZero(bufferSize);

            long end = offSet + count;
            byte[] poolBuffer = ArrayPool<byte>.Shared.Rent((int)Math.Min(bufferSize, count));

            try
            {
                while (offSet < end)
                {
                    int copyLength = (int)Math.Min(end - offSet, poolBuffer.Length);
                    LongArray.Copy(buffer, offSet, poolBuffer, 0, copyLength);

                    stream.Write(poolBuffer, 0, copyLength);

                    offSet += copyLength;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(poolBuffer);
            }
        }

        /// <summary>
        /// データを書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込むデータの配列</param>
        /// <param name="offSet"><paramref name="buffer"/>におけるデータ開始インデックス</param>
        /// <param name="count">書き込むバイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <param name="cancellationToken">キャンセル要求を監視するトークン</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>または<paramref name="buffer"/>が<see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item><paramref name="offSet"/>または<paramref name="count"/>が0未満</item>
        /// <item><paramref name="bufferSize"/>が0以下</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="buffer"/>のサイズが不足</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が書き込みをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        /// <exception cref="InvalidOperationException">既に書き込み処理が実行されている</exception>
        public static Task WriteAsync(this Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize = DefaultBufferSize, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(buffer);
            ThrowHelper.ThrowIfNegative(offSet);
            ThrowHelper.ThrowIfNegative(count);
            if (offSet + count > buffer.Length) ThrowHelper.ThrowAsShortArray(nameof(buffer));
            ThrowHelper.ThrowIfNegativeOrZero(bufferSize);

            return WriteAsyncCore(stream, buffer, offSet, count, bufferSize, cancellationToken);
        }

        /// <summary>
        /// データを書き出します。
        /// </summary>
        /// <param name="stream">対象のストリームオブジェクト</param>
        /// <param name="buffer">書き込むデータの配列</param>
        /// <param name="offSet"><paramref name="buffer"/>におけるデータ開始インデックス</param>
        /// <param name="count">書き込むバイト数</param>
        /// <param name="bufferSize">使用するバッファーのサイズ</param>
        /// <param name="cancellationToken">キャンセル要求を監視するトークン</param>
        /// <exception cref="ObjectDisposedException"><paramref name="stream"/>が既に破棄されている</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/>が書き込みをサポートしない</exception>
        /// <exception cref="IOException">I/Oエラーが発生</exception>
        /// <exception cref="InvalidOperationException">既に書き込み処理が実行されている</exception>
        private static async Task WriteAsyncCore(Stream stream, LongArray<byte> buffer, long offSet, long count, int bufferSize, CancellationToken cancellationToken)
        {
            long end = offSet + count;
            byte[] poolBuffer = ArrayPool<byte>.Shared.Rent((int)Math.Min(bufferSize, count));

            try
            {
                while (offSet < end)
                {
                    int copyLength = (int)Math.Min(end - offSet, poolBuffer.Length);
                    LongArray.Copy(buffer, offSet, poolBuffer, 0, copyLength);

                    await stream.WriteAsync(poolBuffer, 0, copyLength, cancellationToken);

                    offSet += copyLength;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(poolBuffer);
            }
        }
    }
}
