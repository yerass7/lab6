using System;
using TravelBookingSystem;

class TravelDemo
{
    static void Main()
    {
        var request = new TravelRequest
        {
            DistanceKm = 1200,
            Passengers = 2,
            ServiceClass = ServiceClass.Business,
            ChildDiscount = true,
            SeniorDiscount = false,
            BaggageCount = 1
        };

        var context = new TravelBookingContext();


        context.SetStrategy(new PlaneStrategy());
        decimal price = context.CalculatePrice(request);

        Console.WriteLine("Travel cost = " + price);
    }
}
