namespace IM_API
{
    public class IMDbContext : DbContext
    {
        public static string ConnectionString { get; set; } = string.Empty;

        public IMDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<TUSER> User { get; set; }
        public DbSet<TUSEROPTIONS> UserOptions { get; set; }
        public DbSet<TTOKEN> Token { get; set; }

        public DbSet<TPERSON> Person { get; set; }
        public DbSet<TIMAGE> Image { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUSER>().HasIndex(u => u.USERNAME).IsUnique();
            modelBuilder.Entity<TUSER>().HasIndex(u => u.EMAIL).IsUnique();

            modelBuilder.Entity<TTOKEN>().HasIndex(t => t.TOKEN).IsUnique();

            modelBuilder.Entity<TPERSON>().HasIndex(p => p.USERID).IsUnique();
            modelBuilder.Entity<TUSEROPTIONS>().HasIndex(p => p.USERID).IsUnique();

            modelBuilder.Entity<TIMAGE>().HasIndex(i => i.PERSONID).IsUnique();
        }
    }
}