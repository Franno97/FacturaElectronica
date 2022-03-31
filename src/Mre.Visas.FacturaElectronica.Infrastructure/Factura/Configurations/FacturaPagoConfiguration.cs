using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Factura.Configurations
{
  public class FacturaPagoConfiguration : IEntityTypeConfiguration<Domain.Entities.FacturaPago>
  {
    public void Configure(EntityTypeBuilder<Domain.Entities.FacturaPago> builder)
    {
      builder.ToTable("FacturaPago");

      builder.HasKey(e => e.Id);

      builder.Property(e => e.Created)
          .IsRequired(true);
    }
  }
}