using LongArrayLib;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestProject
{
    // !!REMARKS!!
    // Binary serialize is obsolete way of serialization.
    // This code will be removed.

    public partial class LongArrayTest
    {
#pragma warning disable SYSLIB0011 // 型またはメンバーが旧型式です

        /// <summary>
        /// シリアライズを検証します。
        /// </summary>
        [Test]
        public void Serialization()
        {
            var formatter = new BinaryFormatter();
            using var serializeStream = new MemoryStream();

            // serialization
            formatter.Serialize(serializeStream, array1);
            serializeStream.Position = 0L;

            // deserialization
            var recovered = (LongArray<long>)formatter.Deserialize(serializeStream);

            Assert.Multiple(() =>
            {
                Assert.That(ReferenceEquals(recovered, array1), Is.False);
                Assert.That(recovered, Has.Length.EqualTo(array1.Length));
                Assert.That(recovered.SequenceEqual(array1), Is.True);
            });
        }

#pragma warning restore SYSLIB0011 // 型またはメンバーが旧型式です
    }
}
