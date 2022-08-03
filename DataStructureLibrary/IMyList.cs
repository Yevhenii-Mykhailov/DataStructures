using System;
using System.Collections.Generic;

namespace DataStructureLibrary
{
    public interface IMyList<T>
    {
        void AddBack(int itemToAdd);
        void AddFront(int itemToAdd);
        void AddByIndex(int index, int itemToAdd);
        int RemoveBack();
        int RemoveFront();
        int RemoveByIndex(int index);
        int[] RemoveNValuesBack(int n);
        int[] RemoveNValuesFront(int n);
        int[] RemoveNValuesByIndex(int index, int n);
        int Length { get; }
        int this[int index] { get; set; }
        int IndexOf(int element);
        void Reverse();
        int Max();
        int Min();
        int MaxIndex();
        int MinIndex();
        void Sort(bool ascending = true);
        int RemoveByValue(int value);
        int RemoveByValueAll(int value);
        void AddFront(IEnumerable<int> items);
        void AddBack(IEnumerable<int> items);
        void AddByIndex(int index, IEnumerable<int> items);
    }
}