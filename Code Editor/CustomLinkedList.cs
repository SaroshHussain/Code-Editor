using System;
using System.Collections.Generic;

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

        private class Node
        {
            public FileNode data;
            public Node next;

            public Node(FileNode data)
            {
                this.data = data;
                this.next = null;
            }
        }

        private Node head;
        private Node tail;
        private int count;

        public CustomLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void Push(string fileName, string fileContent, string filePath = "")
        {
            FileNode file = new FileNode
            {
                fileName = fileName,
                fileContent = fileContent,
                filePath = filePath
            };

            Node newNode = new Node(file);

            if (head == null)
            {
                head = tail = newNode;
            }
            else
            {
                tail.next = newNode;
                tail = newNode;
            }
            count++;
        }

        public FileNode Pop()
        {
            if (IsEmpty())
                throw new InvalidOperationException("LinkedList is empty.");

            FileNode result = tail.data;

            if (head == tail)
            {
                head = tail = null;
            }
            else
            {
                Node current = head;
                while (current.next != tail)
                {
                    current = current.next;
                }
                tail = current;
                tail.next = null;
            }
            count--;
            return result;
        }

        public FileNode Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("LinkedList is empty.");

            return tail.data;
        }

        public bool IsEmpty() => head == null;

        public bool ContainsKey(string fileName)
        {
            Node current = head;
            while (current != null)
            {
                if (current.data.fileName == fileName)
                    return true;
                current = current.next;
            }
            return false;
        }

        public string Get(string fileName)
        {
            Node current = head;
            while (current != null)
            {
                if (current.data.fileName == fileName)
                    return current.data.fileContent;
                current = current.next;
            }
            throw new KeyNotFoundException($"'{fileName}' not found.");
        }

        public void Set(string fileName, string fileContent, string filePath = "")
        {
            Node current = head;
            while (current != null)
            {
                if (current.data.fileName == fileName)
                {
                    current.data = new FileNode
                    {
                        fileName = fileName,
                        fileContent = fileContent,
                        filePath = string.IsNullOrEmpty(filePath) ? current.data.filePath : filePath
                    };
                    return;
                }
                current = current.next;
            }
            // If not found, add it
            Push(fileName, fileContent, filePath);
        }

        public void Remove(string fileName)
        {
            if (IsEmpty())
                throw new KeyNotFoundException($"'{fileName}' not found.");

            // If head node matches
            if (head.data.fileName == fileName)
            {
                head = head.next;
                if (head == null)
                    tail = null;
                count--;
                return;
            }

            // Search for the node
            Node current = head;
            while (current.next != null)
            {
                if (current.next.data.fileName == fileName)
                {
                    if (current.next == tail)
                        tail = current;
                    current.next = current.next.next;
                    count--;
                    return;
                }
                current = current.next;
            }
            throw new KeyNotFoundException($"'{fileName}' not found.");
        }

        public List<FileNode> GetAll()
        {
            List<FileNode> result = new List<FileNode>();
            Node current = head;
            while (current != null)
            {
                result.Add(current.data);
                current = current.next;
            }
            return result;
        }

        public int Count => count;
    }
}   