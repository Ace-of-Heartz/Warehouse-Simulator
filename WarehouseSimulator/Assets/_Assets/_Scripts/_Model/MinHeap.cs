using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



namespace WarehouseSimulator.Model
{
    public class MinHeap<T,S> : Heap<T,S>
    {
        public MinHeap()
            : this(Comparer<T>.Default)
        {
        }

        public MinHeap(Comparer<T> comparer)
            : base(comparer)
        {
        }

        public MinHeap(IEnumerable<Tuple<T,S>> collection) : base(collection)
        {
        }

        public MinHeap(IEnumerable<Tuple<T,S>> collection, Comparer<T> comparer)
            : base(collection, comparer)
        {
        }

        protected override bool Dominates(Tuple<T,S> x, Tuple<T,S> y)
        {
            return Comparer.Compare(x.Item1, y.Item1) <= 0;
        }
    }
}