namespace ClinicApp.Models;

public class WaitingQueue<T>
{
    private Queue<T> _items;

    public int Count
    {
        get { return _items.Count; }
    }

    public bool IsEmpty
    {
        get { return _items.Count == 0; }
    }

    public WaitingQueue()
    {
        _items = new Queue<T>();
    }

    public void Enqueue(T item)
    {
        _items.Enqueue(item);
    }

    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Черга порожня.");

        return _items.Dequeue();
    }

    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Черга порожня.");

        return _items.Peek();
    }

    public T[] ToArray()
    {
        return _items.ToArray();
    }
}
