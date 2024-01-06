using System.IO;
using System.Runtime.InteropServices;

namespace TestProject.Utils
{
    /// <summary>
    /// テスト用のストリームオブジェクトを表します。
    /// </summary>
    internal sealed unsafe class TestStream : UnmanagedMemoryStream
    {
        private const long DefaultBufferSize = 4096L;

        private byte* buffer;
        private bool isDisposed;

        /// <summary>
        /// <see cref="TestStream"/>の新しいインスタンスを初期化します。
        /// </summary>
        public TestStream()
            : this(DefaultBufferSize, FileAccess.ReadWrite)
        {
        }

        /// <summary>
        /// <see cref="TestStream"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="bufferSize">バッファーサイズ</param>
        public TestStream(long bufferSize)
            : this(bufferSize, FileAccess.ReadWrite)
        {
        }

        /// <summary>
        /// <see cref="TestStream"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="access">許可するアクセス</param>
        public TestStream(FileAccess access)
            : this(DefaultBufferSize, access)
        {
        }

        /// <summary>
        /// <see cref="TestStream"/>の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="bufferSize">バッファーサイズ</param>
        /// <param name="access">許可するアクセス</param>
        public TestStream(long bufferSize, FileAccess access)
        {
            buffer = (byte*)NativeMemory.Alloc((nuint)bufferSize);
            Initialize(buffer, bufferSize, bufferSize, access);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (isDisposed) return;

            NativeMemory.Free(buffer);
            buffer = null;

            base.Dispose(disposing);
            isDisposed = true;
        }
    }
}
