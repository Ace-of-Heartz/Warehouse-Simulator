using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace WarehouseSimulator.Model
{
    public class MaxHeap<T,S> : Heap<T,S>
    {
        public MaxHeap()
            : this(Comparer<T>.Default)
        {
        }

        public MaxHeap(Comparer<T> comparer)
            : base(comparer)
        {
        }

        public MaxHeap(IEnumerable<Tuple<T,S>> collection, Comparer<T> comparer)
            : base(collection, comparer)
        {
        }

        public MaxHeap(IEnumerable<Tuple<T,S>> collection) : base(collection)
        {
        }

        protected override bool Dominates(Tuple<T,S> x, Tuple<T,S> y)
        {
            return Comparer.Compare(x.Item1, y.Item1) >= 0;
        }
    }
}