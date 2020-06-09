using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    /// <summary>
    /// Min Priority queue
    /// </summary>
    /// <typeparam name="T">type stored in the queue</typeparam>
    class PriorityQueue<T>
    {
        /// <summary>
        /// Priority Queue Min Heap
        /// </summary>
        public List<PriorityQueueItem<T>> Queue { get; }
        private Dictionary<T, PriorityQueueItem<T>> _itemTracker;

        public PriorityQueue()
        {
            Queue = new List<PriorityQueueItem<T>>();
            _itemTracker = new Dictionary<T, PriorityQueueItem<T>>();
        }

        /// <summary>
        /// Number of items in the queue
        /// </summary>
        public int Count
        {
            get { return Queue.Count; }
        }

        /// <summary>
        /// Add an object to the priority queue. If the object is already in the queue the old PriorityQueueItem will be removed
        /// </summary>
        /// <param name="item">item to add to th3e queue</param>
        /// <param name="priority">priority of the item</param>
        public void Push(T item, double priority)
        {
            PriorityQueueItem<T> queueItem = new PriorityQueueItem<T>(priority, item);
            Push(queueItem);
        }

        /// <summary>
        /// Add PriorityQueueItem to the queue. If reference item is already in the queue it will be remnoved
        /// </summary>
        private void Push(PriorityQueueItem<T> queueItem)
        {
            TrackItem(queueItem);
            Queue.Add(queueItem);

            // maintain the min heap
            int child_pos = Queue.Count - 1;

            while (child_pos > 0)
            {
                int parent_pos = (child_pos - 1) / 2;

                if (Queue[child_pos].Priority > Queue[parent_pos].Priority)
                    break;

                PriorityQueueItem<T> tempItem = Queue[child_pos];
                Queue[child_pos] = Queue[parent_pos];
                Queue[parent_pos] = tempItem;
                child_pos = parent_pos;
            }
        }

        /// <summary>
        /// Remove and return the item at the top of the queue
        /// </summary>
        public T Pop()
        {
            while (Count > 0)
            {
                PriorityQueueItem<T> topQueueItem = Queue[0];
                UntrackItem(topQueueItem);

                Reheap();

                if (topQueueItem.Removed)
                    continue;

                return topQueueItem.Item;
            }

            // no item left in queue. return null (or equivilent default for type).
            return default(T);
        }

        /// <summary>
        /// Get the queue item for the specified object
        /// </summary>
        public PriorityQueueItem<T> GetQueueItem(T obj)
        {
            PriorityQueueItem<T> output;
            _itemTracker.TryGetValue(obj, out output);
            return output;
        }

        /// <summary>
        /// Remove item at the top and reheaplify the queue
        /// </summary>
        private void Reheap()
        {
            // replace top item with last
            int last = Count - 1;
            Queue[0] = Queue[last];
            Queue.RemoveAt(last); // remove the old top item
            last--;

            // iterate until min heap property satisfied
            int parent = 0;
            while (true)
            {
                int leftChild = (parent * 2) + 1;
                int rightChild = leftChild + 1;

                if (leftChild > last)
                    break; // no children

                // get smallest child
                int smallestChild;
                if ((rightChild <= last) && (Queue[rightChild].Priority < Queue[leftChild].Priority))
                     smallestChild = rightChild;
                else
                    smallestChild = leftChild;

                if (Queue[parent].Priority < Queue[smallestChild].Priority)
                    break; // parent is smaller.

                // swap with smallest child
                PriorityQueueItem<T> tempItem = Queue[smallestChild];
                Queue[smallestChild] = Queue[parent];
                Queue[parent] = tempItem;
                parent = smallestChild;

            }
        }

        /// <summary>
        /// Maintains an item -> PriorityQueueItem reference
        /// </summary>
        private void TrackItem(PriorityQueueItem<T> queueItem)
        {
            // if item is already in the queue, mark it as removed and update refernce to new item
            if (_itemTracker.ContainsKey(queueItem.Item))
            {
                _itemTracker[queueItem.Item].Removed = true;
                _itemTracker[queueItem.Item] = queueItem;
            }
            else
            {
                _itemTracker.Add(queueItem.Item, queueItem);
            }
        }

        /// <summary>
        /// Removes Item -> PriorityQueueItem reference
        /// </summary>
        /// <param name="queueItem"></param>
        private void UntrackItem(PriorityQueueItem<T> queueItem)
        {
            if (_itemTracker.ContainsKey(queueItem.Item))
                _itemTracker.Remove(queueItem.Item);
        }

    }
}
