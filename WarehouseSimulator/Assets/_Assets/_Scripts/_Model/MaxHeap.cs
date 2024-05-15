using System;
using System.Collections.Generic;

namespace WarehouseSimulator.Model
{
    
    /// <summary>
    ///     Maximal heap implementation of a heap data structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class MaxHeap<T,S> : Heap<T,S>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MaxHeap()
            : this(Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Constructor with comparer
        /// </summary>
        /// <param name="comparer"></param>
        public MaxHeap(Comparer<T> comparer)
            : base(comparer)
        {
        }
        
        /// <summary>
        /// Constructor with IEnumerable collection and comparer
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public MaxHeap(IEnumerable<Tuple<T,S>> collection, Comparer<T> comparer)
            : base(collection, comparer)
        {
        }

        /// <summary>
        /// Constructor with IEnumerable collection
        /// </summary>
        /// <param name="collection"></param>
        public MaxHeap(IEnumerable<Tuple<T,S>> collection) : base(collection)
        {
        }

        /// <summary>
        /// Override of the Dominates method from the base class.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if x dominates y or are equal, otherwise false.</returns>
        protected override bool Dominates(Tuple<T,S> x, Tuple<T,S> y)
        {
            return Comparer.Compare(x.Item1, y.Item1) >= 0;
        }
    }
}