using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using WarehouseSimulator.Model;

namespace _Assets._Scripts._Tests
{
    
    public class HeapTests
    {
        [Test]
        public void TestHeapBySorting()
        {
            var minHeap = new MinHeap<int,int>(new[] {new Tuple<int, int>(2,2),new Tuple<int, int>(2,3),new Tuple<int, int>(1,2),new Tuple<int, int>(8,2),new Tuple<int,int>(9,2),new Tuple<int,int>(4,4)});
            AssertHeapSort(minHeap, minHeap.OrderBy(i => i.Item1).Select(i => i.Item1).ToArray());

            minHeap = new MinHeap<int,int> { new Tuple<int,int>(7,1), new Tuple<int,int>(5,2), new Tuple<int,int>(1,3), new Tuple<int,int>(6,4), new Tuple<int,int>(3,5), new Tuple<int,int>(2,6), new Tuple<int,int>(4,7), new Tuple<int,int>(1,8), new Tuple<int,int>(2,9), new Tuple<int,int>(1,10), new Tuple<int,int>(3,11), new Tuple<int,int>(4,12), new Tuple<int,int>(7,13) };
            AssertHeapSort(minHeap, minHeap.OrderBy(i => i.Item1).Select(i => i.Item1).ToArray());

            var maxHeap = new MaxHeap<int,int>(new[] {new Tuple<int,int>(1,1), new Tuple<int,int>(5,2), new Tuple<int,int>(3,3), new Tuple<int,int>(2,4), new Tuple<int,int>(7,5), new Tuple<int,int>(56,6), new Tuple<int,int>(3,7), new Tuple<int,int>(1,8), new Tuple<int,int>(23,9), new Tuple<int,int>(5,10), new Tuple<int,int>(2,11), new Tuple<int,int>(1,12)});
            AssertHeapSort(maxHeap, maxHeap.OrderBy(d => -d.Item1).Select(i => i.Item1).ToArray());

            maxHeap = new MaxHeap<int,int> {new Tuple<int,int>(2,1), new Tuple<int,int>(6,2), new Tuple<int,int>(1,3), new Tuple<int,int>(3,4), new Tuple<int,int>(56,5), new Tuple<int,int>(1,6), new Tuple<int,int>(4,7), new Tuple<int,int>(7,8), new Tuple<int,int>(8,9), new Tuple<int,int>(23,10), new Tuple<int,int>(4,11), new Tuple<int,int>(5,12), new Tuple<int,int>(7,13), new Tuple<int,int>(34,14), new Tuple<int,int>(1,15), new Tuple<int,int>(4,16)};
            AssertHeapSort(maxHeap, maxHeap.OrderBy(d => -d.Item1).Select(i => i.Item1).ToArray());
        }
        
        [Test]
        public void HeapSort_ShouldThrowException_WhenHeapIsEmpty()
        {
            var emptyHeap = new MinHeap<int,int>();
            Assert.Throws<InvalidOperationException>(() =>
            {
                emptyHeap.ExtractDominating();
            });
        }
        
        [Test]
        public void HeapSort_ShouldSortCorrectly_WhenHeapContainsDuplicates()
        {
            var heapWithDuplicates = new MinHeap<int,int>(new[] {new Tuple<int, int>(2,2),new Tuple<int, int>(2,3),new Tuple<int, int>(1,2),new Tuple<int, int>(2,2)});
            AssertHeapSort(heapWithDuplicates, heapWithDuplicates.OrderBy(i => i.Item1).Select(i => i.Item1).ToArray());
        }
        
        [Test]
        public void HeapSort_ShouldSortCorrectly_WhenHeapContainsSingleElement()
        {
            var singleElementHeap = new MinHeap<int,int>(new[] {new Tuple<int, int>(2,2)});
            AssertHeapSort(singleElementHeap, singleElementHeap.OrderBy(i => i.Item1).Select(i => i.Item1).ToArray());
        }

        private static void AssertHeapSort(Heap<int,int> heap, IEnumerable<int> expected)
        {
            var sortedKeys = new List<int>();
            while (heap.Count > 0)
                sortedKeys.Add(heap.ExtractDominating().Item1);
            
             
            
            Assert.IsTrue(sortedKeys.SequenceEqual(expected));
        }
    }
}