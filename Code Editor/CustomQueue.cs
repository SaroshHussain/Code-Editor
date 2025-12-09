using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Code_Editor
{
    internal class CustomQueue
    {
        private int head = 0;
        internal struct QueueElement
        {
            public string fileName;
            public string fileContent;
            public DateTime timestamp;
            public string filePath;
        }

        private readonly List<QueueElement> elements = new List<QueueElement>();
        private readonly int maxCapacity = 10;

        public void Enqueue(QueueElement element)
        {
            if (elements.Count >= maxCapacity)
            {
                elements.RemoveAt(0);
            }
            elements.Add(element);
        }

        public QueueElement Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            QueueElement element = elements[0];
            elements.RemoveAt(0);
            return element;
        }

        public QueueElement Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            return elements[0];
        }

        public bool IsEmpty()
        {
            return elements.Count == 0;
        }

        public int Count
        {
            get { return elements.Count; }
        }

        public void Clear()
        {
            elements.Clear();
        }

        public QueueElement GetAt(int index)
        {
            if (index < 0 || index >= elements.Count)
            {
                throw new IndexOutOfRangeException($"Index {index} is out of range. Queue has {elements.Count} elements.");
            }
            return elements[index];
        }

        public List<QueueElement> GetAll()
        {
            return new List<QueueElement>(elements);
        }

        public List<QueueElement> GetByFileName(string fileName)
        {
            return elements.Where(e => e.fileName == fileName).ToList();
        }

        public QueueElement GetLatest()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            return elements[elements.Count - 1];
        }

        public QueueElement GetOldest()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            return elements[0];
        }
    }
}
