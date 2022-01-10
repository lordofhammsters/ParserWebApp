using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.UnitTests;

public class ContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.Database.EnsureCreated();

        // add some data and save
        //context.SaveChanges();

        return context;
    }

    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();

        context.Dispose();
    }
}