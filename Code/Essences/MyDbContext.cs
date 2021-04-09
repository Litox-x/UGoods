using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UGoods.Essences
{
    public class MyDbContext : DbContext
    {


        public DbSet<Goods> Goods { get; set; }
        public DbSet<RegIn> RegIn { get; set; }
        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<SellInfo> SellInfo { get; set; }

        public MyDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=UGoodsBase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegIn>().HasData(
                new RegIn[]
                {
                new RegIn { Id=1, Login="Admin", Password=ViewModel.MainWindowViewModel.getSHA256("Admin")}
                });
            modelBuilder.Entity<PersonalInfo>().HasData(
                new PersonalInfo[]
                {
                new PersonalInfo { Id=1, Name="My Admin Role", Role="Manager" }
                });

        }
    }
}