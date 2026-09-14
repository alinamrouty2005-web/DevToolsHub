using System;
using System.Collections.Generic;
using System.Text;
using DevToolsHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DevToolsHub.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolHistory> ToolHistories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionItem> CollectionItems { get; set; }

        public DbSet<ApiRequest> ApiRequests { get; set; }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ==========================================
            // UserRole - Many-to-Many
            // ==========================================

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new
                {
                    ur.UserId,
                    ur.RoleId
                });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);


            // ==========================================
            // User -> Projects (One-to-Many)
            // ==========================================

            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ==========================================
            // User -> Subscription (One-to-One)
            // ==========================================

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithOne(u => u.Subscription)
                .HasForeignKey<Subscription>(s => s.UserId);


            // ==========================================
            // Subscription -> Plan (Many-to-One)
            // ==========================================

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Plan)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Restrict);


            // ==========================================
            // CollectionItem - Many-to-Many
            // ==========================================

            modelBuilder.Entity<CollectionItem>()
                .HasKey(ci => new
                {
                    ci.CollectionId,
                    ci.ToolId
                });

            modelBuilder.Entity<CollectionItem>()
                .HasOne(ci => ci.Collection)
                .WithMany(c => c.CollectionItems)
                .HasForeignKey(ci => ci.CollectionId);

            modelBuilder.Entity<CollectionItem>()
                .HasOne(ci => ci.Tool)
                .WithMany(t => t.CollectionItems)
                .HasForeignKey(ci => ci.ToolId);


            // ==========================================
            // Favorite
            // ==========================================

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Tool)
                .WithMany(t => t.Favorites)
                .HasForeignKey(f => f.ToolId);


            // ==========================================
            // ToolHistory
            // ==========================================

            modelBuilder.Entity<ToolHistory>()
                .HasOne(th => th.User)
                .WithMany(u => u.ToolHistories)
                .HasForeignKey(th => th.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ToolHistory>()
                .HasOne(th => th.Tool)
                .WithMany(t => t.ToolHistories)
                .HasForeignKey(th => th.ToolId);

            modelBuilder.Entity<ToolHistory>()
                .HasOne(th => th.Project)
                .WithMany(p => p.ToolHistories)
                .HasForeignKey(th => th.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);


            // ==========================================
            // ApiRequest
            // ==========================================

            modelBuilder.Entity<ApiRequest>()
                .HasOne(ar => ar.User)
                .WithMany(u => u.ApiRequests)
                .HasForeignKey(ar => ar.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApiRequest>()
                .HasOne(ar => ar.Project)
                .WithMany(p => p.ApiRequests)
                .HasForeignKey(ar => ar.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);


            // ==========================================
            // Notification
            // ==========================================

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);


            // ==========================================
            // Unique Constraints
            // ==========================================

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<Tool>()
                .HasIndex(t => t.Name)
                .IsUnique();


            // ==========================================
            // Subscription - one subscription per user
            // ==========================================

            modelBuilder.Entity<Subscription>()
                .HasIndex(s => s.UserId)
                .IsUnique();


            // ==========================================
            // Favorite - prevent duplicate favorite
            // ==========================================

            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new
                {
                    f.UserId,
                    f.ToolId
                })
                .IsUnique();


            // ==========================================
            // Decimal Precision
            // ==========================================

            modelBuilder.Entity<Plan>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
            // ==========================================
            // Seed Data
            // ==========================================

            // Roles
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "System administrator"
                },
                new Role
                {
                    Id = 2,
                    Name = "User",
                    Description = "Normal application user"
                }
            );

            // Plans
            modelBuilder.Entity<Plan>().HasData(
                new Plan
                {
                    Id = 1,
                    Name = "Free",
                    Price = 0,
                    DurationInDays = 30,
                    IsActive = true
                },
                new Plan
                {
                    Id = 2,
                    Name = "Pro",
                    Price = 9.99m,
                    DurationInDays = 30,
                    IsActive = true
                },
                new Plan
                {
                    Id = 3,
                    Name = "Team",
                    Price = 29.99m,
                    DurationInDays = 30,
                    IsActive = true
                }
            );

            // Tools
            modelBuilder.Entity<Tool>().HasData(
                new Tool
                {
                    Id = 1,
                    Name = "JSON Formatter",
                    Description = "Format and validate JSON data",
                    Category = "JSON",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Tool
                {
                    Id = 2,
                    Name = "JWT Decoder",
                    Description = "Decode JWT token payload",
                    Category = "Authentication",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Tool
                {
                    Id = 3,
                    Name = "Base64 Encoder Decoder",
                    Description = "Encode and decode Base64 text",
                    Category = "Encoding",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Tool
                {
                    Id = 4,
                    Name = "UUID Generator",
                    Description = "Generate UUID and GUID values",
                    Category = "Generators",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Tool
                {
                    Id = 5,
                    Name = "URL Encoder Decoder",
                    Description = "Encode and decode URLs",
                    Category = "Encoding",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}
