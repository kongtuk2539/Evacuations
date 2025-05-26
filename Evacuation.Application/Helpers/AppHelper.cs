using Evacuations.Application.Dtos.Common;

namespace Evacuations.Application.Helpers;

public static class AppHelper
{
    public static LocationCoordinates ToLocation(double lat, double lon)
    {
        return new() { Latitude = lat, Longitude = lon };
    }
}
