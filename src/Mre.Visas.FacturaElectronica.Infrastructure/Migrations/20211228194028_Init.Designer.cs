﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mre.Visas.FacturaElectronica.Infrastructure.Persistence.Contexts;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211228194028_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mre.Visas.FacturaElectronica.Domain.Entities.Factura", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClaveAcceeso")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodigoOficina")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodigoUsuario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CorreoComprador")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DireccionComprador")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EstadoProceso")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FechaEmisionLocal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificacionComprador")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ImporteTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Numero")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RazonSocialComprador")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Referencia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Resultado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelefonoComprador")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoIdentificacionComprador")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalDescuento")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalSinImpuestos")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Factura");
                });

            modelBuilder.Entity("Mre.Visas.FacturaElectronica.Domain.Entities.FacturaDetalle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CampoAdicional1Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CampoAdicional1Valor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CampoAdicional2Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CampoAdicional2Valor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CampoAdicional3Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CampoAdicional3Valor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Cantidad")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CodigoAuxiliar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodigoPrincipal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Descuento")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("FacturaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OrdenDetalle")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecioTotalSinImpuesto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("FacturaDetalle");
                });

            modelBuilder.Entity("Mre.Visas.FacturaElectronica.Domain.Entities.FacturaPago", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FacturaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FormaPago")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastModifierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("FacturaPago");
                });
#pragma warning restore 612, 618
        }
    }
}
