using EFCore.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DAL
{
    public class DataDBContext : DbContext
    {
        public DataDBContext(DbContextOptions<DataDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //关联表，指定两个ID为主键。
            modelBuilder.Entity<CityCompany>().HasKey(x => new { x.CityID, x.CompanyID });
            //关联一对多关系，一个省份多个城市
            modelBuilder.Entity<City>().HasOne(x => x.Province).WithMany(x => x.Cities).HasForeignKey(x => x.ProvinceID);

            //关联多对多关系，一个城市多个公司
            modelBuilder.Entity<CityCompany>().HasOne(x => x.City).WithMany(x => x.CityCompanies).HasForeignKey(x => x.CityID);
            //关联多对多关系，一个公司多个城市
            modelBuilder.Entity<CityCompany>().HasOne(x => x.Company).WithMany(x => x.CityCompanies).HasForeignKey(x => x.CompanyID);
            //关联一对一关系，一个城市一个市长
            modelBuilder.Entity<Mayor>().HasOne(x => x.City).WithOne(x => x.Mayor).HasForeignKey<Mayor>(x => x.CityID);

            modelBuilder.Entity<Province>().HasData(
                //需要指定ID，如果是GUID，请写死，不要new GUID()
                new Province { ID = 1, name = "广东", population = 9000_000 },
                new Province { ID = 2, name = "福建", population = 8000_000 }
            );

            modelBuilder.Entity<City>().HasData(
                //需要指定外键ProvinceID
                new City { ProvinceID = 1, ID = 1, name = "汕头" },
                new City { ProvinceID = 1, ID = 2, name = "广州" },
                new City { ProvinceID = 1, ID = 3, name = "深圳" }
            );
        }
        //省份
        public DbSet<Province> Provinces { get; set; }
        //城市
        public DbSet<City> Cities { get; set; }
        //公司
        public DbSet<Company> Companies { get; set; }
        //城市公司关联表
        public DbSet<CityCompany> CityCompanies { get; set; }
        //市长
        public DbSet<Mayor> Mayors { get; set; }
    }
}
