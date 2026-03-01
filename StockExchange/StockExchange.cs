using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeSystem
{
    // ===== OBSERVER =====
    public interface IObserver
    {
        Task Update(string stock, decimal price);
    }

    // ===== SUBJECT =====
    public interface ISubject
    {
        void Subscribe(string stock, IObserver observer);
        void Unsubscribe(string stock, IObserver observer);
        Task Notify(string stock, decimal price);
    }

    // ===== STOCK EXCHANGE =====
    public class StockExchange : ISubject
    {
        private readonly Dictionary<string, List<IObserver>> _subscribers = new();
        private readonly Dictionary<string, decimal> _stocks = new();

        public void Subscribe(string stock, IObserver observer)
        {
            if (!_subscribers.ContainsKey(stock))
                _subscribers[stock] = new List<IObserver>();

            _subscribers[stock].Add(observer);
            Console.WriteLine($"Subscribed: {observer.GetType().Name} -> {stock}");
        }

        public void Unsubscribe(string stock, IObserver observer)
        {
            if (_subscribers.ContainsKey(stock))
                _subscribers[stock].Remove(observer);
        }

        public async Task SetStockPrice(string stock, decimal price)
        {
            _stocks[stock] = price;
            Console.WriteLine($"Price update: {stock} = {price}");
            await Notify(stock, price);
        }

        public async Task Notify(string stock, decimal price)
        {
            if (!_subscribers.ContainsKey(stock)) return;

            var tasks = _subscribers[stock]
                .Select(observer => observer.Update(stock, price));

            await Task.WhenAll(tasks);
        }
    }

    // ===== TRADER =====
    public class Trader : IObserver
    {
        private readonly string _name;

        public Trader(string name)
        {
            _name = name;
        }

        public Task Update(string stock, decimal price)
        {
            Console.WriteLine($"Trader {_name}: {stock} = {price}");
            return Task.CompletedTask;
        }
    }

    // ===== TRADING ROBOT =====
    public class TradingRobot : IObserver
    {
        private readonly decimal _buyBelow;
        private readonly decimal _sellAbove;

        public TradingRobot(decimal buyBelow, decimal sellAbove)
        {
            _buyBelow = buyBelow;
            _sellAbove = sellAbove;
        }

        public Task Update(string stock, decimal price)
        {
            if (price < _buyBelow)
                Console.WriteLine($"ROBOT: Buy {stock}");

            if (price > _sellAbove)
                Console.WriteLine($"ROBOT: Sell {stock}");

            return Task.CompletedTask;
        }
    }
}
