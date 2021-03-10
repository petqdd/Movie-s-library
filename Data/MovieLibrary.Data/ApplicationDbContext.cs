namespace MovieLibrary.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using MovieLibrary.Data.Common.Models;
    using MovieLibrary.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Director> Directors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<MoviesArtist> MoviesArtists { get; set; }

        public DbSet<UsersComment> UsersComments { get; set; }

        public DbSet<MoviesComment> MoviesComments { get; set; }

        public DbSet<UsersMovie> UsersMovies { get; set; }

        public DbSet<MoviesCategory> MoviesCategories { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<UsersRatings> UsersRatings { get; set; }

        public DbSet<MoviesRatings> MoviesRatings { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration

            //base.OnModelCreating(builder);

            //builder.Entity<UsersComment>()
            //   .HasKey(x => new { x.UserId, x.CommentId });

            //builder.Entity<MoviesComment>()
            //  .HasKey(x => new { x.MovieId, x.CommentId });

            //builder.Entity<UsersMovie>()
            //  .HasKey(x => new { x.UserId, x.MovieId });

            //builder.Entity<MoviesArtist>()
            //    .HasKey(x => new { x.MovieId, x.ArtistId });

            //builder.Entity<MoviesRatings>()
            //    .HasKey(x => new { x.MovieId, x.RatingId });

            //builder.Entity<UsersRatings>()
            //    .HasKey(x => new { x.UserId, x.RatingId });

            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
