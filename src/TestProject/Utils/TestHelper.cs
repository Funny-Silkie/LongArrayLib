using System;

namespace TestProject.Utils
{
    /// <summary>
    /// テストのヘルパクラスです。
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// 指定した例外を持つ<see cref="AggregateException"/>がスローされるかどうかを検証します。
        /// </summary>
        /// <typeparam name="TActual">例外クラス</typeparam>
        /// <param name="code"><typeparamref name="TActual"/>のスローされる処理</param>
        public static void AggregateThrows<TActual>(TestDelegate code)
            where TActual : Exception
        {
            Assert.Throws<TActual>(() =>
            {
                try
                {
                    code.Invoke();
                }
                catch (AggregateException e)
                {
                    if (e.InnerException is not null) throw e.InnerException;
                    else throw;
                }
            });
        }

        /// <summary>
        /// 指定した例外を持つ<see cref="AggregateException"/>がスローされるかどうかを検証します。
        /// </summary>
        /// <typeparam name="TActual">例外クラス</typeparam>
        /// <param name="code"><typeparamref name="TActual"/>のスローされる処理</param>
        public static void AggregateCatch<TActual>(TestDelegate code)
            where TActual : Exception
        {
            Assert.Catch<TActual>(() =>
            {
                try
                {
                    code.Invoke();
                }
                catch (AggregateException e)
                {
                    if (e.InnerException is not null) throw e.InnerException;
                    else throw;
                }
            });
        }
    }
}
