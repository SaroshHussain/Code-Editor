using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Code_Editor
{
    internal class CustomLinkedList
    {
        internal struct FileNode
        {
            public string fileName;
            public string fileContent;
            public string filePath;
        }

        private LinkedList<FileNode> elements = new LinkedList<FileNode>();

        public void Push(string fileName, string fileContent, string filePath = "")
        {
            FileNode file;
            file.fileContent = fileContent;
            file.filePath = filePath;
            file.fileName = fileName;

            elements.AddLast(file);
        }

        public FileNode Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("LinkedList is empty.");
            }

            FileNode result = elements.Last.Value;
            elements.RemoveLast();
            return result;
        }

        public FileNode Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("LinkedList is empty.");
            }
            return elements.Last.Value;
        }

        public bool IsEmpty()
        {
            return elements.Count == 0;
        }
        
        public bool ContainsKey(string fileName)
        {
            var current = elements.First;
            while (current != null)
            {
                if(current.Value.fileName == fileName)
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public string Get(string fileName)
        {
            var node = elements.FirstOrDefault(e => e.fileName == fileName);
            if (node.fileName == null)
            {
                throw new KeyNotFoundException($"'{fileName}' not found.");
            }
            return node.fileContent;
        }

        public void Set(string fileName, string fileContent, string filePath = "")
        {
            var linkedNode = elements.FirstOrDefault(e => e.fileName == fileName);
            
            if (linkedNode.fileName != null)
            {
                var node = elements.Find(linkedNode);
                if (node != null)
                {
                    elements.Remove(node);
                    elements.AddLast(new FileNode
                    {
                        fileName = linkedNode.fileName,
                        fileContent = fileContent,
                        filePath = string.IsNullOrEmpty(filePath) ? linkedNode.filePath : filePath
                    });
                }
            }
            else
            {
                Push(fileName, fileContent, filePath);
            }
        }

        public void Remove(string fileName)
        {
            var node = elements.FirstOrDefault(e => e.fileName == fileName);
            if (node.fileName == null)
            {
                throw new KeyNotFoundException($"'{fileName}' not found.");
            }

            var linkedNode = elements.Find(node);
            if (linkedNode != null)
            {
                elements.Remove(linkedNode);
            }
        }

        public List<FileNode> GetAll()
        {
            return new List<FileNode>(elements);
        }

        public int Count
        {
            get { return elements.Count; }
        }
    }
}
