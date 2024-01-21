using System;

using UnityEngine;

using System.Collections;


public class Heap<T> where T : IHeapItem<T>
{
	T[] items;

	int counter;

	public Heap(int heapMaxSize)
	{
		items = new T[heapMaxSize];
	}

	public void Add(T item)
	{
		item.HeapIndex = counter;

		items[counter] = item;
		
        SortUp(item);
		
        counter++;
	}

	public T RemoveFirst()
	{
		T firstItem = elements[0];

		counter--;
		
        elements[0] = elements[counter];
		
        elements[0].HeapIndex = 0;
		
        SortDown(elements[0]);
		
        return firstItem;
	}


	public int Count
	{
		get
		{
			return counter;
		}
	}

	public bool Contains(T item)
	{
		return Equals(items[item.HeapIndex], item);
	}

	void SortUp(T item)
	{
		int parentIndex = (item.HeapIndex - 1) / 2;

		while (true)
		{
			T parentItem = items[parentIndex];

			if (item.CompareTo(parentItem) > 0)
			{
				Swap(item, parentItem);
			}
			else
			{
				break;
			}

			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	void SortDown(T item)
	{
		while (true)
		{
			int leftChild = item.HeapIndex * 2 + 1;

			int rightChild = item.HeapIndex * 2 + 2;

			int swapIndex = 0;

			if (leftChild < counter)
			{
				swapIndex = leftChild;

				if (rightChild < counter)
				{
					if (items[leftChild].CompareTo(items[rightChild]) < 0)
					{
						swapIndex = rightChild;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0)
				{
					Swap(item, items[swapIndex]);
				}
				else
				{
					return;
				}
			}
			else
			{
				return;
			}
		}
	}


    public void UpdateItem(T item)
	{
		SortUp(item);
	}

	void Swap(T itemA, T itemB)
	{
		items[itemA.HeapIndex] = itemB;

		items[itemB.HeapIndex] = itemA;
		
        int itemAIndex = itemA.HeapIndex;
		
        itemA.HeapIndex = itemB.HeapIndex;
		
        itemB.HeapIndex = itemAIndex;
	}

}

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex
	{
		get;

		set;
	}
}
