using Evacuations.Application.Dtos.Common;

namespace Evacuations.Application.Helpers;

public static class CalculatorHelper
{
    private static readonly int radianOfEarth = 6371;
    public static double Haversine(LocationCoordinates location1, LocationCoordinates location2)
    {
        double distanceLat = toRadian(location2.Latitude - location1.Latitude);
        double distanceLon = toRadian(location2.Longitude - location2.Longitude);

        double radianLat1 = toRadian(location1.Latitude);
        double radianLat2 = toRadian(location2.Latitude);

        double calculate = Math.Pow(Math.Sin(distanceLat / 2), 2) +
                   Math.Pow(Math.Sin(distanceLon / 2), 2) *
                   Math.Cos(radianLat1) * Math.Cos(radianLat2);

        double c = 2 * Math.Asin(Math.Sqrt(calculate));
        return radianOfEarth * c;
    }

    public static double ETAMinutes(double distance, double speedKmh)
    {
        if (speedKmh <= 0)
            throw new ArgumentException("Speed must be greater than zero.");

        double timeHours = distance / speedKmh;

        return Math.Round(timeHours);
    }

    private static double toRadian(double val)
    {
        return (Math.PI / 180) * val;
    }
}
