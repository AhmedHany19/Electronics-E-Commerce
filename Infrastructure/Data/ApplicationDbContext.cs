using Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			builder.Entity<Category>().Property(x => x.Id).HasDefaultValueSql("(newid())");
			builder.Entity<LogCategory>().Property(x => x.Id).HasDefaultValueSql("(newid())");
			builder.Entity<Product>().Property(x => x.Id).HasDefaultValueSql("(newid())");
			builder.Entity<LogProduct>().Property(x => x.Id).HasDefaultValueSql("(newid())");
			builder.Entity<Brand>().Property(x => x.Id).HasDefaultValueSql("(newid())");
			builder.Entity<LogBrand>().Property(x => x.Id).HasDefaultValueSql("(newid())");
			builder.Entity<ShoppingCart>().Property(x => x.Id).HasDefaultValueSql("(newid())");
			builder.Entity<Product>().HasOne(a => a.Category).WithMany(a => a.Products).HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.ClientSetNull);
			builder.Entity<Product>().HasOne(a => a.Brand).WithMany(a => a.Products).HasForeignKey(a => a.BrandId).OnDelete(DeleteBehavior.ClientSetNull);

			builder.Entity<VwUser>(entity =>
			{
				entity.HasNoKey();
				entity.ToView("VwUsers");
			});

		}

		public DbSet<Category>? Categories { get; set; }
		public DbSet<LogCategory>? LogCategories { get; set; }
		public DbSet<Product>? Products { get; set; }
		public DbSet<LogProduct>? LogProducts { get; set; }
		public DbSet<Brand>? Brands { get; set; }
		public DbSet<LogBrand>? LogBrands { get; set; }
		public DbSet<OrderDetail>? OrderDetails { get; set; }
		public DbSet<ShoppingCart>? ShoppingCarts { get; set; }
		public DbSet<OrderHeader>? OrderHeaders { get; set; }
		public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
		public DbSet<VwUser>? VwUsers { get; set; }
		public DbSet<ContactUs>? ContactUs { get; set; }
	}
}
