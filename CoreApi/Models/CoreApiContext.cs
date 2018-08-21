using Microsoft.EntityFrameworkCore;
using CoreApi.DTO;

namespace CoreApi.Models
{
    public class CoreApiContext : DbContext
    {
        public CoreApiContext(DbContextOptions<CoreApiContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Access> Access { get; set; }

        public DbSet<Address> Address { get; set; }
        public DbSet<AddressType> AddressType { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<District> District { get; set; }

        public DbSet<TokenBlackList> TokenBlackList { get; set; }
    }
}