using System;
using System.Collections.Generic;
using System.Linq;

namespace AspiringDemo.Pathfinding
{
    [Serializable]
    public class PriorityQueue<T> where T : IComparable<T>
    {
        public List<T> data;

        public PriorityQueue()
        {
            data = new List<T>();
        }

        public void Put(T item)
        {
            //do check against last item
            if (data.Count == 0 || item.CompareTo(data.LastOrDefault()) >= 0)
            {
                // normal add is always appended as the last entry
                data.Add(item);
            }

            int index = 0;

            // improve this by guessing position
            for (int i = index; i < data.Count; i++)
            {
                if (item.CompareTo(data[i]) <= 0)
                {
                    data.Insert(i, item);
                    break;
                }
            }
        }

        public T Pop()
        {
            T frontItem = data[0];
            data.RemoveAt(0);
            return frontItem;
        }

        public T Peek()
        {
            T frontItem = data[0];
            return frontItem;
        }

        // TODO: Rewrite this
        public bool Contains(T node)
        {
            return data.Contains(node);
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < data.Count; ++i)
                s += data[i] + " ";
            s += "count = " + data.Count;
            return s;
        }

        public bool IsConsistent()
        {
            // is the heap property true for all data?
            if (data.Count == 0) return true;
            int li = data.Count - 1; // last index
            for (int pi = 0; pi < data.Count; ++pi) // each parent index
            {
                int lci = 2 * pi + 1; // left child index
                int rci = 2 * pi + 2; // right child index

                if (lci <= li && data[pi].CompareTo(data[lci]) > 0)
                    return false; // if lc exists and it's greater than parent then bad.
                if (rci <= li && data[pi].CompareTo(data[rci]) > 0) return false; // check the right child too.
            }
            return true; // passed all checks
        }
    }
}