using System;
using System.Threading.Tasks;
using StockExchangeSystem;

class StockDemo
{
    static async Task Main()
    {
        var exchange = new StockExchange();

        var traderAlice = new Trader("Alice");
        var traderBob = new Trader("Bob");

        var robot = new TradingRobot(buyBelow: 90, sellAbove: 150);

        exchange.Subscribe("AAPL", traderAlice);
        exchange.Subscribe("AAPL", robot);
        exchange.Subscribe("GOOG", traderBob);

        await exchange.SetStockPrice("AAPL", 80);   
        await exchange.SetStockPrice("AAPL", 160);  
        await exchange.SetStockPrice("GOOG", 200); 

        exchange.Unsubscribe("AAPL", traderAlice);
        await exchange.SetStockPrice("AAPL", 120); 
    }
}
