using System;

namespace TravelBookingSystem
{
    // ===== ENUMS =====
    public enum TransportType
    {
        Plane,
        Train,
        Bus
    }

    public enum ServiceClass
    {
        Economy,
        Business
    }

    // ===== DATA MODEL =====
    public class TravelRequest
    {
        public double DistanceKm { get; set; }
        public int Passengers { get; set; }
        public ServiceClass ServiceClass { get; set; }
        public bool ChildDiscount { get; set; }
        public bool SeniorDiscount { get; set; }
        public int BaggageCount { get; set; }
    }

    // ===== STRATEGY INTERFACE =====
    public interface ICostCalculationStrategy
    {
        decimal CalculateCost(TravelRequest request);
    }

    // ===== PLANE STRATEGY =====
    public class PlaneStrategy : ICostCalculationStrategy
    {
        public decimal CalculateCost(TravelRequest request)
        {
            decimal cost = (decimal)request.DistanceKm * 0.12m;

            if (request.ServiceClass == ServiceClass.Business)
                cost *= 1.8m;

            cost += request.BaggageCount * 25;

            return ApplyDiscounts(cost, request);
        }

        private decimal ApplyDiscounts(decimal cost, TravelRequest request)
        {
            if (request.ChildDiscount) cost *= 0.8m;
            if (request.SeniorDiscount) cost *= 0.85m;

            return cost * request.Passengers;
        }
    }

    // ===== TRAIN STRATEGY =====
    public class TrainStrategy : ICostCalculationStrategy
    {
        public decimal CalculateCost(TravelRequest request)
        {
            decimal cost = (decimal)request.DistanceKm * 0.07m;

            if (request.ServiceClass == ServiceClass.Business)
                cost *= 1.4m;

            return cost * request.Passengers;
        }
    }

    // ===== BUS STRATEGY =====
    public class BusStrategy : ICostCalculationStrategy
    {
        public decimal CalculateCost(TravelRequest request)
        {
            decimal cost = (decimal)request.DistanceKm * 0.05m;
            return cost * request.Passengers;
        }
    }

    // ===== CONTEXT =====
    public class TravelBookingContext
    {
        private ICostCalculationStrategy _strategy;

        public void SetStrategy(ICostCalculationStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException("Strategy not selected");
        }

        public decimal CalculatePrice(TravelRequest request)
        {
            if (_strategy == null)
                throw new InvalidOperationException("Strategy not set");

            if (request.DistanceKm <= 0 || request.Passengers <= 0)
                throw new ArgumentException("Invalid travel data");

            return _strategy.CalculateCost(request);
        }
    }
}
