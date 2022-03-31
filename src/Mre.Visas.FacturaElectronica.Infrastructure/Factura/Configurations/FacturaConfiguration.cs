using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Factura.Configurations
{
  public class FacturaConfiguration : IEntityTypeConfiguration<Domain.Entities.Factura>
  {
    public void Configure(EntityTypeBuilder<Domain.Entities.Factura> builder)
    {
      builder.ToTable("Factura");

      builder.HasKey(e => e.Id);

      builder.Property(e => e.Created)
          .IsRequired(true);
    }
  }
}
