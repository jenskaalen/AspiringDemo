using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Pathfinding
{
    [Serializable]
    public class SortedPath<T> where T : IPathfindingNode
    {
        //private SortedList<T, float> _list;
        public SortedSet<T> data;

        public SortedPath()
        {
            data = new SortedSet<T>();
        }

        public void Put(T node)
        {
            data.Add(node);
        }

        public bool Contains(T node)
        {
            return data.Contains(node);
        }

        public T Pop()
        {
            var obj = data.First();
            data.Remove(obj);

            return obj;
        }

        public void Pop(T node)
        {
            // safe remove
            bool contains = data.Contains(node);

            if (contains != null)
            {
                data.Remove(node);
            }
            else
                throw new Exception("wtf");
        }
    }
}
