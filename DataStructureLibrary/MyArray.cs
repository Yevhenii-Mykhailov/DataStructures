using System;
using System.Collections.Generic;

namespace DataStructureLibrary
{
    public class MyArray : IMyList
    {
        private const int DefaultSize = 4;
        private const double Coef = 1.3;
        private int _count;
        private int[] _array;

        public int Capacity => _array.Length;
        public int Length => _count;

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                {
                    throw new IndexOutOfRangeException();
                }

                return _array[index];
            }
            set
            {
                if (index < 0 || index >= Length)
                {
                    throw new IndexOutOfRangeException();
                }

                _array[index] = value;
            }
        }

        public MyArray() : this(DefaultSize)
        {
        }

        public MyArray(int size)
        {
            if (size < 0 )
            {
                throw new ArgumentException();
            }

            size = size > DefaultSize ? (int)(size * Coef) : DefaultSize;
            _array = new int[size];
        }

        public MyArray(int[] array)
        {
            if (array == null)
            {
                throw new ArgumentException();
            }

            int size = array.Length > DefaultSize ? (int)(array.Length * Coef) : DefaultSize;
            _array = new int[size];

            for (int i = 0; i < array.Length; i++)
            {
                _array[i] = array[i];
            }

            _count = array.Length;
        }

        public int[] ToArray()
        {
            int[] result = new int[Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = _array[i];
            }

            return result;
        }

        public void AddBack(int itemToAdd)
        {
            ResizeArray();

            AddByIndex(_count, itemToAdd);
        }

        public void AddFront(int itemToAdd)
        {
            ResizeArray();

            AddByIndex(0, itemToAdd);
        }

        public void AddByIndex(int index, int itemToAdd)
        {
            ResizeArray();

            int[] newArray = new int[_array.Length];
            for (int i = 0; i < index; i++)
            {
                newArray[i] = _array[i];
            }

            for (int i = index + 1; i < newArray.Length; i++)
            {
                newArray[i] = _array[i - 1];
            }

            newArray[index] = itemToAdd;
            _array = newArray;
            _count++;
        }

        public int RemoveBack()
        {
            ResizeArray();

            return RemoveByIndex(_count);
        }

        public int RemoveFront()
        {
            ResizeArray();

            int[] newArray = new int[_array.Length];
            for (int i = 1; i < _count; i++)
            {
                newArray[i - 1] = _array[i];
            }

            int firstItem = _array[0];
            _array = newArray;
            _count--;
            return firstItem;
        }

        public int RemoveByIndex(int index)
        {
            ResizeArray();

            int[] newArray = new int[_array.Length];
            int item = _array[index];

            if (index != 0)
            {
                for (int i = 0; i < index; i++)
                {
                    newArray[i] = _array[i];
                }

                for (int i = index; i < _count; i++)
                {
                    newArray[i] = _array[i + 1];
                }

                _array = newArray;
                _count--;
            }
            else
            {
                RemoveFront();
            }

            return item;
        }

        public int[] RemoveNValuesBack(int n)
        {
            ResizeArray();

            int[] newArray = new int[_array.Length];
            int[] deletedArray = new int[n];
            int localCount = _count;
            int deletedCount = n;

            for (int i = 0; i < n; i++)
            {
                deletedArray[deletedCount - 1] = _array[localCount - 1];
                deletedCount--;
                localCount--;
                _count--;
            }

            for (int i = 0; i < _count; i++)
            {
                if (localCount > 0)
                {
                    newArray[i] = _array[i];
                    localCount--;
                }
            }

            _array = newArray;
            return deletedArray;
        }
        
        public int[] RemoveNValuesFront(int n)
        {
            ResizeArray();

            int[] newArray = new int[_array.Length];
            int[] deletedArray = new int[n];
            int localCount = _count;
            int deletedCount = n;

            for (int i = 0; i < n; i++)
            {
                deletedArray[i] = _array[i];
                deletedCount--;
                localCount--;
                _count--;
            }

            for (int i = 0; i < _count; i++)
            {
                if (localCount > 0)
                {
                    newArray[i] = _array[_array.Length - localCount - 1];
                    localCount--;
                }
            }

            _array = newArray;
            return deletedArray;
        }

        //todo reuse for nValuesFront/nValuesBack
        public int[] RemoveNValuesByIndex(int index, int n)
        {
            ResizeArray();

            int localCount = 0;
            int[] newArray = new int[_array.Length];
            int[] deletedArray = new int[n];

            for (int i = 0; i < n; i++)
            {
                deletedArray[i] = _array[index + localCount];
                localCount++;
                _count--;
            }

            for (int i = 0; i < _count; i++)
            {
                if (n != 1)
                {
                    newArray[i] = _array[i];
                    _array[i] = newArray[i];
                }
                else
                {
                    RemoveByIndex(index);
                    _count++;
                    break;
                }
            }

            return deletedArray;
        }

        public int IndexOf(int element)
        {
            ResizeArray();

            int item = -1;
            for (int i = 0; i < _array.Length; i++)
            {
                if (element == _array[i])
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

        public void Reverse()
        {
            ResizeArray();

            int count = 1;
            int[] newArray = new int[_array.Length];

            for (int i = 0; i < _count; i++)
            {
                newArray[i] = _array[_count - count];
                count++;
            }

            _array = newArray;
        }

        public int Max()
        {
            return _array[MaxIndex()];
        }

        public int Min()
        {
            return _array[MinIndex()];
        }

        public int MaxIndex()
        {
            ResizeArray();

            int maxIndex = 0;

            for (int i = 1; i < _count; i++)
            {
                if (_array[i] > _array[maxIndex])
                {
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public int MinIndex()
        {
            if (_array == null)
            {
                throw new ArgumentException();
            }

            ResizeArray();

            int minIndex = 0;

            for (int i = 1; i < _count; i++)
            {
                if (_array[i] < _array[minIndex])
                {
                    minIndex = i;
                }
            }

            return minIndex;
        }

        public void Sort(bool ascending = true)
        {
            ResizeArray();
            if (ascending == true)
            {
                for (int i = 1; i < _count; i++)
                {
                    int value = _array[i];
                    for (int j = i - 1; j >= 0;)
                    {
                        if (value < _array[j])
                        {
                            _array[j + 1] = _array[j];
                            j--;
                            _array[j + 1] = value;
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
                        if (_array[j] > _array[smalestIndex])
                        {
                            smalestIndex = j;
                        }
                    }

                    Swap(ref _array[smalestIndex], ref _array[i]);
                }
            }

        }

        public int RemoveByValue(int value)
        {
            ResizeArray();

            int[] newArray = new int[_array.Length];
            int removedValueIndex = -1;

            for (int i = 0; i < _count; i++)
            {
                if (value == _array[i])
                {
                    removedValueIndex = i;

                    for (int j = 0; j < removedValueIndex; j++)
                    {
                        newArray[j] = _array[j];
                    }

                    for (int j = removedValueIndex; j < _count; j++)
                    {
                        newArray[j] = _array[j + 1];
                    }

                    _array = newArray;
                    _count--;
                    break;
                }
            }
            
            return removedValueIndex;
        }

        public int RemoveByValueAll(int value)
        {
            ResizeArray();

            int[] newArray = new int[_array.Length];
            int countOfRemovedNumbers = 0;
            int elementIndex;

            for (int i = 0; i < _count; i++)
            {
                if (value == _array[i])
                {
                    elementIndex = i;

                    for (int j = 0; j < elementIndex; j++)
                    {
                        newArray[j] = _array[j];
                    }

                    for (int j = elementIndex; j < _count; j++)
                    {
                        newArray[j] = _array[j + 1];
                    }

                    _array = newArray;
                    i--;
                    countOfRemovedNumbers++;
                    _count--;
                }
                else if (_array[0] == value)
                {
                    countOfRemovedNumbers++;
                    break;
                }
            }

            return countOfRemovedNumbers;

        }

        public void AddFront(IEnumerable<int> items)
        {
            ResizeArray();

            AddByIndex(0, items);
        }

        public void AddBack(IEnumerable<int> items)
        {
            ResizeArray();

            AddByIndex(_count, items);
        }

        public void AddByIndex(int index, IEnumerable<int> items)
        {
            int localCount = 0;

            foreach (int item in items)
            {
                localCount++;
            }

            int[] newArray = new int[_array.Length + localCount];
            int i = 0;

            for (int j = 0; j < index; j++)
            {
                newArray[j] = _array[j];
                i++;
            }

            foreach (int item in items)
            {
                newArray[i++] = item;
                _count++;
            }

            for (int j = i; j < _count; j++)
            {
                newArray[j] = _array[index];
                index++;
            }

            _array = newArray;
        }

        private void ResizeArray()
        {
            if (Capacity == Length)
            {
                int[] newArray = new int[(int)(_array.Length * Coef)];
                for (int i = 0; i < _array.Length; i++)
                {
                    newArray[i] = _array[i];
                }

                _array = newArray;
            }
        }

        private void Swap (ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}