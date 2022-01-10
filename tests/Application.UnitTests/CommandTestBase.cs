using Infrastructure.Persistence;

namespace Application.UnitTests;

public class CommandTestBase
{
    protected readonly ApplicationDbContext _context;

    public CommandTestBase()
    {
        _context = ContextFactory.Create();
    }

    public void Dispose()
    {
        ContextFactory.Destroy(_context);
    }
}