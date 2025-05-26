using Evacuations.Application.Dtos.Common;

namespace Evacuations.Application.Helpers;

public static class CalculatorHelper
{
    private static readonly int radianOfEarth = 6371;
    public static double Haversine(LocationCoordinates location1, LocationCoordinates location2)
    {
        double distanceLat = ToRadian(location2.Latitude - location1.Latitude);
        double distanceLon = ToRadian(location2.Longitude - location2.Longitude);

        double radianLat1 = ToRadian(location1.Latitude);
        double radianLat2 = ToRadian(location2.Latitude);

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

        return ToHoursString(minutes);
    }

    public static int RemainOfPeople(int numberOfPeople, int capacity)
    {
        return numberOfPeople <= capacity ? 0 : numberOfPeople - capacity;
    }

    public static int EvacuatedPeople(int numberOfPeople, int capacity)
    {
        return numberOfPeople <= capacity ? numberOfPeople : capacity;
    }

    private static string ToHoursString(double minutes)
    {
        var minutesPerHours = 60;
        var hours = (int)minutes / minutesPerHours;
        return $"{AddPrefixZero(hours)}:{AddPrefixZero(minutes % minutesPerHours)} Minuite";
    }

    private static double ToRadian(double val)
    {
        return (Math.PI / 180) * val;
    }

    private static string AddPrefixZero(double number)
    {
        return number <= 9 ? $"0{number}" : number.ToString();
    }


}
