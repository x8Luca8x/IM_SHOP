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

        public DbSet<TARTICLE> Article { get; set; }
        public DbSet<TCATEGORY> Category { get; set; }

        public DbSet<TCURRENCY> Currency { get; set; }

        public DbSet<TCART> Cart { get; set; }
        public DbSet<TCARTARTICLE> CartArticle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUSER>().HasIndex(u => u.USERNAME).IsUnique();
            modelBuilder.Entity<TUSER>().HasIndex(u => u.EMAIL).IsUnique();

            modelBuilder.Entity<TTOKEN>().HasIndex(t => t.TOKEN).IsUnique();

            modelBuilder.Entity<TPERSON>().HasIndex(p => p.USERID).IsUnique();
            modelBuilder.Entity<TUSEROPTIONS>().HasIndex(p => p.USERID).IsUnique();

            modelBuilder.Entity<TCART>().HasIndex(c => c.USERID).IsUnique();
        }
    }
}