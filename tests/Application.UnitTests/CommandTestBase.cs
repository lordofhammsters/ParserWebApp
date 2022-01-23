using System;
using ParserWebApp.Infrastructure.Persistence;

namespace Application.UnitTests;

public class CommandTestBase : IDisposable
{
    protected readonly ApplicationDbContext Context;

    public CommandTestBase()
    {
        Context = DbContextFactory.Create();
    }

    public void Dispose()
    {
        DbContextFactory.Destroy(Context);
    }
}