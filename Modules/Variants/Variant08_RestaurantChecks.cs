namespace ReviewSamples.Modules.Variants;

// Простые автоматические проверки без сторонних пакетов.
// Запуск: dotnet run -- test-8
public static class Variant08_RestaurantChecks
{
    private static int _passed;

    public static void RunAll()
    {
        _passed = 0;
        Check("Две позиции заказа: 1320.00", () =>
        {
            var service = new Variant08_RestaurantFixed();
            var order = new Variant08_OrderFixed(1);
            service.AddDish(order, new Variant08_DishFixed("Пицца", 500m), 2);
            service.AddDish(order, new Variant08_DishFixed("Сок", 200m), 1);
            AssertTotal(1320m, service.CalculateTotal(order));
        });

        Check("Пустой заказ: ошибка", () =>
        {
            var service = new Variant08_RestaurantFixed();
            ExpectThrows<InvalidOperationException>(() =>
                service.CalculateTotal(new Variant08_OrderFixed(1)));
        });

        Check("Отрицательное количество: ошибка", () =>
        {
            var service = new Variant08_RestaurantFixed();
            ExpectThrows<ArgumentOutOfRangeException>(() =>
                service.AddDish(new Variant08_OrderFixed(1), new Variant08_DishFixed("Суп", 500m), -2));
        });

        Check("Нулевое количество: ошибка", () =>
        {
            var service = new Variant08_RestaurantFixed();
            ExpectThrows<ArgumentOutOfRangeException>(() =>
                service.AddDish(new Variant08_OrderFixed(1), new Variant08_DishFixed("Суп", 500m), 0));
        });

        Check("Отрицательная цена: ошибка", () =>
            ExpectThrows<ArgumentOutOfRangeException>(() => new Variant08_DishFixed("Суп", -100m)));

        Check("Цена 19.99: итог 21.99", () =>
        {
            var service = new Variant08_RestaurantFixed();
            var order = new Variant08_OrderFixed(1);
            service.AddDish(order, new Variant08_DishFixed("Чай", 19.99m), 1);
            AssertTotal(21.99m, service.CalculateTotal(order));
        });

        Check("100 x 3: итог 330.00", () =>
        {
            var service = new Variant08_RestaurantFixed();
            var order = new Variant08_OrderFixed(1);
            service.AddDish(order, new Variant08_DishFixed("Чай", 100m), 3);
            AssertTotal(330m, service.CalculateTotal(order));
        });

        Check("Не задан заказ: ошибка", () =>
            ExpectThrows<ArgumentNullException>(() =>
                new Variant08_RestaurantFixed().CalculateTotal(null!)));

        Console.WriteLine($"Проверки варианта 8: {_passed}/8 успешно.");
    }

    private static void Check(string name, Action action)
    {
        action();
        _passed++;
        Console.WriteLine($"[OK] {name}");
    }

    private static void AssertTotal(decimal expected, decimal actual)
    {
        if (expected != actual)
            throw new InvalidOperationException($"Ожидалось {expected}, получено {actual}.");
    }

    private static void ExpectThrows<T>(Action action) where T : Exception
    {
        try
        {
            action();
        }
        catch (T)
        {
            return;
        }

        throw new InvalidOperationException($"Ожидалось исключение {typeof(T).Name}.");
    }
}
