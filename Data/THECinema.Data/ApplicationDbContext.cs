﻿namespace THECinema.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Models;
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

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

        public DbSet<Hall> Halls { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Projection> Projections { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<ProjectionSeat> ProjectionsSeats { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Comment> Comments { get; set; }

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
            base.OnModelCreating(builder);

            this.ConfigureRelations(builder);

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
        private void ConfigureRelations(ModelBuilder builder)
        {
            builder.Entity<Reservation>()
                .HasOne(r => r.ApplicationUser)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.ApplicationUserId);

            builder.Entity<Projection>()
                .HasKey(p => p.Id);

            builder.Entity<Projection>()
                .HasOne(p => p.Movie)
                .WithMany(m => m.Halls)
                .HasForeignKey(p => p.MovieId);

            builder.Entity<Projection>()
                .HasOne(p => p.Hall)
                .WithMany(h => h.Movies)
                .HasForeignKey(p => p.HallId);

            builder.Entity<ProjectionSeat>()
                .HasKey(ps => new { ps.ProjectionId, ps.SeatId });

            builder.Entity<ProjectionSeat>()
                .HasOne(ps => ps.Projection)
                .WithMany(p => p.Seats)
                .HasForeignKey(ps => ps.ProjectionId);

            builder.Entity<ProjectionSeat>()
                .HasOne(ps => ps.Seat)
                .WithMany(s => s.Projections)
                .HasForeignKey(ps => ps.SeatId);

            builder.Entity<ProjectionSeat>()
                .HasOne(ps => ps.Reservation)
                .WithMany(r => r.Seats)
                .HasForeignKey(ps => ps.ReservationId);
        }

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
