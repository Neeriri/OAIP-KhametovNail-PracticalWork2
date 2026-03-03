using System;

public class Athlete
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Sport { get; set; }
    public double Result { get; set; }

    public Athlete(int id, string name, string sport, double result)
    {
        Id = id;
        Name = name;
        Sport = sport;
        Result = result;
    }

    public override string ToString()
    {
        return $"№{Id}: {Name} ({Sport}) - Результат: {Result:F2}";
    }
}

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
        var container = new DynamicContainer<Athlete>();
        for (int i = 1; i <= 1000; i++)
        {
            var athlete = new Athlete(i, $"Спортсмен {i}", "Легкая атлетика", i * 1.5);
            container.Add(athlete);
        }

        Console.WriteLine("Спортшкола: Добавление 1000 спортсменов ");
    
        Console.WriteLine($"Успешно добавлено спортсменов: {container.Count}");

        Console.WriteLine($"Спортсмен [0]: {container[0]}");
        Console.WriteLine($"Спортсмен [500]: {container[500]}");
        Console.WriteLine($"Спортсмен [999]: {container[999]}");

        Console.WriteLine("\nСпортшкола: Вставка и Удаление");
        var topAthlete = new Athlete(0, "Чемпион", "Плавание", 99.9);
        container.Insert(0, topAthlete);
        Console.WriteLine($"После вставки лидера элемент [0]: {container[0]}");
        
        container.RemoveAt(0);
        Console.WriteLine($"После удаления лидера элемент [0]: {container[0]}");

        Console.WriteLine("\nРабота завершена успешно.");
    }
}
