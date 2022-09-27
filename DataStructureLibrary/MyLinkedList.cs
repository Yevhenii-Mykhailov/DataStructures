using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DataStructureLibrary
{
    public class Node<T>
    {
        public T Value { get; set; }
        public Node<T> Next { get; set; }
    }

    public class MyLinkedList<T> : IMyList<T> where T : IComparable<T>
    {
        private Node<T> _root;
        private int _count;

        public int Length => _count;

        public MyLinkedList()
        {
        }

        private Node<T> Insert(Node<T> root, T item)
        {
            Node<T> temp = new Node<T>();
            Node<T> ptr;
            temp.Value = item;
            temp.Next = null;

            if (root == null)
                root = temp;
            else
            {
                ptr = root;
                while (ptr.Next != null)
                {
                    ptr = ptr.Next;
                }
                ptr.Next = temp;

            }
            return root;
        }

        public MyLinkedList(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentException();
            }

            int lenght = 0;
            foreach (var item in items)
            {
                AddBack(item);
                lenght++;
                _count = lenght;
            }
        }

        //todo set, case then get find it a wrong way
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                {
                    throw new IndexOutOfRangeException();
                }

                var temp = _root;
                int i = 0;
                while (i < index)
                {
                    temp = temp.Next;
                    i++;
                }

                return temp.Value;
            }
            set
            {
                if (index < 0 || index >= Length)
                {
                    throw new IndexOutOfRangeException();
                }

                var temp = _root;
                int i = 0;
                while (i < index)
                {
                    temp = temp.Next;
                    i++;
                }

                temp.Value = value;
                _root = temp;

            }
        }

        public void AddBack(T itemToAdd)
        {
            if (_root != null)
            {
                Node<T> temp = _root;
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }

                temp.Next = new Node<T> { Value = itemToAdd };
            }
            else
            {
                _root = new Node<T> { Value = itemToAdd };
            }
        }

        public void AddBack(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (_root != null)
                {
                    AddBack(item);
                }
                else
                {
                    _root = new Node<T> { Value = item };
                }
            }
        }

        public void AddByIndex(int index, T itemToAdd)
        {
            var array = CreateArrayFromLinkedList();
            T[] newArray = new T[_count + 1];
            for (int i = 0; i < index; i++)
            {
                newArray[i] = array[i];
            }

            for (int i = index + 1; i < newArray.Length; i++)
            {
                newArray[i] = array[i - 1];
            }

            newArray[index] = itemToAdd;
            array = newArray;
            _root = CreateLinkedListFromArray(array);
        }

        public void AddByIndex(int index, IEnumerable<T> items)
        {
            int localCount = 0;
            var array = CreateArrayFromLinkedList();

            foreach (T item in items)
            {
                localCount++;
            }

            T[] newArray = new T[array.Length + localCount];
            int i = 0;

            for (int j = 0; j < index; j++)
            {
                newArray[j] = array[j];
                i++;
            }

            foreach (T item in items)
            {
                newArray[i++] = item;
                _count++;
            }

            for (int j = i; j < _count; j++)
            {
                newArray[j] = array[index];
                index++;
            }

            array = newArray;
            _root = CreateLinkedListFromArray(array);
        }

        public void AddFront(T item)
        {
            if (_root != null)
            {
                Node<T> temp = new Node<T> { Value = item, Next = _root };
                _root = temp;
            }
            else
            {
                _root = new Node<T> { Value = item };
            }
        }

        public void AddFront(IEnumerable<T> items)
        {
            AddByIndex(0, items);
        }

        public int IndexOf(T element)
        {
            var sourceArray = CreateArrayFromLinkedList();
            int item = -1;
            for (int i = 0; i < sourceArray.Length; i++)
            {
                if (element.CompareTo(sourceArray[i]) == 0)
                {
                    item = i;
                    break;
                }
            }

            if (item == -1)
            {
                throw new IndexOutOfRangeException();
            }

            return item;
        }

        public T Max()
        {
            var sourceArray = CreateArrayFromLinkedList();
            return sourceArray[MaxIndex()];
        }

        public int MaxIndex()
        {
            int maxIndex = 0;
            var sourceArray = CreateArrayFromLinkedList();

            for (int i = 1; i < sourceArray.Length; i++)
            {
                if (sourceArray[i].CompareTo(sourceArray[maxIndex]) == 1)
                {
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public T Min()
        {
            var sourceArray = CreateArrayFromLinkedList();
            return sourceArray[MinIndex()];
        }

        public int MinIndex()
        {
            int minIndex = 0;
            var sourceArray = CreateArrayFromLinkedList();

            for (int i = 1; i < sourceArray.Length; i++)
            {
                if (sourceArray[i].CompareTo(sourceArray[minIndex]) == -1)
                {
                    minIndex = i;
                }
            }

            return minIndex;
        }

        public T RemoveBack()
        {
            T result = default;
            Node<T> last = default;

            if (_root != null)
            {
                var temp = _root;
                if (_root.Next == null)
                {
                    result = _root.Value;
                    _root = null;
                }
                else
                {
                    while (temp.Next != null)
                    {
                        last = temp;
                        temp = temp.Next;
                    }
                    result = temp.Value;
                    last.Next = null;
                }
            }
            else
            {
                throw new ArgumentException();
            }

            _count--;
            return result;
        }

        public T RemoveByIndex(int index)
        {
            var array = CreateArrayFromLinkedList();
            T[] newArray = new T[array.Length - 1];
            T item = array[index];

            for (int i = 0; i < index; i++)
            {
                newArray[i] = array[i];
            }

            _count--;

            for (int i = index; i < _count; i++)
            {
                newArray[i] = array[i + 1];
            }

            array = newArray;
            _root = CreateLinkedListFromArray(array);

            return item;
        }

        public int RemoveByValue(T value)
        {
            int localCount = 0;
            bool success = false;

            if (_root.Value.CompareTo(value) == 0)
            {
                _root = _root.Next;
                return localCount;
            }

            var temp = _root;
            while (temp.Next != null)
            {
                localCount++;
                if (temp.Next.Value.CompareTo(value) == 0)
                {
                    temp.Next = temp.Next.Next;
                    success = true;
                    break;
                }

                temp = temp.Next;
            }

            if (!success)
            {
                localCount = -1;
            }

            return localCount;
        }

        public int RemoveByValueAll(T value)
        {
            int localCount = 0;

            var temp = _root;

            do
            {
                if (RemoveByValue(value) != -1)
                {
                    localCount++;
                }

                temp = temp.Next;
            } while (temp.Next != null);

            if (temp.Value.CompareTo(value) == 0)
            {
                localCount++;
                RemoveBack();
            }

            return localCount;
        }

        public T RemoveFront()
        {
            var temp = _root;
            var value = temp.Value;

            if (_root != null)
            {
                _root = _root.Next;
                temp = null;
            }

            return value;
        }

        //todo
        public T[] RemoveNValuesBack(int n)
        {
            throw new NotImplementedException();
        }

        //todo
        public T[] RemoveNValuesByIndex(int index, int n)
        {
            var array = CreateArrayFromLinkedList();
            int localCount = 0;
            T[] newArray = new T[array.Length];
            T[] deletedArray = new T[n];

            do
            {
                if (n > 0)
                {
                    deletedArray[localCount] = array[index];

                    for (int i = 0; i < index; i++)
                    {
                        newArray[i] = array[i];
                    }

                    for (int i = index; i < _count - 1; i++)
                    {
                        newArray[i] = array[i + 1];
                    }

                    localCount++;
                    _count--;
                    array = newArray;
                }

            } while (localCount < n);

            _root = CreateLinkedListFromArray(array);

            return deletedArray;
        }

        //todo
        public T[] RemoveNValuesFront(int n)
        {
            throw new NotImplementedException();
        }

        public void Reverse()
        {
            Node<T> prev = null, current = _root, next = null;
            while (current != null)
            {
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }

            _root = prev;
        }

        public void Sort(bool ascending = true)
        {
            var array = CreateArrayFromLinkedList();

            if (ascending == true)
            {
                for (int i = 1; i < _count; i++)
                {
                    T value = array[i];
                    for (int j = i - 1; j >= 0;)
                    {
                        if (value.CompareTo(array[j]) == -1)
                        {
                            array[j + 1] = array[j];
                            j--;
                            array[j + 1] = value;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _count - 1; i++)
                {
                    int smalestIndex = i;
                    for (int j = i + 1; j < _count; j++)
                    {
                        if (array[j].CompareTo(array[smalestIndex]) == 1)
                        {
                            smalestIndex = j;
                        }
                    }

                    Swap(ref array[smalestIndex], ref array[i]);
                }
            }

            var result = CreateLinkedListFromArray(array);
            _root = result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> temp = _root;
            while (temp != null)
            {
                yield return temp.Value;
                temp = temp.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private T[] CreateArrayFromLinkedList()
        {
            T[] array = new T[_count];

            Node<T> temp = _root;
            if (_root != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = temp.Value;
                    temp = temp.Next;
                }
            }

            return array;
        }

        private Node<T> CreateLinkedListFromArray(T[] array)
        {
            Node<T> root = null;
            for (int i = 0; i < array.Length; i++)
            {
                root = Insert(root, array[i]);
            }
                
            return root;
        }

        private void Swap (ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}

