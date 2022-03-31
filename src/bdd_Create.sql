USE [FacturaElectronica]
GO
/****** Object:  Table [dbo].[Factura]    Script Date: 13/12/2021 18:00:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factura](
	[Id] [uniqueidentifier] NOT NULL,
	[CodigoOficina] [varchar](20) NULL,
	[CodigoUsuario] [varchar](300) NULL,
	[TipoIdentificacionComprador] [varchar](2) NULL,
	[RazonSocialComprador] [varchar](300) NULL,
	[IdentificacionComprador] [varchar](20) NULL,
	[DireccionComprador] [varchar](300) NULL,
	[TelefonoComprador] [varchar](50) NULL,
	[CorreoComprador] [varchar](300) NULL,
	[TotalSinImpuestos] [numeric](18, 4) NULL,
	[TotalDescuento] [numeric](18, 4) NULL,
	[ImporteTotal] [numeric](18, 4) NULL,
	[Porcentaje] [numeric](18, 4) NULL,
	[FechaEmisionLocal] [varchar](8) NULL,
	[Referencia] [varchar](300) NULL,
	[LastModified] [datetime] NULL,
	[LastModifierId] [uniqueidentifier] NULL,
	[Created] [datetime] NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Factura] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FacturaDetalle]    Script Date: 13/12/2021 18:00:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacturaDetalle](
	[Id] [uniqueidentifier] NOT NULL,
	[IdFactura] [uniqueidentifier] NULL,
	[OrdenDetalle] [int] NULL,
	[CodigoPrincipal] [varchar](25) NULL,
	[CodigoAuxiliar] [varchar](25) NULL,
	[Descripcion] [varchar](300) NULL,
	[Cantidad] [int] NULL,
	[PrecioUnitario] [decimal](12, 4) NULL,
	[Descuento] [decimal](12, 4) NULL,
	[PrecioTotalSinImpuesto] [decimal](12, 4) NULL,
	[LastModified] [datetime] NULL,
	[LastModifierId] [uniqueidentifier] NULL,
	[Created] [datetime] NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_FacturaDetalle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FacturaPago]    Script Date: 13/12/2021 18:00:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacturaPago](
	[Id] [uniqueidentifier] NOT NULL,
	[IdFactura] [uniqueidentifier] NULL,
	[Orden] [int] NULL,
	[FormaPago] [int] NULL,
	[Total] [decimal](12, 4) NULL,
	[LastModified] [datetime] NULL,
	[LastModifierId] [uniqueidentifier] NULL,
	[Created] [datetime] NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_FacturaPago] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FacturaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_FacturaDetalle] FOREIGN KEY([IdFactura])
REFERENCES [dbo].[Factura] ([Id])
GO
ALTER TABLE [dbo].[FacturaDetalle] CHECK CONSTRAINT [FK_FacturaDetalle]
GO
ALTER TABLE [dbo].[FacturaPago]  WITH CHECK ADD  CONSTRAINT [FK_FacturaPago] FOREIGN KEY([IdFactura])
REFERENCES [dbo].[Factura] ([Id])
GO
ALTER TABLE [dbo].[FacturaPago] CHECK CONSTRAINT [FK_FacturaPago]
GO
