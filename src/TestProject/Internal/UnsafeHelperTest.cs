using LongArrayLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.Internal
{
    /// <summary>
    /// <see cref="UnsafeHelper"/>のテストを表します。
    /// </summary>
    [TestFixture]
    public class UnsafeHelperTest
    {
#pragma warning disable NUnit2045 // Use Assert.Multiple

        /// <summary>
        /// <see cref="UnsafeHelper.GetInnerArray{T}(Stack{T}, out Span{T})"/>を検証します。
        /// </summary>
        [Test]
        public void GetInnerArrayFromStack()
        {
            const int NumElement = 100;

            var stack = new Stack<int>(NumElement);
            for (int i = 0; i < NumElement; i++) stack.Push(i);

            UnsafeHelper.GetInnerArray(stack, out Span<int> span);

            Assert.That(span.Length, Is.EqualTo(stack.Count));
            Assert.That(span.SequenceEqual(stack.Reverse().ToArray()), Is.True);
        }

        /// <summary>
        /// <see cref="UnsafeHelper.GetInnerArray{T}(Queue{T}, out Span{T}, out Span{T})"/>を検証します。
        /// </summary>
        [Test]
        [Repeat(30)]
        public void GetInnerArrayFromQueue()
        {
            int numElement = Random.Shared.Next(10, 300);

            var queue = new Queue<int>();
            for (int i = 0; i < numElement; i++)
            {
                if (i % 3 == 1) queue.Dequeue();
                else queue.Enqueue(i);
            }

            UnsafeHelper.GetInnerArray(queue, out Span<int> head, out Span<int> tail);
            IEnumerable<int> actualSequence = head.ToArray().Concat(tail.ToArray());

            Assert.That(head.Length + tail.Length, Is.EqualTo(queue.Count));
            Assert.That(actualSequence.SequenceEqual(queue), Is.True);
        }

#pragma warning restore NUnit2045 // Use Assert.Multiple
    }
}
