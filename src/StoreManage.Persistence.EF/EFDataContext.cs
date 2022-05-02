using Microsoft.EntityFrameworkCore;
using StoreManage.Entities;
using StoreManage.Persistence.EF.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Persistence.EF
{
    public class EFDataContext : DbContext
    {
        public EFDataContext(string connectionString) :
          this(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)
        { }

        public EFDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(CommodityEntityMap).Assembly);
        }

        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BuyFactor> BuyFactors { get; set; }
        public DbSet<SellFactor> SellFactors { get; set; }
    }
}
