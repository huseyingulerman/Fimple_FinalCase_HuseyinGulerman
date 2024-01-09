using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Fimple_FinalCase_HuseyinGulerman.Repository
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Process> Processes { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is IEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.UtcNow;
                                entityReference.Status = Core.Enums.Status.Added;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                entityReference.ModifiedDate = DateTime.UtcNow;
                                entityReference.Status = Core.Enums.Status.Modified;
                                break;
                            }
                      
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is IEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.UtcNow;
                                entityReference.Status = Core.Enums.Status.Added;
                                entityReference.IsActive= true;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                entityReference.ModifiedDate = DateTime.UtcNow;
                                entityReference.Status = Core.Enums.Status.Modified;
                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}