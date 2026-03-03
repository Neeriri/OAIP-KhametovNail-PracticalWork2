using System;

public class DynamicContainer<T>
{
    private T[] data;
    private int size;
    private int capacity;

    public DynamicContainer(int initialCapacity = 4)
    {
        capacity = initialCapacity;
        data = new T[capacity];
        size = 0;
    }

    public int Count => size;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException(nameof(index));
            return data[index];
        }
        set
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException(nameof(index));
            data[index] = value;
        }
    }

    public void Add(T item)
    {
        if (size == capacity)
        {
            capacity *= 2;
            T[] newData = new T[capacity];
            Array.Copy(data, newData, size);
            data = newData;
        }
        data[size++] = item;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));

        for (int i = index; i < size - 1; i++)
            data[i] = data[i + 1];
        
        size--;
        data[size] = default;
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (size == capacity)
        {
            capacity *= 2;
            T[] newData = new T[capacity];
            Array.Copy(data, 0, newData, 0, index);
            newData[index] = item;
            Array.Copy(data, index, newData, index + 1, size - index);
            data = newData;
        }
        else
        {
            for (int i = size; i > index; i--)
                data[i] = data[i - 1];
            data[index] = item;
        }
        size++;
    }
}

class Program
{
    static void Main()
    {
        var container = new DynamicContainer<int>();

        Console.WriteLine("=== Тест 1: Добавление 1000 элементов ===");
        for (int i = 1; i <= 1000; i++)
        {
            container.Add(i * 10);
        }
        Console.WriteLine($"Успешно добавлено элементов: {container.Count}");

        Console.WriteLine($"Элемент [0]: {container[0]}");
        Console.WriteLine($"Элемент [500]: {container[500]}");
        Console.WriteLine($"Элемент [999]: {container[999]}");

        Console.WriteLine("\n=== Тест 2: Вставка и Удаление ===");
        container.Insert(0, 999);
        Console.WriteLine($"После вставки в начало элемент [0]: {container[0]}");
        
        container.RemoveAt(0);
        Console.WriteLine($"После удаления элемент [0]: {container[0]}");

        Console.WriteLine("\nРабота завершена успешно.");
    }
}
