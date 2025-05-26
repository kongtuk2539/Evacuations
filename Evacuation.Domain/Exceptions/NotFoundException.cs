namespace Evacuations.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string resourceType, string resourceIdentifier) :
        base($"There are no {resourceType} with id: {resourceIdentifier} available")
    {
        
    }

    public NotFoundException(string resourceType) :
        base($"There are no {resourceType} available")
    {

    }
}
