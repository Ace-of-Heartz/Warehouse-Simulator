using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WarehouseSimulator.Model
{
    /// <summary>
    ///     Abstract class for heap data structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public abstract class Heap<T, S> : IEnumerable<Tuple<T, S>>
    {
        #region Fields

        private const int InitialCapacity = 0;
        private const int GrowFactor = 2;
        private const int MinGrow = 1;

        private Tuple<T, S>[] _heap = new Tuple<T, S>[InitialCapacity];

        #endregion

        #region Properties

        public int Count { get; private set; }

        public int Capacity { get; private set; } = InitialCapacity;

        protected Comparer<T> Comparer { get; private set; }
        protected abstract bool Dominates(Tuple<T, S> x, Tuple<T, S> y);

        #endregion

        #region Constructors

        /// <summary>
        ///     Default constructor
        /// </summary>
        protected Heap() : this(Comparer<T>.Default)
        {
        }

        /// <summary>
        ///     Constructor with comparer. Implements Enumerable.Empty.
        /// </summary>
        /// <param name="comparer"></param>
        protected Heap(Comparer<T> comparer) : this(Enumerable.Empty<Tuple<T, S>>(), comparer)
        {
        }

        /// <summary>
        ///     Constructor with IEnumerable collection
        /// </summary>
        /// <param name="collection"></param>
        protected Heap(IEnumerable<Tuple<T, S>> collection)
            : this(collection, Comparer<T>.Default)
        {
        }

        /// <summary>
        ///     Constructor with IEnumerable collection and comparer
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected Heap(IEnumerable<Tuple<T, S>> collection, Comparer<T> comparer)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (comparer == null) throw new ArgumentNullException("comparer");

            Comparer = comparer;

            foreach (var item in collection)
            {
                if (Count == Capacity)
                    Grow();

                _heap[Count++] = item;
            }

            for (var i = Parent(Count - 1); i >= 0; i--)
                BubbleDown(i);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Add item to heap
        /// </summary>
        /// <param name="item">T must implement Comparer</param>
        public void Add(Tuple<T, S> item)
        {
            if (Count == Capacity)
                Grow();

            _heap[Count++] = item;
            BubbleUp(Count - 1);
        }

        /// <summary>
        ///     Bubble up the heap
        /// </summary>
        /// <param name="i">Index</param>
        private void BubbleUp(int i)
        {
            if (i == 0 || Dominates(_heap[Parent(i)], _heap[i]))
                return; //correct domination (or root)

            Swap(i, Parent(i));
            BubbleUp(Parent(i));
        }

        /// <summary>
        ///     Get the minimum element from the heap
        /// </summary>
        /// <returns>Key-value pair, with minimum key</returns>
        /// <exception cref="InvalidOperationException">Throws this if heap is empty</exception>
        public Tuple<T, S> GetMin()
        {
            if (Count == 0) throw new InvalidOperationException("Heap is empty");
            return _heap[0];
        }

        /// <summary>
        ///     Extract the dominating element from the heap
        /// </summary>
        /// <returns>Key-value pair, with dominating key</returns>
        /// <exception cref="InvalidOperationException">Throws this if heap is empty</exception>
        public Tuple<T, S> ExtractDominating()
        {
            if (Count == 0) throw new InvalidOperationException("Heap is empty");
            var ret = _heap[0];
            Count--;
            Swap(Count, 0);
            BubbleDown(0);
            return ret;
        }

        /// <summary>
        ///     Bubble down the heap
        /// </summary>
        /// <param name="i">Index</param>
        private void BubbleDown(int i)
        {
            var dominatingNode = Dominating(i);
            if (dominatingNode == i) return;
            Swap(i, dominatingNode);
            BubbleDown(dominatingNode);
        }

        /// <summary>
        ///     Dominating node
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Dominating(int i)
        {
            var dominatingNode = i;
            dominatingNode = GetDominating(YoungChild(i), dominatingNode);
            dominatingNode = GetDominating(OldChild(i), dominatingNode);

            return dominatingNode;
        }

        /// <summary>
        ///     Get the dominating node
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="dominatingNode"></param>
        /// <returns></returns>
        private int GetDominating(int newNode, int dominatingNode)
        {
            if (newNode < Count && !Dominates(_heap[dominatingNode], _heap[newNode]))
                return newNode;
            return dominatingNode;
        }

        /// <summary>
        ///     Swap two elements in the heap
        /// </summary>
        /// <param name="i">Index of first element</param>
        /// <param name="j">Index of second element</param>
        private void Swap(int i, int j)
        {
            var tmp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = tmp;
        }

        /// <summary>
        ///     Gets the parent of the node
        /// </summary>
        /// <param name="i">Index of element of which node's parent node we want</param>
        /// <returns>Index of parent</returns>
        private static int Parent(int i)
        {
            return (i + 1) / 2 - 1;
        }

        /// <summary>
        ///     Gets the young child of the node
        /// </summary>
        /// <param name="i">Index of the element of which node's young child we want</param>
        /// <returns>Index of young child</returns>
        private static int YoungChild(int i)
        {
            return (i + 1) * 2 - 1;
        }

        /// <summary>
        ///     Gets the old child of the node
        /// </summary>
        /// <param name="i">Index of the element of which node's old child we wan</param>
        /// <returns>Index of old child</returns>
        private static int OldChild(int i)
        {
            return YoungChild(i) + 1;
        }

        /// <summary>
        ///     Increase the capacity of the heap
        /// </summary>
        private void Grow()
        {
            var newCapacity = Capacity * GrowFactor + MinGrow;
            var newHeap = new Tuple<T, S>[newCapacity];
            Array.Copy(_heap, newHeap, Capacity);
            _heap = newHeap;
            Capacity = newCapacity;
        }

        /// <summary>
        ///     Implementation of IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Tuple<T, S>> GetEnumerator()
        {
            return _heap.Take(Count).GetEnumerator();
        }

        /// <summary>
        ///     Implementation of IEnumerable
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}