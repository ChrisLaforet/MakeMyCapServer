USE [master]
GO
/****** Object:  Database [MakeMyCapServer]    Script Date: 11/16/2023 3:01:49 PM ******/
CREATE DATABASE [MakeMyCapServer]
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
USE [MakeMyCapServer]
GO
/****** Object:  Table [dbo].[Distributor]    Script Date: 11/20/2023 10:11:13 PM ******/
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
/****** Object:  Table [dbo].[DistributorSkuMap]    Script Date: 11/20/2023 10:11:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DistributorSkuMap](
	[Sku] [varchar](30) NOT NULL,
	[DistributorCode] [varchar](5) NOT NULL,
	[DistributorSku] [varchar](30) NULL,
	[Brand] [varchar](100) NULL,
	[StyleCode] [varchar](50) NOT NULL,
	[PartId] [varchar](50) NULL,
	[Color] [varchar](100) NULL,
	[ColorCode] [varchar](10) NULL,
	[SizeCode] [varchar](20) NULL,
 CONSTRAINT [PK_DistributorSkuMap] PRIMARY KEY CLUSTERED 
(
	[Sku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailQueue]    Script Date: 11/20/2023 10:11:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailQueue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sender] [varchar](120) NOT NULL,
	[Subject] [varchar](250) NOT NULL,
	[Body] [varchar](max) NOT NULL,
	[PostedDateTime] [datetime2](7) NOT NULL,
	[Recipient] [varchar](120) NOT NULL,
	[Recipient2] [varchar](120) NULL,
	[Recipient3] [varchar](120) NULL,
	[Recipient4] [varchar](120) NULL,
	[LastAttemptDateTime] [datetime2](7) NULL,
	[TotalAttempts] [int] NOT NULL,
	[SentDateTime] [datetime2](7) NULL,
	[AbandonedDateTime] [datetime2](7) NULL,
	[BodyIsHtml] [bit] NOT NULL,
 CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FulfillmentOrder]    Script Date: 11/21/2023 9:35:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FulfillmentOrder](
	[FulfillmentOrderId] [bigint] NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[Status] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FulfillmentOrder] PRIMARY KEY CLUSTERED 
(
	[FulfillmentOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/21/2023 9:35:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [bigint] NOT NULL,
	[OrderNumber] [varchar](50) NOT NULL,
	[CheckoutId] [bigint] NOT NULL,
	[CheckoutToken] [varchar](255) NULL,
	[CreatedDateTime] [datetime2](7) NOT NULL,
	[ProcessStartDateTime] [datetime2](7) NOT NULL,
	[Cancelled] [bit] NOT NULL,
 CONSTRAINT [PK_Order_1] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderLineItem]    Script Date: 11/21/2023 9:35:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderLineItem](
	[LineItemId] [bigint] NOT NULL,
	[FulfillmentOrderId] [bigint] NOT NULL,
	[ShopId] [bigint] NOT NULL,
	[Quantity] [int] NOT NULL,
	[InventoryItemId] [bigint] NOT NULL,
	[VariantId] [bigint] NOT NULL,
	[PONumber] [varchar](25) NULL,
 CONSTRAINT [PK_OrderLineItem] PRIMARY KEY CLUSTERED 
(
	[LineItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/20/2023 10:11:13 PM ******/
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
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 11/20/2023 10:11:13 PM ******/
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
/****** Object:  Table [dbo].[ServiceLog]    Script Date: 11/20/2023 10:11:13 PM ******/
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
/****** Object:  Table [dbo].[Settings]    Script Date: 11/20/2023 10:11:13 PM ******/
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
/****** Object:  Table [dbo].[Shipping]    Script Date: 11/20/2023 10:11:13 PM ******/
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
/****** Object:  Table [dbo].[SkuDistributor]    Script Date: 11/20/2023 10:11:13 PM ******/
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
INSERT [dbo].[Distributor] ([Id], [LookupCode], [Name], [AccountNumber]) VALUES (4, N'MMC', N'Make My Cap', NULL)
GO
INSERT [dbo].[Settings] ([InventoryCheckHours], [FulfillmentCheckHours], [NextPOSequence], [StatusEmailRecipient1], [StatusEmailRecipient2], [StatusEmailRecipient3], [CriticalEmailRecipient1], [CriticalEmailRecipient2], [CriticalEmailRecipient3]) VALUES (8, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Shipping] ON 
GO
INSERT [dbo].[Shipping] ([Id], [ShipTo], [Name], [ShipAddress], [ShipCity], [ShipState], [ShipZip], [ShipEmail], [ShipMethod], [Attention]) VALUES (2, N'Cap America Annex', N'CapAmerica', N'1 Cap America Drive', N'Fredrickstown', N'MO', N'63645', N'webmaster', N'FedEx', NULL)
GO
SET IDENTITY_INSERT [dbo].[Shipping] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Distributor]    Script Date: 11/20/2023 10:11:13 PM ******/
ALTER TABLE [dbo].[Distributor] ADD  CONSTRAINT [IX_Distributor] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Distributor_1]    Script Date: 11/20/2023 10:11:13 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Distributor_1] ON [dbo].[Distributor]
(
	[LookupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DistributorSkuMap]    Script Date: 11/20/2023 10:11:13 PM ******/
CREATE NONCLUSTERED INDEX [IX_DistributorSkuMap] ON [dbo].[DistributorSkuMap]
(
	[DistributorSku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DistributorSkuMap_1]    Script Date: 11/20/2023 10:11:13 PM ******/
CREATE NONCLUSTERED INDEX [IX_DistributorSkuMap_1] ON [dbo].[DistributorSkuMap]
(
	[StyleCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EmailQueue]    Script Date: 11/20/2023 10:11:13 PM ******/
CREATE NONCLUSTERED INDEX [IX_EmailQueue] ON [dbo].[EmailQueue]
(
	[SentDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product]    Script Date: 11/20/2023 10:11:13 PM ******/
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [IX_Product] UNIQUE NONCLUSTERED 
(
	[VariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Product_1]    Script Date: 11/20/2023 10:11:13 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Product_1] ON [dbo].[Product]
(
	[Sku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SkuDistributor]    Script Date: 11/20/2023 10:11:13 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_SkuDistributor] ON [dbo].[SkuDistributor]
(
	[Sku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailQueue] ADD  CONSTRAINT [DF_EmailQueue_TotalAttempts]  DEFAULT ((0)) FOR [TotalAttempts]
GO
ALTER TABLE [dbo].[EmailQueue] ADD  CONSTRAINT [DF_EmailQueue_BodyIsHtml]  DEFAULT ((0)) FOR [BodyIsHtml]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  CONSTRAINT [DF_PurchaseOrder_Attempts]  DEFAULT ((0)) FOR [Attempts]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  CONSTRAINT [DF_PurchaseOrder_WarningNotificationCount]  DEFAULT ((0)) FOR [WarningNotificationCount]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_InventoryCheckHours]  DEFAULT ((8)) FOR [InventoryCheckHours]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_FulfillmentCheckHours]  DEFAULT ((2)) FOR [FulfillmentCheckHours]
GO
ALTER TABLE [dbo].[FulfillmentOrder]  WITH CHECK ADD  CONSTRAINT [FK_FulfillmentOrder_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[FulfillmentOrder] CHECK CONSTRAINT [FK_FulfillmentOrder_Order]
GO
ALTER TABLE [dbo].[OrderLineItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderLineItem_FulfillmentOrder] FOREIGN KEY([FulfillmentOrderId])
REFERENCES [dbo].[FulfillmentOrder] ([FulfillmentOrderId])
GO
ALTER TABLE [dbo].[OrderLineItem] CHECK CONSTRAINT [FK_OrderLineItem_FulfillmentOrder]
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
