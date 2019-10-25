using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain;

namespace WebApplication1.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostCategory> PostsCategories { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Na ovaj nacin ce Entity Framework znati o relacijama izmedju User, Roles, UserRole klasa, entiteta.
            // User moze biti deo vise rola.
            // Jedan Role moze biti povezan sa vise usera.
            // Vise ka vise(many to many) veza
            // User ----------------------------------------------------------------------------------
            /*builder.Entity<UserRole>(userRole =>
            {
                // U UserRole tabelu  spustam kljuceve od User i Role tabela.
                // Na ovaj nacin simuliram many-to-many relaciju.
                // UserRole postaje kompozitna tabela.
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                userRole
                .HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });*/
            // User ----------------------------------------------------------------------------------

            // PostCategory ----------------------------------------------------------------------------------
            builder.Entity<PostCategory>(postCategory => {

                postCategory.HasKey(pc => new { pc.PostId, pc.CategoryId });

                postCategory
                // Prvi deo -> jedan post se moze nalaziti unutar vise post kategorija
                .HasOne(pc => pc.Post)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(pc => pc.PostId)
                .IsRequired();

                // Drugi deo -> jedna kategorija moze sadrzati vise postova.
                postCategory
                .HasOne(pc => pc.Category)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired();
            });
            // PostCategory ----------------------------------------------------------------------------------
        }

    }
}
