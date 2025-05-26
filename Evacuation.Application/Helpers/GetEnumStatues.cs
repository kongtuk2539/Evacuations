namespace Evacuations.Domain.Common;

public static class GetEnumStatues
{
    public static List<string> GetAll()
    {
        return Enum.GetNames(typeof(EnumStatuses)).ToList();
    }
}
