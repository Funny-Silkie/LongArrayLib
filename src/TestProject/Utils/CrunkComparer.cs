using System.Collections;
using System.Collections.Generic;

namespace TestProject.Utils
{
    /// <summary>
    /// でたらめな値を返す<see cref="IComparer{T}"/>を表します。
    /// </summary>
    /// <typeparam name="T">既定値</typeparam>
    internal sealed class CrunkComparer<T> : IComparer<T>, IComparer
    {
        /// <summary>
        /// <see cref="CrunkComparer{T}"/>の新しいインスタンスを初期化します。
        /// </summary>
        public CrunkComparer()
        {
        }

        /// <inheritdoc/>
        public int Compare(T? x, T? y) => 1;

        int IComparer.Compare(object? x, object? y) => 1;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is CrunkComparer<T>;

        /// <inheritdoc/>
        public override int GetHashCode() => GetType().Name.GetHashCode();
    }
}
