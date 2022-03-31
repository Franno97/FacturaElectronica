using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Factura.Configurations
{
 public class FacturaDetalleConfiguration : IEntityTypeConfiguration<Domain.Entities.FacturaDetalle>
  {
    public void Configure(EntityTypeBuilder<Domain.Entities.FacturaDetalle> builder)
    {
      builder.ToTable("FacturaDetalle");

      builder.HasKey(e => e.Id);

      builder.Property(e => e.Created)
          .IsRequired(true);
    }
  }
}
