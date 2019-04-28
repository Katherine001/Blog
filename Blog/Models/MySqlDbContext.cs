using Microsoft.EntityFrameworkCore;

namespace Blog.Models
{
    public class MySQLDbContext : DbContext
    {
        public MySQLDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        
        public DbSet<Blog> Blog { get; set; }
        
        public DbSet<Role> Role { get; set; }
        
        public DbSet<Btag> Btag { get; set; }
        
        public DbSet<Breview> Breview { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //定义User的主键u_id
            modelBuilder.Entity<User>().HasKey(b => b.u_id).HasName("PRIMARY");
            modelBuilder.Entity<Blog>().HasKey(b => b.a_id).HasName("PRIMARY");
            modelBuilder.Entity<Role>().HasKey(b => b.r_id).HasName("PRIMARY");
            modelBuilder.Entity<Btag>().HasKey(b => b.k_id).HasName("PRIMARY");
            modelBuilder.Entity<Breview>().HasKey(b => b.p_id).HasName("PRIMARY");
          
        }

        
    }
}