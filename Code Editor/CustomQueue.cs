namespace Code_Editor
{
    internal class CustomQueue
    {

        internal struct QueueElement
        {
            public string fileName;
            public string fileContent;
            public DateTime timestamp;
            public string filePath;
        }
  
        private readonly List<QueueElement> elements = new List<QueueElement>();
        private readonly int maxCapacity = 50;
        
        public void Enqueue(QueueElement element)
        {
            if (elements.Count >= maxCapacity)
            {
                Dequeue();
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

        public bool IsEmpty()
        {
            return elements.Count == 0;
        }
      

        public List<QueueElement> GetAll()
        {
            return new List<QueueElement>(elements);
        }

     
    }
}
