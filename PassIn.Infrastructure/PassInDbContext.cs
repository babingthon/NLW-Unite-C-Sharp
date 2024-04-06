using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entities;
using System;

namespace PassIn.Infrastructure;
public class PassInDbContext : DbContext
{   
    public DbSet<Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSqlite("Data Source=C:\\Users\\Babingthon\\Desktop\\PassIn\\PassInDb.db");
    }
  
}
