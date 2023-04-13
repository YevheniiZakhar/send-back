namespace Land.Data
{
    public class Localit
    {
        public int Id { get; set; }
        public string Locality {  get; set; }
    }
    public class LandDbContext : DbContext
    {
        public LandDbContext(DbContextOptions<LandDbContext> options) : base(options)
        {

        }
        public DbSet<Ad> Ad { get; set; }

        public DbSet<User> User { get; set; }

        //public DbSet<Locality> Locality { get; set; }
        public DbSet<Localit> Locality { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Localit>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.Id);
                entity.Property(e => e.Locality);
            });
            //modelBuilder.Entity<Video>(e => e.Property(o =>o.Description).HasColumnType("tinyint(1)").HasConversion<short>());
            //modelBuilder.Entity<Video>(e => e.Property(o => o.Dislikes).HasColumnType("bit"));
        }
    }
}