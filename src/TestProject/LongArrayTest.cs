using LongArrayLib;

namespace TestProject
{
    /// <summary>
    /// <see cref="LongArray{T}"/>・<see cref="LongArray"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public partial class LongArrayTest
    {
        private LongArray<long> array1;
        private LongArray<string> array2;
        private LongArray<byte> array3;

        /// <summary>
        /// セットアップ処理を記述します。
        /// </summary>
        [SetUp]
        public void Setup()
        {
            array1 = new LongArray<long>(3);
            for (long i = 0; i < array1.Length; i++) array1[i] = -i;

#if NET8_0_OR_GREATER
            array2 = ["hoge", "fuga", "piyo", "hoge"];
#else
            array2 = new LongArray<string>(4);
            array2[0] = "hoge";
            array2[1] = "fuga";
            array2[2] = "piyo";
            array2[3] = "hoge";
#endif

            array3 = new LongArray<byte>(10);
            for (long i = 0; i < array3.Length; i++) array3[i] = (byte)(i + 10);
        }

        /// <summary>
        /// クリーンアップ処理を記述します。
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            array1.Dispose();
            array2.Dispose();
            array3.Dispose();
        }
    }
}
