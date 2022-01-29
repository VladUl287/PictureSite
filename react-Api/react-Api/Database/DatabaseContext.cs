using Microsoft.EntityFrameworkCore;
using react_Api.Database.Models;
using System;
using System.Diagnostics;

namespace react_Api.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureTags> PicturesTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo((string message) =>
            {
                Console.WriteLine(message);
                Debug.WriteLine(message);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(build =>
            {
                build.HasKey(e => e.Id);

                build.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                build.HasIndex(e => e.Email)
                    .IsUnique();

                build.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(150);

                build.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150);

                build.HasData(new User
                {
                    Id = 1,
                    Email = "ulyanovskiy.01@mail.ru",
                    Login = "fefrues",
                    Password = "rqhdJQb/Oi7AvOFUJsnFlo99n6F7ct0B+Sgudw7kNMM=",
                    RoleId = 1
                });
            });

            modelBuilder.Entity<Role>(build =>
            {
                build.HasKey(e => e.Id);

                build.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                build.HasData(new Role[]
                {
                    new Role() { Id = 1, Name = "Admin" },
                    new Role() { Id = 2, Name = "User" }
                });
            });

            modelBuilder.Entity<Token>(build =>
            {
                build.HasKey(e => e.Id);

                build.Property(e => e.RefreshToken)
                    .IsRequired();

                build.HasIndex(e => e.RefreshToken);
            });

            modelBuilder.Entity<Picture>(build =>
            {
                build.HasKey(e => e.Id);
                
                build.Property(e => e.Path)
                    .IsRequired();
            });

            modelBuilder.Entity<Tag>(build =>
            {
                build.HasKey(e => e.Id);

                build.HasData(new Tag[]
                {
                    new Tag { Id = 1, Name = "море" },
                    new Tag { Id = 2, Name = "волны" },
                    new Tag { Id = 3, Name = "песок" }
                });
            });

            modelBuilder.Entity<PictureTags>(build =>
            {
                build.HasKey(e => new { e.PictureId, e.TagId });

                build.HasOne(bc => bc.Picture)
                    .WithMany(b => b.PicturesTags)
                    .HasForeignKey(bc => bc.PictureId);

                build.HasOne(bc => bc.Tag)
                    .WithMany(c => c.PicturesTags)
                    .HasForeignKey(bc => bc.TagId);
            });
        }
    }
}