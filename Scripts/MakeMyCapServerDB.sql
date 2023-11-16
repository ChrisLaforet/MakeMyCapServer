USE [master]
GO
/****** Object:  Database [MakeMyCapServer]    Script Date: 11/16/2023 3:01:49 PM ******/
CREATE DATABASE [MakeMyCapServer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MakeMyCapServer', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\MakeMyCapServer.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MakeMyCapServer_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\MakeMyCapServer_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [MakeMyCapServer] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MakeMyCapServer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MakeMyCapServer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET ARITHABORT OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MakeMyCapServer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MakeMyCapServer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MakeMyCapServer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MakeMyCapServer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET RECOVERY FULL 
GO
ALTER DATABASE [MakeMyCapServer] SET  MULTI_USER 
GO
ALTER DATABASE [MakeMyCapServer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MakeMyCapServer] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MakeMyCapServer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MakeMyCapServer] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MakeMyCapServer] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MakeMyCapServer] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'MakeMyCapServer', N'ON'
GO
ALTER DATABASE [MakeMyCapServer] SET QUERY_STORE = OFF
GO
USE [MakeMyCapServer]
GO
/****** Object:  Table [dbo].[Distributor]    Script Date: 11/16/2023 3:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Distributor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LookupCode] [varchar](5) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[AccountNumber] [varchar](25) NULL,
 CONSTRAINT [PK_Distributor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/16/2023 3:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VariantId] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Sku] [varchar](20) NOT NULL,
	[Title] [varchar](255) NULL,
	[Vendor] [varchar](255) NULL,
	[InventoryItemId] [bigint] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 11/16/2023 3:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[DistributorId] [int] NOT NULL,
	[PONumber] [varchar](25) NOT NULL,
	[ShopifyOrderId] [bigint] NULL,
	[Sku] [varchar](20) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Style] [varchar](50) NULL,
	[Color] [varchar](50) NULL,
	[Size] [varchar](50) NULL,
	[SubmittedDateTime] [datetime2](7) NOT NULL,
	[LastAttemptDateTime] [datetime2](7) NULL,
	[SuccessDateTime] [datetime2](7) NULL,
	[Attempts] [int] NOT NULL,
	[WarningNotificationCount] [int] NOT NULL,
	[FailureNotificationDateTime] [datetime2](7) NULL,
 CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanMarSkuMap]    Script Date: 11/16/2023 3:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanMarSkuMap](
	[Sku] [varchar](20) NOT NULL,
	[Style] [varchar](50) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[Size] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SanMarSkuMap] PRIMARY KEY CLUSTERED 
(
	[Sku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceLog]    Script Date: 11/16/2023 3:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [varchar](50) NOT NULL,
	[StartTime] [datetime2](7) NOT NULL,
	[EndTime] [datetime2](7) NULL,
	[Failed] [bit] NULL,
 CONSTRAINT [PK_ServiceLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 11/16/2023 3:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[InventoryCheckHours] [int] NOT NULL,
	[FulfillmentCheckHours] [int] NOT NULL,
	[NextPOSequence] [int] NULL,
	[StatusEmailRecipient1] [varchar](120) NULL,
	[StatusEmailRecipient2] [varchar](120) NULL,
	[StatusEmailRecipient3] [varchar](120) NULL,
	[CriticalEmailRecipient1] [varchar](120) NULL,
	[CriticalEmailRecipient2] [varchar](120) NULL,
	[CriticalEmailRecipient3] [varchar](120) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shipping]    Script Date: 11/16/2023 3:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shipping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShipTo] [varchar](25) NULL,
	[Name] [varchar](35) NOT NULL,
	[ShipAddress] [varchar](35) NOT NULL,
	[ShipCity] [varchar](25) NOT NULL,
	[ShipState] [varchar](2) NOT NULL,
	[ShipZip] [varchar](10) NOT NULL,
	[ShipEmail] [varchar](100) NULL,
	[ShipMethod] [varchar](15) NOT NULL,
	[Attention] [varchar](35) NULL,
 CONSTRAINT [PK_Shipping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SkuDistributor]    Script Date: 11/16/2023 3:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkuDistributor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sku] [varchar](20) NOT NULL,
	[DistributorId] [int] NOT NULL,
 CONSTRAINT [PK_SkuDistributor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Distributor] ON 
GO
INSERT [dbo].[Distributor] ([Id], [LookupCode], [Name], [AccountNumber]) VALUES (1, N'CA', N'CapAmerica', NULL)
GO
INSERT [dbo].[Distributor] ([Id], [LookupCode], [Name], [AccountNumber]) VALUES (2, N'SS', N'S&S', NULL)
GO
INSERT [dbo].[Distributor] ([Id], [LookupCode], [Name], [AccountNumber]) VALUES (3, N'SM', N'SanMar', N'277332')
GO
SET IDENTITY_INSERT [dbo].[Distributor] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 
GO
INSERT [dbo].[Product] ([Id], [VariantId], [ProductId], [Sku], [Title], [Vendor], [InventoryItemId]) VALUES (20, 47336645656878, 8852182663470, N'sku-hosted-1', N'The 3p Fulfilled Snowboard: Default Title', N'Chris Learning Center', 49384697430318)
GO
INSERT [dbo].[Product] ([Id], [VariantId], [ProductId], [Sku], [Title], [Vendor], [InventoryItemId]) VALUES (21, 47336645165358, 8852182401326, N'sku-untracked-1', N'The Inventory Not Tracked Snowboard: Default Title', N'Chris Learning Center', 49384696938798)
GO
INSERT [dbo].[Product] ([Id], [VariantId], [ProductId], [Sku], [Title], [Vendor], [InventoryItemId]) VALUES (22, 47336645624110, 8852182630702, N'sku-managed-1', N'The Multi-managed Snowboard: Default Title', N'Multi-managed Vendor', 49384697397550)
GO
INSERT [dbo].[Product] ([Id], [VariantId], [ProductId], [Sku], [Title], [Vendor], [InventoryItemId]) VALUES (23, 47340689555758, 8852692042030, N'12345678WB4', N'White snapback cap with black bill: XL', N'Chris Learning Center', 49388744376622)
GO
INSERT [dbo].[Product] ([Id], [VariantId], [ProductId], [Sku], [Title], [Vendor], [InventoryItemId]) VALUES (24, 47340689588526, 8852692042030, N'12345678WB5', N'White snapback cap with black bill: XXL', N'Chris Learning Center', 49388744409390)
GO
INSERT [dbo].[Product] ([Id], [VariantId], [ProductId], [Sku], [Title], [Vendor], [InventoryItemId]) VALUES (25, 47340689621294, 8852692042030, N'12345678WB3', N'White snapback cap with black bill: L', N'Chris Learning Center', 49388744442158)
GO
INSERT [dbo].[Product] ([Id], [VariantId], [ProductId], [Sku], [Title], [Vendor], [InventoryItemId]) VALUES (26, 47340689654062, 8852692042030, N'12345678WB2', N'White snapback cap with black bill: M', N'Chris Learning Center', 49388744474926)
GO
INSERT [dbo].[Product] ([Id], [VariantId], [ProductId], [Sku], [Title], [Vendor], [InventoryItemId]) VALUES (27, 47340689686830, 8852692042030, N'12345678WB1', N'White snapback cap with black bill: S', N'Chris Learning Center', 49388744507694)
GO
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceLog] ON 
GO
SET IDENTITY_INSERT [dbo].[ServiceLog] OFF
GO
INSERT [dbo].[Settings] ([InventoryCheckHours], [FulfillmentCheckHours], [NextPOSequence], [StatusEmailRecipient1], [StatusEmailRecipient2], [StatusEmailRecipient3], [CriticalEmailRecipient1], [CriticalEmailRecipient2], [CriticalEmailRecipient3]) VALUES (8, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Shipping] ON 
GO
INSERT [dbo].[Shipping] ([Id], [ShipTo], [Name], [ShipAddress], [ShipCity], [ShipState], [ShipZip], [ShipEmail], [ShipMethod], [Attention]) VALUES (2, N'Cap America Annex', N'CapAmerica', N'200 South Chamber Dr', N'Fredrickstown', N'MO', N'63645', N'webmaster', N'FedEx', NULL)
GO
SET IDENTITY_INSERT [dbo].[Shipping] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Distributor]    Script Date: 11/16/2023 3:01:49 PM ******/
ALTER TABLE [dbo].[Distributor] ADD  CONSTRAINT [IX_Distributor] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Distributor_1]    Script Date: 11/16/2023 3:01:49 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Distributor_1] ON [dbo].[Distributor]
(
	[LookupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product]    Script Date: 11/16/2023 3:01:49 PM ******/
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [IX_Product] UNIQUE NONCLUSTERED 
(
	[VariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Product_1]    Script Date: 11/16/2023 3:01:49 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Product_1] ON [dbo].[Product]
(
	[Sku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SkuDistributor]    Script Date: 11/16/2023 3:01:49 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_SkuDistributor] ON [dbo].[SkuDistributor]
(
	[Sku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  CONSTRAINT [DF_PurchaseOrder_Attempts]  DEFAULT ((0)) FOR [Attempts]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  CONSTRAINT [DF_PurchaseOrder_WarningNotificationCount]  DEFAULT ((0)) FOR [WarningNotificationCount]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_InventoryCheckHours]  DEFAULT ((8)) FOR [InventoryCheckHours]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_FulfillmentCheckHours]  DEFAULT ((2)) FOR [FulfillmentCheckHours]
GO
ALTER TABLE [dbo].[PurchaseOrder]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrder_Distributor] FOREIGN KEY([DistributorId])
REFERENCES [dbo].[Distributor] ([Id])
GO
ALTER TABLE [dbo].[PurchaseOrder] CHECK CONSTRAINT [FK_PurchaseOrder_Distributor]
GO
ALTER TABLE [dbo].[SkuDistributor]  WITH CHECK ADD  CONSTRAINT [FK_SkuDistributor_Distributor] FOREIGN KEY([DistributorId])
REFERENCES [dbo].[Distributor] ([Id])
GO
ALTER TABLE [dbo].[SkuDistributor] CHECK CONSTRAINT [FK_SkuDistributor_Distributor]
GO
USE [master]
GO
ALTER DATABASE [MakeMyCapServer] SET  READ_WRITE 
GO
