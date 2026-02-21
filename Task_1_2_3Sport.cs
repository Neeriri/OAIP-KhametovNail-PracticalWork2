using System;

public class Athlete
{
    public int Number { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Sport { get; set; }
    public double Result { get; set; }

    public Athlete(int number, string firstName, string lastName, int age, string sport, double result)
    {
        Number = number;
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Sport = sport;
        Result = result;
    }

    public override string ToString()
    {
        return $"{Number}. {FirstName} {LastName}, {Age} лет, {Sport}: {Result:F2}";
    }
}

public class DynamicContainer<T>
{
    private T[] _data;
    
    private int _size;
    private int _capacity;

    public DynamicContainer(int initialCapacity = 4)
    {
        if (initialCapacity < 1)
            throw new ArgumentException("Первая вместимост небольше одного 1.");
        _capacity = initialCapacity;
        _data = new T[_capacity];
        _size = 0;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _data[index];
        }
        set
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException(nameof(index));
            _data[index] = value;
        }
    }

    public int Count => _size;

    public void Add(T item)
    {
        if (_size == _capacity)
        {
            _capacity *= 2;
            T[] newData = new T[_capacity];
            Array.Copy(_data, newData, _size);
            _data = newData;
        }
        _data[_size++] = item;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _size)
            throw new ArgumentOutOfRangeException(nameof(index));
        for (int i = index; i < _size - 1; i++)
            _data[i] = _data[i + 1];
        _size--;
        _data[_size] = default!;
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > _size)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (_size == _capacity)
        {
            _capacity *= 2;
            T[] newData = new T[_capacity];
            Array.Copy(_data, newData, index);
            newData[index] = item;
            Array.Copy(_data, index, newData, index + 1, _size - index);
            _data = newData;
        }
        else
        {
            for (int i = _size; i > index; i--)
                _data[i] = _data[i - 1];
            _data[index] = item;
        }
        _size++;
    }
}

class Program
{
    static void Main()
    {
        var container = new DynamicContainer<Athlete>();

        Console.Write("Сколько спортсменов вы хотите добавить? ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Некорректное количество.");
            return;
        }

        for (int i = 1; i <= count; i++)
        {
            Console.WriteLine($"\n--- Ввод данных спортсмена #{i} ---");

            Console.Write("Номер: ");
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Некорректный номер. Пропускаем.");
                continue;
            }

            Console.Write("Имя: ");
            string firstName = Console.ReadLine() ?? "Неизвестно";

            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine() ?? "Неизвестно";

            Console.Write("Возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age) || age < 6 || age > 100)
            {
                Console.WriteLine("Некорректный возраст. Пропускаем.");
                continue;
            }

            Console.Write("Вид спорта: ");
            string sport = Console.ReadLine() ?? "Без спорта";

            Console.Write("Результат (число): ");
            if (!double.TryParse(Console.ReadLine(), out double result) || result < 0)
            {
                Console.WriteLine("Некорректный результат. Пропускаем.");
                continue;
            }

            var athlete = new Athlete(number, firstName, lastName, age, sport, result);
            container.Add(athlete);
        }

        Console.WriteLine($"\nВы добавили {container.Count} спортсменов:\n");

        for (int i = 0; i < container.Count; i++)
        {
            Console.WriteLine(container[i]);
        }

        try { var x = container[-1]; } catch (ArgumentOutOfRangeException) { Console.WriteLine("\nIndex -1 error"); }
        try { container.RemoveAt(container.Count); } catch (ArgumentOutOfRangeException) { Console.WriteLine("RemoveAt(Count) error"); }
        try { container.Insert(container.Count + 1, null!); } catch (ArgumentOutOfRangeException) { Console.WriteLine("Insert beyond end error"); }
    }
}
