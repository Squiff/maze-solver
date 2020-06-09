using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    /// <summary>
    /// Container for storing an item in the PriorityQueue
    /// </summary>
    /// <typeparam name="T">Type of object being stored in the queue</typeparam>
    class PriorityQueueItem<T>
    {
        public T Item { get; set; }
        public double Priority { get; set; }
        public bool Removed { get; set; }

        public PriorityQueueItem(double priority, T item)
        {
            Priority = priority;
            Item = item;
        }
    }

}
