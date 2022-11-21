// 
// ObjectsDbContext.cs
// 
//   Created: 2022-11-02-01:15:56
//   Modified: 2022-11-02-01:16:11
// 
//   Author: Justin Chase <justin@justinwritescode.com>
//   
//   Copyright © 2022 Justin Chase, All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
// 

namespace JustinWritesCode.Functions.Objects.EntityFrameworkCore;

using JustinWritesCode.Functions.Objects.Models;
using Microsoft.EntityFrameworkCore;

public class ObjectsDbContext : DbContext
{
    public ObjectsDbContext(DbContextOptions<ObjectsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ObjectPropertyDbModel>().ToTable("UserProperties");
        modelBuilder.Entity<ObjectPropertyDbModel>().HasKey(x => x.Id);
        modelBuilder.Entity<ObjectPropertyDbModel>().Property(x => x.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<ObjectPropertyDbModel>().Property(x => x.ObjectId).IsRequired().HasColumnName("UserId");
        modelBuilder.Entity<ObjectPropertyDbModel>().Property(x => x.Key).IsRequired().HasColumnName("Property");
    }

    public DbSet<ObjectPropertyDbModel>? ObjectProperties { get; set; }
}
