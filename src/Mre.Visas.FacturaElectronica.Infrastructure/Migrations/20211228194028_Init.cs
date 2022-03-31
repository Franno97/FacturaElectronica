using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Factura",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoOficina = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoIdentificacionComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RazonSocialComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificacionComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DireccionComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonoComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorreoComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSinImpuestos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImporteTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaEmisionLocal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaveAcceeso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resultado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoProceso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacturaDetalle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacturaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrdenDetalle = table.Column<int>(type: "int", nullable: false),
                    CodigoPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoAuxiliar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioTotalSinImpuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CampoAdicional1Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoAdicional1Valor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoAdicional2Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoAdicional2Valor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoAdicional3Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoAdicional3Valor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaDetalle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacturaPago",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacturaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    FormaPago = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaPago", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Factura");

            migrationBuilder.DropTable(
                name: "FacturaDetalle");

            migrationBuilder.DropTable(
                name: "FacturaPago");
        }
    }
}
