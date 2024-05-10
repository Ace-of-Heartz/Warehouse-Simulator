using System;
using System.Collections.Generic;

namespace WarehouseSimulator.Model
{
    /// <summary>
    ///     Minimal heap implementation of a heap data structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class MinHeap<T,S> : Heap<T,S>
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public MinHeap()
            : this(Comparer<T>.Default)
        {
        }

        /// <summary>
        ///     Constructor with comparer
        /// </summary>
        /// <param name="comparer"></param>
        public MinHeap(Comparer<T> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        ///     Constructor with IEnumerable collection
        /// </summary>
        /// <param name="collection"></param>
        public MinHeap(IEnumerable<Tuple<T,S>> collection) : base(collection)
        {
        }

        /// <summary>
        ///     Constructor with IEnumerable collection and comparer
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public MinHeap(IEnumerable<Tuple<T,S>> collection, Comparer<T> comparer)
            : base(collection, comparer)
        {
        }

        /// <summary>
        /// Override of the Dominates method from the base class.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if y dominates or are equal to x, otherwise false</returns>
        protected override bool Dominates(Tuple<T,S> x, Tuple<T,S> y)
        {
            return Comparer.Compare(x.Item1, y.Item1) <= 0;
        }
    }
}