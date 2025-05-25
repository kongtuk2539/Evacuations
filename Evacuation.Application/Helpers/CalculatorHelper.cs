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

    public static string ETAMinutes(double distance, double speedKmh)
    {
        var oneMinutes = 1;
        if (speedKmh <= 0)
            throw new ArgumentException("Speed must be greater than zero.");

        double timeHours = distance / speedKmh;

        var minutes = Math.Round(timeHours) <= 0 ? oneMinutes : Math.Round(timeHours);

        return toHoursString(minutes);
    }

    private static string toHoursString(double minutes)
    {
        var minutesPerHours = 60;
        var hours = (int)minutes / minutesPerHours;
        return $"{addPrefixZero(hours)}:{addPrefixZero(minutes % minutesPerHours)} Minuite";
    }

    private static string addPrefixZero(double number)
    {
        return number <= 9 ? $"0{number}" : number.ToString();
    }

    private static double toRadian(double val)
    {
        return (Math.PI / 180) * val;
    }
}
