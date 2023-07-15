using System;
using Microsoft.EntityFrameworkCore;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;

namespace ReadTrack.API.Data;

public class ReadTrackContext : DbContext
{
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<BookEntity> Books { get; set; }
    public virtual DbSet<SessionEntity> Sessions { get; set; }

    public ReadTrackContext(DbContextOptions<ReadTrackContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookEntity>(entity =>
        {
            entity.ToTable("Books");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.Published).HasColumnType("datetime");

            entity.Property(e => e.Category).HasConversion(c => c.ToString(), c => (BookCategory)Enum.Parse(typeof(BookCategory), c));

            entity.HasMany<SessionEntity>()
                .WithOne(e => e.Book)
                .HasForeignKey(s => s.BookId);
        });

        modelBuilder.Entity<SessionEntity>(entity =>
        {
            entity.ToTable("Sessions");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
        });
    }
}