using System;

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex { get; set; }
}

public class Heap<T> where T : IHeapItem<T>
{
	T[] _items;
	public int Count { get; private set; }

	public Heap (int maxHeapSize)
	{
		_items = new T[maxHeapSize];
	}

	public void UpdateItem (T item)
	{
		SortUp (item);
	}

	public bool Contains (T item)
	{
		return Equals (_items[item.HeapIndex], item);
	}

	public void Add (T item)
	{
		item.HeapIndex = Count;
		_items[Count] = item;
		SortUp (item);
		Count++;
	}

	public T RemoveFirst ()
	{
		T firstItem = _items[0];
		Count--;
		_items[0] = _items[Count];
		_items[0].HeapIndex = 0;
		SortDown (_items[0]);
		return firstItem;
	}

	void SortDown (T item)
	{
		while (true)
		{
			int leftChildIndex = item.HeapIndex * 2 + 1;
			int rightChildIndex = leftChildIndex + 1;
			int swapIndex = 0;

			if (leftChildIndex < Count)
			{
				swapIndex = leftChildIndex;
				if (rightChildIndex < Count)
					if (_items[rightChildIndex].CompareTo (_items[leftChildIndex]) < 0)
						swapIndex = rightChildIndex;

				if (item.CompareTo (_items[swapIndex]) < 0)
					Swap (item, _items[swapIndex]);
				else return;
			}
			else return;
		}
	}

	void SortUp (T item)
	{
		int parentIndex = (item.HeapIndex - 1) / 2;
		while (true)
		{
			T parentItem = _items[parentIndex];
			if (item.CompareTo (parentItem) > 0)
			{
				Swap (item, parentItem);
				parentIndex = (item.HeapIndex - 1) / 2;
				continue;
			}

			break;
		}
	}

	void Swap (T a, T b)
	{
		_items[a.HeapIndex] = b;
		_items[b.HeapIndex] = a;
		int aIndex = a.HeapIndex;
		a.HeapIndex = b.HeapIndex;
		b.HeapIndex = aIndex;
	}
}