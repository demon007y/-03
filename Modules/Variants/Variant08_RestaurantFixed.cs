namespace ReviewSamples.Modules.Variants;

// Исправленная версия варианта 8: ресторан и расчёт заказа.
public sealed class Variant08_DishFixed
{
    public string Name { get; }
    public decimal Price { get; }

    public Variant08_DishFixed(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название блюда не может быть пустым.", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Цена не может быть отрицательной.");

        Name = name;
        Price = price;
    }
}

public sealed record Variant08_OrderItemFixed(Variant08_DishFixed Dish, int Quantity);

public sealed class Variant08_OrderFixed
{
    private readonly List<Variant08_OrderItemFixed> _items = new();

    public int TableNumber { get; }
    public IReadOnlyList<Variant08_OrderItemFixed> Items => _items.AsReadOnly();

    public Variant08_OrderFixed(int tableNumber)
    {
        if (tableNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(tableNumber));
        TableNumber = tableNumber;
    }

    internal void AddItem(Variant08_OrderItemFixed item) => _items.Add(item);
}

public sealed class Variant08_RestaurantFixed
{
    private const decimal ServiceChargeRate = 0.10m;

    public void AddDish(Variant08_OrderFixed order, Variant08_DishFixed dish, int quantity)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(dish);
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть больше нуля.");

        order.AddItem(new Variant08_OrderItemFixed(dish, quantity));
    }

    public decimal CalculateTotal(Variant08_OrderFixed order)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (order.Items.Count == 0)
            throw new InvalidOperationException("Нельзя рассчитать пустой заказ.");

        decimal subtotal = 0m;
        foreach (var item in order.Items)
            subtotal += item.Dish.Price * item.Quantity;

        return decimal.Round(subtotal * (1m + ServiceChargeRate), 2, MidpointRounding.AwayFromZero);
    }
}
