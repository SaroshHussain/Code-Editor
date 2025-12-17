using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Code_Editor
{
    internal class CustomStack
    {
        private int top = -1; 
        internal struct StackElement
        {
            public string fileName;
            public string fileContent;
        }

        private readonly List<StackElement> elements = new List<StackElement>();
        public void Push(StackElement element)
        {
            elements.Add(element);
            top++;
        }
        public StackElement Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty.");
            }
            StackElement element = elements[top];
            elements.RemoveAt(top);
            top--;
            return element;
        }
        public StackElement Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty.");
            }
            return elements[top];
        }
        public bool IsEmpty()
        {
            return top == -1;
        }
        public bool ContainsKey(string fileName)
        {
            return elements.Any(e => e.fileName == fileName);
        }

        public string Get(string fileName)
        {
            var element = elements.FirstOrDefault(e => e.fileName == fileName);
            if (element.fileName == null)
            {
                throw new KeyNotFoundException($"'{fileName}' not found.");
            }
            return element.fileContent;
        }

        public void Set(string fileName, string fileContent)
        {
            int index = elements.FindIndex(e => e.fileName == fileName);
            if (index >= 0)
            {
                var element = elements[index];
                element.fileContent = fileContent;
                elements[index] = element;
            }
            else
            {
                Push(new StackElement { fileName = fileName, fileContent = fileContent });
            }
        }
    }

}
