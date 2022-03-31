using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using Mre.Visas.FacturaElectronica.Domain.Entities;
using Audit.EntityFramework;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Persistence.Contexts
{
  public class ApplicationDbContext : DbContext
  {
    #region Properties

    public DbSet<Domain.Entities.Factura> Facturas { get; set; }

    #endregion Properties

    #region Constructors

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

        #endregion Constructors

        #region Methods

        //Configure Db
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
      {
        switch (entry.State)
        {
          case EntityState.Added:
            entry.Entity.Created = DateTime.UtcNow;
            break;

          case EntityState.Modified:
            entry.Entity.LastModified = DateTime.UtcNow;
            break;
        }
      }

      return base.SaveChangesAsync(cancellationToken);
    }

    #endregion Methods
  }
}