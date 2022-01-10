namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base()
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entity, object key) 
        : base($"Entity \"{entity}\" with key {key} was not found")
    {
    }
}