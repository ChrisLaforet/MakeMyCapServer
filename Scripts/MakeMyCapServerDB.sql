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
/****** Object:  Table [dbo].[Distributor]    Script Date: 11/22/2023 8:55:08 AM ******/
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
/****** Object:  Table [dbo].[DistributorSkuMap]    Script Date: 11/22/2023 8:55:08 AM ******/
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
/****** Object:  Table [dbo].[EmailQueue]    Script Date: 11/22/2023 8:55:08 AM ******/
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
/****** Object:  Table [dbo].[FulfillmentOrder]    Script Date: 11/22/2023 8:55:08 AM ******/
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
/****** Object:  Table [dbo].[Order]    Script Date: 11/22/2023 8:55:08 AM ******/
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
	[DeliverToName] [varchar](255) NULL,
	[DeliverToAddress1] [varchar](255) NULL,
	[DeliverToAddress2] [varchar](255) NULL,
	[DeliverToCity] [varchar](255) NULL,
	[DeliverToStateProv] [varchar](10) NULL,
	[DeliverToZipPC] [varchar](20) NULL,
	[DeliverToCountry] [varchar](10) NULL,
 CONSTRAINT [PK_Order_1] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderLineItem]    Script Date: 11/22/2023 8:55:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderLineItem](
	[LineItemId] [bigint] NOT NULL,
	[FulfillmentOrderId] [bigint] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Sku] [varchar](50) NOT NULL,
	[Name] [varchar](1000) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[VariantId] [bigint] NOT NULL,
	[PONumber] [varchar](25) NULL,
	[ShopifyName] [varchar](1024) NULL,
 CONSTRAINT [PK_OrderLineItem] PRIMARY KEY CLUSTERED 
(
	[LineItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/22/2023 8:55:08 AM ******/
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
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 11/22/2023 8:55:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[DistributorId] [int] NOT NULL,
	[PONumber] [varchar](25) NOT NULL,
	[PoNumberSequence] [int] NOT NULL,
	[ShopifyOrderId] [bigint] NULL,
	[Sku] [varchar](20) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Style] [varchar](50) NULL,
	[Color] [varchar](50) NULL,
	[Size] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[Correlation] [varchar](100) NULL,
	[ImageOrText] [varchar](255) NULL,
	[Position] [varchar](50) NULL,
	[SpecialInstructions] [varchar](4000) NULL,
	[SubmittedDateTime] [datetime2](7) NOT NULL,
	[LastAttemptDateTime] [datetime2](7) NULL,
	[SuccessDateTime] [datetime2](7) NULL,
	[Attempts] [int] NOT NULL,
	[WarningNotificationCount] [int] NOT NULL,
	[FailureNotificationDateTime] [datetime2](7) NULL,
	[ShopifyName] [varchar](1024) NULL,
	[Supplier] [varchar](50) NULL,
	[SupplierPoNumber] [varchar](25) NULL,
 CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceLog]    Script Date: 11/22/2023 8:55:08 AM ******/
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
/****** Object:  Table [dbo].[Settings]    Script Date: 11/22/2023 8:55:08 AM ******/
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
/****** Object:  Table [dbo].[InHouseInventory]    Script Date: 3/28/2024 2:05:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InHouseInventory](
	[Sku] [varchar](30) NOT NULL,
	[OnHand] [int] NOT NULL,
	[LastUsage] [int] NOT NULL,
 CONSTRAINT [PK_InHouseInventory] PRIMARY KEY CLUSTERED 
(
	[Sku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[InHouseInventory] ADD  CONSTRAINT [DF_InHouseInventory_LastUsage]  DEFAULT ((0)) FOR [LastUsage]
GO
/****** Object:  Table [dbo].[Shipping]    Script Date: 11/22/2023 8:55:08 AM ******/
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
SET IDENTITY_INSERT [dbo].[Distributor] OFF
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020001', N'CA', NULL, NULL, N'I1002', N'I1002-Black', N'Black - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020002', N'CA', NULL, NULL, N'I1002', N'I1002-Brown', N'Brown - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020003', N'CA', NULL, NULL, N'I1002', N'I1002-Burnt Orange', N'Burnt Orange - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020004', N'CA', NULL, NULL, N'I1002', N'I1002-Charcoal', N'Charcoal - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020005', N'CA', NULL, NULL, N'I1002', N'I1002-Forest Green', N'Forest Green - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020006', N'CA', NULL, NULL, N'I1002', N'I1002-Irish Green', N'Irish Green - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020007', N'CA', NULL, NULL, N'I1002', N'I1002-Khaki', N'Khaki - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020008', N'CA', NULL, NULL, N'I1002', N'I1002-Light Navy', N'Light Navy - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020009', N'CA', NULL, NULL, N'I1002', N'I1002-Navy', N'Navy - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020010', N'CA', NULL, NULL, N'I1002', N'I1002-Orange', N'Orange - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020011', N'CA', NULL, NULL, N'I1002', N'I1002-Pink', N'Pink - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020012', N'CA', NULL, NULL, N'I1002', N'I1002-Purple', N'Purple - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020013', N'CA', NULL, NULL, N'I1002', N'I1002-Royal', N'Royal - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020014', N'CA', NULL, NULL, N'I1002', N'I1002-Sage', N'Sage - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020015', N'CA', NULL, NULL, N'I1002', N'I1002-Scuba', N'Scuba - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020016', N'CA', NULL, NULL, N'I1002', N'I1002-Stone', N'Stone - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020017', N'CA', NULL, NULL, N'I1002', N'I1002-White', N'White - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020018', N'CA', NULL, NULL, N'I1002', N'I1002-Wine', N'Wine - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020019', N'CA', NULL, NULL, N'I1002', N'I1002-Green', N'Green - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020020', N'CA', NULL, NULL, N'I1002', N'I1002-Sky Blue', N'Sky Blue - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020021', N'CA', NULL, NULL, N'I1002', N'I1002-Yellow', N'Yellow - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020022', N'CA', NULL, NULL, N'I1002', N'i1002-Mint', N'Mint - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020023', N'CA', NULL, NULL, N'I1002', N'i1002-Gold', N'Gold - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020024', N'CA', NULL, NULL, N'I1002', N'i1002-Melon', N'Melon - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020025', N'CA', NULL, NULL, N'I1002', N'i1002-Gray', N'Gray - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020026', N'CA', NULL, NULL, N'I1002', N'i1002-Smoke Blue', N'Smoke Blue - i1002', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020027', N'CA', NULL, NULL, N'I1002', N'i1002-White/Black', N'White/Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020028', N'CA', NULL, NULL, N'I1002', N'i1002-White/Dark Gray', N'White/Dark Gray', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020029', N'CA', NULL, NULL, N'I1002', N'i1002-White/Navy', N'White/Navy', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020030', N'CA', NULL, NULL, N'I1002', N'i1002-White/Royal', N'White/Royal', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020031', N'CA', NULL, NULL, N'I1002', N'i1002-Lavender', N'Lavender', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020032', N'CA', NULL, NULL, N'I1002', N'i1002-Plum', N'Plum', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020033', N'CA', NULL, NULL, N'I1002', N'i1002-White/Smoke Blue', N'White/Smoke Blue', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI10020034', N'CA', NULL, NULL, N'I1002', N'i1002-Dark Gray/Black', N'Dark Gray/Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300001', N'CA', NULL, NULL, N'I2030', N'I2030-Blaze', N'Blaze - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300002', N'CA', NULL, NULL, N'I2030', N'I2030-Mossy Oak-¼+å Break-Up-¼+å', N'Mossy Oak-¼+å Break-Up-¼+å - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300003', N'CA', NULL, NULL, N'I2030', N'I2030-Mossy Oak-¼+å Break-Up Country-¼+å', N'Mossy Oak-¼+å Break-Up Country-¼+å - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300004', N'CA', NULL, NULL, N'I2030', N'I2030-Realtree APGÇÜ+æ-ó', N'Realtree APGÇÜ+æ-ó - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300005', N'CA', NULL, NULL, N'I2030', N'I2030-Realtree AP-¼+å Snow Camo', N'Realtree AP-¼+å Snow Camo - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300006', N'CA', NULL, NULL, N'I2030', N'I2030-Realtree APGGÇÜ+æ-ó', N'Realtree APGGÇÜ+æ-ó - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300007', N'CA', NULL, NULL, N'I2030', N'I2030-Realtree Max-5-¼+å', N'Realtree Max-5-¼+å - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300008', N'CA', NULL, NULL, N'I2030', N'I2030-Realtree Advantage TimberGÇÜ+æ-ó', N'Realtree Advantage TimberGÇÜ+æ-ó - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300009', N'CA', NULL, NULL, N'I2030', N'I2030-Realtree Xtra-¼+å', N'Realtree Xtra-¼+å - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI20300010', N'CA', NULL, NULL, N'I2030', N'i2030-Mossy Oak-¼+å Original Bottomland-¼+å', N'Mossy Oak-¼+å Original Bottomland-¼+å - i2030', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270001', N'CA', NULL, NULL, N'I3027', N'I3027-Black/Stone', N'Black/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270002', N'CA', NULL, NULL, N'I3027', N'I3027-Khaki/Stone', N'Khaki/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270003', N'CA', NULL, NULL, N'I3027', N'I3027-Maroon/Stone', N'Maroon/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270004', N'CA', NULL, NULL, N'I3027', N'I3027-Navy/Stone', N'Navy/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270005', N'CA', NULL, NULL, N'I3027', N'I3027-Olive/Stone', N'Olive/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270006', N'CA', NULL, NULL, N'I3027', N'i3027-Brown/Stone', N'Brown/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270007', N'CA', NULL, NULL, N'I3027', N'i3027-Mustard/Stone', N'Mustard/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270008', N'CA', NULL, NULL, N'I3027', N'i3027-Black/Red/Stone', N'Black/Red/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270009', N'CA', NULL, NULL, N'I3027', N'i3027-Khaki/Black/Stone', N'Khaki/Black/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270010', N'CA', NULL, NULL, N'I3027', N'i3027-Navy/Black/Stone', N'Navy/Black/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270011', N'CA', NULL, NULL, N'I3027', N'i3027-Olive/Khaki/Stone', N'Olive/Khaki/Stone - i3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30270012', N'CA', NULL, NULL, N'I3027', N'I3027-Navy/Red/Stone', N'Navy/Red/Stone-I3027', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380001', N'CA', NULL, NULL, N'I3038', N'i3038-Black', N'Black - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380002', N'CA', NULL, NULL, N'I3038', N'i3038-Black/White', N'Black/White - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380003', N'CA', NULL, NULL, N'I3038', N'i3038-Charcoal/Black', N'Charcoal/Black - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380004', N'CA', NULL, NULL, N'I3038', N'i3038-Dark Green/White', N'Dark Green/White - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380005', N'CA', NULL, NULL, N'I3038', N'i3038-Heather/Amber/Stone', N'Heather/Amber/Stone - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380006', N'CA', NULL, NULL, N'I3038', N'i3038-Loden/Black', N'Loden/Black - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380007', N'CA', NULL, NULL, N'I3038', N'i3038-Marine Blue/Charcoal', N'Marine Blue/Charcoal - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380008', N'CA', NULL, NULL, N'I3038', N'i3038-Navy', N'Navy - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380009', N'CA', NULL, NULL, N'I3038', N'i3038-Navy/White', N'Navy/White - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380010', N'CA', NULL, NULL, N'I3038', N'i3038-Red/White', N'Red/White - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380011', N'CA', NULL, NULL, N'I3038', N'i3038-Royal/White', N'Royal/White - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380012', N'CA', NULL, NULL, N'I3038', N'i3038-Heather/Red/Black', N'Heather/Red/Black - i3038', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380013', N'CA', NULL, NULL, N'I3038', N'i3038-Heather/Black', N'Heather/Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380014', N'CA', NULL, NULL, N'I3038', N'i3038-Heather/White', N'Heather/White', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380015', N'CA', NULL, NULL, N'I3038', N'i3038-Heather/Loden/Stone', N'Heather/Loden/Stone', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380016', N'CA', NULL, NULL, N'I3038', N'i3038-White/Black/Gray', N'White/Black/Gray', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380017', N'CA', NULL, NULL, N'I3038', N'i3038-White/Navy/Gray', N'White/Navy/Gray', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380018', N'CA', NULL, NULL, N'I3038', N'i3038-Caramel/Black', N'Caramel/Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380019', N'CA', NULL, NULL, N'I3038', N'i3038-Caramel/Stone', N'Caramel/Stone', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380020', N'CA', NULL, NULL, N'I3038', N'i3038-Heather/Kelly Green', N'Heather/Kelly Green', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380021', N'CA', NULL, NULL, N'I3038', N'i3038-Navy/Stone', N'Navy/Stone', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380022', N'CA', NULL, NULL, N'I3038', N'i3038-White-Amber-Black', N'White/Amber/Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380023', N'CA', NULL, NULL, N'I3038', N'i3038-White-Amber-Navy', N'White/Amber/Navy', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380024', N'CA', NULL, NULL, N'I3038', N'i3038-Brown-Khaki', N'Brown-Khaki', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380025', N'CA', NULL, NULL, N'I3038', N'i3038-Nantucket-Red-Charcoal', N'Nantucket Red/Charcoal', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380026', N'CA', NULL, NULL, N'I3038', N'i3038-White', N'White', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380027', N'CA', NULL, NULL, N'I3038', N'i3038-Khaki-Brown', N'Khaki-Brown', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI30380028', N'CA', NULL, NULL, N'I3038', N'i3038-Slate-Blue-Charcoal', N'Slate Blue/Charcoal', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020001', N'CA', NULL, NULL, N'I8502', N'i8502_ATHLETIC GOLD/BLACK/WHITE_0SFM_OSFM', N'ATHLETIC GOLD/BLACK/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020002', N'CA', NULL, NULL, N'I8502', N'i8502_ATHLETIC GOLD/NAVY/WHITE_0SFM', N'ATHLETIC GOLD/NAVY/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020003', N'CA', NULL, NULL, N'I8502', N'i8502_BLACK/RED/WHITE_0SFM_OSFM', N'BLACK/RED/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020004', N'CA', NULL, NULL, N'I8502', N'i8502_BLACK/ROYAL/WHITE_0SFM', N'BLACK/ROYAL/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020005', N'CA', NULL, NULL, N'I8502', N'i8502_COLUMBIA/NAVY/WHITE_0SFM', N'COLUMBIA/NAVY/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020006', N'CA', NULL, NULL, N'I8502', N'i8502_MAROON/BLACK/WHITE_0SFM', N'MAROON/BLACK/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020007', N'CA', NULL, NULL, N'I8502', N'i8502_NAVY/RED/WHITE_0SFM', N'NAVY/RED/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020008', N'CA', NULL, NULL, N'I8502', N'i8502_ORANGE/BLACK/WHITE_0SFM', N'ORANGE/BLACK/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020009', N'CA', NULL, NULL, N'I8502', N'i8502_WHITE/NAVY/RED_0SFM', N'WHITE/NAVY/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020010', N'CA', NULL, NULL, N'I8502', N'i8502_WHITE/ROYAL/RED_0SFM', N'WHITE/ROYAL/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020011', N'CA', NULL, NULL, N'I8502', N'i8502_BLACK/WHITE_0SFM', N'BLACK/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020012', N'CA', NULL, NULL, N'I8502', N'i8502_DARK GREEN/WHITE_0SFM', N'DARK GREEN/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020013', N'CA', NULL, NULL, N'I8502', N'i8502_GRAPHITE/BLACK_0SFM', N'GRAPHITE/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020014', N'CA', NULL, NULL, N'I8502', N'i8502_ORANGE/WHITE_0SFM', N'ORANGE/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020015', N'CA', NULL, NULL, N'I8502', N'i8502_NAVY/WHITE_0SFM', N'NAVY/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020016', N'CA', NULL, NULL, N'I8502', N'i8502_GRAPHITE/WHITE_0SFM', N'GRAPHITE/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020017', N'CA', NULL, NULL, N'I8502', N'i8502_GRAPHITE/PINK_0SFM', N'GRAPHITE/PINK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020018', N'CA', NULL, NULL, N'I8502', N'i8502_GRAPHITE/NEON YELLOW_0SFM', N'GRAPHITE/NEON YELLOW', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020019', N'CA', NULL, NULL, N'I8502', N'i8502_RED/WHITE_0SFM', N'RED/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020020', N'CA', NULL, NULL, N'I8502', N'i8502_WHITE_0SFM', N'WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020021', N'CA', NULL, NULL, N'I8502', N'i8502_NAVY_0SFM', N'NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020022', N'CA', NULL, NULL, N'I8502', N'i8502_BLACK_0SFM', N'BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020023', N'CA', NULL, NULL, N'I8502', N'i8502_ROYAL/WHITE_0SFM', N'ROYAL/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020024', N'CA', NULL, NULL, N'I8502', N'i8502-Black/Graphite', N'BLACK/GRAPHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020025', N'CA', NULL, NULL, N'I8502', N'i8502-Cardinal/White', N'CARDINAL/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020026', N'CA', NULL, NULL, N'I8502', N'i8502-Indigo Blue/Black', N'INDIGO BLUE/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020027', N'CA', NULL, NULL, N'I8502', N'i8502-Maroon/Graphite', N'MAROON/GRAPHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020028', N'CA', NULL, NULL, N'I8502', N'i8502-Silver/White', N'SILVER/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020029', N'CA', NULL, NULL, N'I8502', N'i8502-Allure/Graphite', N'ALLURE/GRAPHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85020030', N'CA', NULL, NULL, N'I8502', N'i8502-Purple/White', N'PURPLE/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030001', N'CA', NULL, NULL, N'I8503', N'i8503_BLACK_XS', N'BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030002', N'CA', NULL, NULL, N'I8503', N'i8503_BLACK_S/M', N'BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030003', N'CA', NULL, NULL, N'I8503', N'i8503_BLACK_L/XL', N'BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030004', N'CA', NULL, NULL, N'I8503', N'i8503-Black-XXL', N'BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030005', N'CA', NULL, NULL, N'I8503', N'i8503_BLACK/ATHLETIC GOLD_XS', N'BLACK/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030006', N'CA', NULL, NULL, N'I8503', N'i8503_BLACK/ATHLETIC GOLD_S/M', N'BLACK/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030007', N'CA', NULL, NULL, N'I8503', N'i8503_BLACK/ATHLETIC GOLD_L/XL', N'BLACK/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030008', N'CA', NULL, NULL, N'I8503', N'i8503-Black/Athletic Gold-XXL', N'BLACK/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030009', N'CA', NULL, NULL, N'I8503', N'i8503_COLUMBIA BLUE_XS', N'COLUMBIA BLUE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030010', N'CA', NULL, NULL, N'I8503', N'i8503_COLUMBIA BLUE_S/M', N'COLUMBIA BLUE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030011', N'CA', NULL, NULL, N'I8503', N'i8503_COLUMBIA BLUE_L/XL', N'COLUMBIA BLUE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030012', N'CA', NULL, NULL, N'I8503', N'i8503-Columbia Blue-XXL', N'COLUMBIA BLUE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030013', N'CA', NULL, NULL, N'I8503', N'i8503_DARK GREEN_XS', N'DARK GREEN', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030014', N'CA', NULL, NULL, N'I8503', N'i8503_DARK GREEN_S/M', N'DARK GREEN', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030015', N'CA', NULL, NULL, N'I8503', N'i8503_DARK GREEN_L/XL', N'DARK GREEN', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030016', N'CA', NULL, NULL, N'I8503', N'i8503-Dark Green-XXL', N'DARK GREEN', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030017', N'CA', NULL, NULL, N'I8503', N'i8503_DARK GREEN/ATHLETIC GOLD_XS', N'DARK GREEN/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030018', N'CA', NULL, NULL, N'I8503', N'i8503_DARK GREEN/ATHLETIC GOLD_S/M', N'DARK GREEN/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030019', N'CA', NULL, NULL, N'I8503', N'i8503_DARK GREEN/ATHLETIC GOLD_L/XL', N'DARK GREEN/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030020', N'CA', NULL, NULL, N'I8503', N'i8503-Dark Green/Athletic Gold-XXL', N'DARK GREEN/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030021', N'CA', NULL, NULL, N'I8503', N'i8503_GRAPHITE_XS', N'GRAPHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030022', N'CA', NULL, NULL, N'I8503', N'i8503_GRAPHITE_S/M', N'GRAPHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030023', N'CA', NULL, NULL, N'I8503', N'i8503_GRAPHITE_L/XL', N'GRAPHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030024', N'CA', NULL, NULL, N'I8503', N'i8503-Graphite-XXL', N'GRAPHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030025', N'CA', NULL, NULL, N'I8503', N'i8503_MAROON_XS', N'MAROON', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030026', N'CA', NULL, NULL, N'I8503', N'i8503_MAROON_S/M', N'MAROON', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030027', N'CA', NULL, NULL, N'I8503', N'i8503_MAROON_L/XL', N'MAROON', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030028', N'CA', NULL, NULL, N'I8503', N'i8503-Maroon-XXL', N'MAROON', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030029', N'CA', NULL, NULL, N'I8503', N'i8503_NAVY_XS', N'NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030030', N'CA', NULL, NULL, N'I8503', N'i8503_NAVY_S/M', N'NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030031', N'CA', NULL, NULL, N'I8503', N'i8503_NAVY_L/XL', N'NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030032', N'CA', NULL, NULL, N'I8503', N'i8503-Navy-XXL', N'NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030033', N'CA', NULL, NULL, N'I8503', N'i8503_NAVY/ATHLETIC GOLD_XS', N'NAVY/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030034', N'CA', NULL, NULL, N'I8503', N'i8503_NAVY/ATHLETIC GOLD_S/M', N'NAVY/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030035', N'CA', NULL, NULL, N'I8503', N'i8503_NAVY/ATHLETIC GOLD_L/XL', N'NAVY/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030036', N'CA', NULL, NULL, N'I8503', N'i8503-Navy/Athletic Gold-XXL', N'NAVY/ATHLETIC GOLD', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030037', N'CA', NULL, NULL, N'I8503', N'i8503_ORANGE_XS', N'ORANGE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030038', N'CA', NULL, NULL, N'I8503', N'i8503_ORANGE_S/M', N'ORANGE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030039', N'CA', NULL, NULL, N'I8503', N'i8503_ORANGE_L/XL', N'ORANGE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030040', N'CA', NULL, NULL, N'I8503', N'i8503-Orange-XXL', N'ORANGE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030041', N'CA', NULL, NULL, N'I8503', N'i8503_ORANGE/BLACK/WHITE_XS', N'ORANGE/BLACK/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030042', N'CA', NULL, NULL, N'I8503', N'i8503_ORANGE/BLACK/WHITE_S/M', N'ORANGE/BLACK/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030043', N'CA', NULL, NULL, N'I8503', N'i8503_ORANGE/BLACK/WHITE_L/XL', N'ORANGE/BLACK/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030044', N'CA', NULL, NULL, N'I8503', N'i8503-Orange/Black/White-XXL', N'ORANGE/BLACK/WHITE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030045', N'CA', NULL, NULL, N'I8503', N'i8503_PURPLE_XS', N'PURPLE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030046', N'CA', NULL, NULL, N'I8503', N'i8503_PURPLE_S/M', N'PURPLE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030047', N'CA', NULL, NULL, N'I8503', N'i8503_PURPLE_L/XL', N'PURPLE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030048', N'CA', NULL, NULL, N'I8503', N'i8503-Purple-XXL', N'PURPLE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030049', N'CA', NULL, NULL, N'I8503', N'i8503_RED_XS', N'RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030050', N'CA', NULL, NULL, N'I8503', N'i8503_RED_S/M', N'RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030051', N'CA', NULL, NULL, N'I8503', N'i8503_RED_L/XL', N'RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030052', N'CA', NULL, NULL, N'I8503', N'i8503-Red-XXL', N'RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030053', N'CA', NULL, NULL, N'I8503', N'i8503_RED/BLACK_XS', N'RED/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030054', N'CA', NULL, NULL, N'I8503', N'i8503_RED/BLACK_S/M', N'RED/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030055', N'CA', NULL, NULL, N'I8503', N'i8503_RED/BLACK_L/XL', N'RED/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030056', N'CA', NULL, NULL, N'I8503', N'i8503-Red/Black-XXL', N'RED/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030057', N'CA', NULL, NULL, N'I8503', N'i8503_RED/NAVY_XS', N'RED/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030058', N'CA', NULL, NULL, N'I8503', N'i8503_RED/NAVY_S/M', N'RED/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030059', N'CA', NULL, NULL, N'I8503', N'i8503_RED/NAVY_L/XL', N'RED/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030060', N'CA', NULL, NULL, N'I8503', N'i8503-Red/Navy-XXL', N'RED/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030061', N'CA', NULL, NULL, N'I8503', N'i8503_ROYAL_XS', N'ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030062', N'CA', NULL, NULL, N'I8503', N'i8503_ROYAL_S/M', N'ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030063', N'CA', NULL, NULL, N'I8503', N'i8503_ROYAL_L/XL', N'ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030064', N'CA', NULL, NULL, N'I8503', N'i8503-Royal-XXL', N'ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030065', N'CA', NULL, NULL, N'I8503', N'i8503_ROYAL/RED_XS', N'ROYAL/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030066', N'CA', NULL, NULL, N'I8503', N'i8503_ROYAL/RED_S/M', N'ROYAL/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030067', N'CA', NULL, NULL, N'I8503', N'i8503_ROYAL/RED_L/XL', N'ROYAL/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030068', N'CA', NULL, NULL, N'I8503', N'i8503-Royal/Red-XXL', N'ROYAL/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030069', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/BLACK_XS', N'SILVER/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030070', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/BLACK_S/M', N'SILVER/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030071', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/BLACK_L/XL', N'SILVER/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030072', N'CA', NULL, NULL, N'I8503', N'i8503-Silver/Black-XXL', N'SILVER/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030073', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/NAVY_XS', N'SILVER/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030074', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/NAVY_S/M', N'SILVER/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030075', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/NAVY_L/XL', N'SILVER/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030076', N'CA', NULL, NULL, N'I8503', N'i8503-Silver/Navy-XXL', N'SILVER/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030077', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/RED_XS', N'SILVER/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030078', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/RED_S/M', N'SILVER/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030079', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/RED_L/XL', N'SILVER/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030080', N'CA', NULL, NULL, N'I8503', N'i8503-Silver/Red-XXL', N'SILVER/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030081', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/ROYAL_XS', N'SILVER/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030082', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/ROYAL_S/M', N'SILVER/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030083', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/ROYAL_L/XL', N'SILVER/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030084', N'CA', NULL, NULL, N'I8503', N'i8503-Silver/Royal-XXL', N'SILVER/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030085', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/SILVER/BLACK_XS', N'SILVER/SILVER/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030086', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/SILVER/BLACK_S/M', N'SILVER/SILVER/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030087', N'CA', NULL, NULL, N'I8503', N'i8503_SILVER/SILVER/BLACK_L/XL', N'SILVER/SILVER/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030088', N'CA', NULL, NULL, N'I8503', N'i8503-Silver/Silver/Black-XXL', N'SILVER/SILVER/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030089', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/BLACK/BLACK_XS', N'WHITE/BLACK/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030090', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/BLACK/BLACK_S/M', N'WHITE/BLACK/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030091', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/BLACK/BLACK_L/XL', N'WHITE/BLACK/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030092', N'CA', NULL, NULL, N'I8503', N'i8503-White/Black/Black-XXL', N'WHITE/BLACK/BLACK', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030093', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/NAVY_XS', N'WHITE/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030094', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/NAVY_S/M', N'WHITE/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030095', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/NAVY_L/XL', N'WHITE/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030096', N'CA', NULL, NULL, N'I8503', N'i8503-White/Navy-XXL', N'WHITE/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030097', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED_XS', N'WHITE/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030098', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED_S/M', N'WHITE/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030099', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED_L/XL', N'WHITE/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030100', N'CA', NULL, NULL, N'I8503', N'i8503-White/Red-XXL', N'WHITE/RED', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030101', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED/NAVY_XS', N'WHITE/RED/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030102', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED/NAVY_S/M', N'WHITE/RED/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030103', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED/NAVY_L/XL', N'WHITE/RED/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030104', N'CA', NULL, NULL, N'I8503', N'i8503-White/Red/Navy-XXL', N'WHITE/RED/NAVY', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030105', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED/ROYAL_XS', N'WHITE/RED/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030106', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED/ROYAL_S/M', N'WHITE/RED/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030107', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/RED/ROYAL_L/XL', N'WHITE/RED/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030108', N'CA', NULL, NULL, N'I8503', N'i8503-White/Red/Royal-XXL', N'WHITE/RED/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030109', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/ROYAL_XS', N'WHITE/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030110', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/ROYAL_S/M', N'WHITE/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030111', N'CA', NULL, NULL, N'I8503', N'i8503_WHITE/ROYAL_L/XL', N'WHITE/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030112', N'CA', NULL, NULL, N'I8503', N'i8503-White/Royal-XXL', N'WHITE/ROYAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030113', N'CA', NULL, NULL, N'I8503', N'i8503-Cardinal-XS', N'CARDINAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030114', N'CA', NULL, NULL, N'I8503', N'i8503-Cardinal-S/M', N'CARDINAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030115', N'CA', NULL, NULL, N'I8503', N'i8503-Cardinal-L/XL', N'CARDINAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030116', N'CA', NULL, NULL, N'I8503', N'i8503-Cardinal-XXL', N'CARDINAL', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030117', N'CA', NULL, NULL, N'I8503', N'i8503-Texas Orange-XS', N'TEXAS ORANGE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030118', N'CA', NULL, NULL, N'I8503', N'i8503-Texas Orange-S/M', N'TEXAS ORANGE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030119', N'CA', NULL, NULL, N'I8503', N'i8503-Texas Orange-L/XL', N'TEXAS ORANGE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030120', N'CA', NULL, NULL, N'I8503', N'i8503-Texas Orange-XXL', N'TEXAS ORANGE', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030121', N'CA', NULL, NULL, N'I8503', N'i8503-Kelly Green-XS', N'KELLY GREEN', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030122', N'CA', NULL, NULL, N'I8503', N'i8503-Kelly Green-S/M', N'KELLY GREEN', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030123', N'CA', NULL, NULL, N'I8503', N'i8503-Kelly Green-L/XL', N'KELLY GREEN', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030124', N'CA', NULL, NULL, N'I8503', N'i8503-Kelly Green-XXL', N'KELLY GREEN', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030125', N'CA', NULL, NULL, N'I8503', N'i8503-White-XS', N'White', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030126', N'CA', NULL, NULL, N'I8503', N'i8503-White-SM/MD', N'White', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030127', N'CA', NULL, NULL, N'I8503', N'i8503-White-LG/XL', N'White', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030128', N'CA', NULL, NULL, N'I8503', N'i8503-White-XXL', N'White', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030129', N'CA', NULL, NULL, N'I8503', N'i8503_White/Maroon_LG/XL', N'White/Maroon/Maroon', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030130', N'CA', NULL, NULL, N'I8503', N'i8503_White/Maroon_SM/MD', N'White/Maroon/Maroon', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030131', N'CA', NULL, NULL, N'I8503', N'i8503_White/Maroon_XS', N'White/Maroon/Maroon', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85030132', N'CA', NULL, NULL, N'I8503', N'i8503_White/Maroon_XXL', N'White/Maroon/Maroon', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070001', N'CA', NULL, NULL, N'I8507', N'i8507-Black', N'Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070002', N'CA', NULL, NULL, N'I8507', N'i8507-Graphite', N'Graphite', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070003', N'CA', NULL, NULL, N'I8507', N'i8507-Navy', N'Navy', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070004', N'CA', NULL, NULL, N'I8507', N'i8507-White', N'White', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070005', N'CA', NULL, NULL, N'I8507', N'i8507-Woodland', N'Woodland', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070006', N'CA', NULL, NULL, N'I8507', N'i8507-Graphite/Red', N'Graphite/Red', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070007', N'CA', NULL, NULL, N'I8507', N'i8507-Graphite/Royal', N'Graphite/Royal', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070008', N'CA', NULL, NULL, N'I8507', N'i8507-Sand', N'Sand', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85070009', N'CA', NULL, NULL, N'I8507', N'i8507-Silver', N'Silver', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85130001', N'CA', NULL, NULL, N'I8513', N'i8513-MultiCam-« Black/Black', N'MultiCam-« Black/Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85130002', N'CA', NULL, NULL, N'I8513', N'i8513-MultiCam-« Original/Black', N'MultiCam-« Original/Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85130003', N'CA', NULL, NULL, N'I8513', N'i8513-MultiCam-« Original/Khaki', N'MultiCam-« Original/Khaki', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200001', N'CA', NULL, NULL, N'I8520', N'i8520-Black', N'Black_OSFM - i8520', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200002', N'CA', NULL, NULL, N'I8520', N'i8520-Graphite', N'Graphite - i8520', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200003', N'CA', NULL, NULL, N'I8520', N'i8520-Navy', N'Navy - i8520', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200004', N'CA', NULL, NULL, N'I8520', N'i8520-White', N'White - i8520', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200005', N'CA', NULL, NULL, N'I8520', N'i8520-Dark Green', N'Dark Green', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200006', N'CA', NULL, NULL, N'I8520', N'i8520-Maroon', N'Maroon', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200007', N'CA', NULL, NULL, N'I8520', N'i8520-Orange', N'Orange', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200008', N'CA', NULL, NULL, N'I8520', N'i8520-Purple', N'Purple', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200009', N'CA', NULL, NULL, N'I8520', N'i8520-Red', N'Red', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200010', N'CA', NULL, NULL, N'I8520', N'i8520-Royal', N'Royal', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAI85200011', N'CA', NULL, NULL, N'I8520', N'i8520-Silver', N'Silver', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650001', N'CA', NULL, NULL, N'IK65', N'IK65-Dark Heather/Heather/White', N'Dark Heather/Heather/White - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650002', N'CA', NULL, NULL, N'IK65', N'IK65-Black/Heather/White', N'Black-Heather-White-iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650003', N'CA', NULL, NULL, N'IK65', N'IK65-Navy/Heather/White', N'Navy/Heather/White - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650004', N'CA', NULL, NULL, N'IK65', N'IK65-Red/Heather/White', N'Red/Heather/White - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650005', N'CA', NULL, NULL, N'IK65', N'iK65-Black/Gold/White', N'Black/Gold/White - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650006', N'CA', NULL, NULL, N'IK65', N'iK65-Royal/Dark Heather/White', N'Royal/Dark Heather/White - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650007', N'CA', NULL, NULL, N'IK65', N'iK65-Cardinal/Dark Heather/white', N'Cardinal/Dark Heather/white - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650008', N'CA', NULL, NULL, N'IK65', N'iK65-Hunter/Heather/White', N'Hunter/Heather/White - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650009', N'CA', NULL, NULL, N'IK65', N'iK65-Navy/Red/White', N'Navy/Red/White - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK650010', N'CA', NULL, NULL, N'IK65', N'iK65-Orange/Black/White', N'Orange/Black/White - iK65', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500001', N'CA', NULL, NULL, N'IK8550', N'iK8550_cider', N'Cider', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500002', N'CA', NULL, NULL, N'IK8550', N'iK8550_black', N'Black', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500003', N'CA', NULL, NULL, N'IK8550', N'iK8550_dark heather', N'Dark Heather', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500004', N'CA', NULL, NULL, N'IK8550', N'iK8550_hunter', N'Hunter', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500005', N'CA', NULL, NULL, N'IK8550', N'iK8550_ivory', N'Ivory', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500006', N'CA', NULL, NULL, N'IK8550', N'iK8550_navy', N'Navy', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500007', N'CA', NULL, NULL, N'IK8550', N'iK8550_red', N'Red', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500008', N'CA', NULL, NULL, N'IK8550', N'iK8550_royal', N'Royal', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500009', N'CA', NULL, NULL, N'IK8550', N'iK8550_silver', N'Silver', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500010', N'CA', NULL, NULL, N'IK8550', N'iK8550-Heather', N'Heather', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'CAIK85500011', N'CA', NULL, NULL, N'IK8550', N'iK8550-Olive', N'Olive', NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SM5800870001', N'SM', NULL, N'Nike', N'580087', N'Deep Black', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SM5800870002', N'SM', NULL, N'Nike', N'580087', N'True White', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SM5800870003', N'SM', NULL, N'Nike', N'580087', N'Deep Navy', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SM5800870004', N'SM', NULL, N'Nike', N'580087', N'Dark Khaki', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SM5800870005', N'SM', NULL, N'Nike', N'580087', N'Fusion Pink', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SM5800870006', N'SM', NULL, N'Nike', N'580087', N'Dark Grey', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SM5800870007', N'SM', NULL, N'Nike', N'580087', N'Game Royal', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SM5800870008', N'SM', NULL, N'Nike', N'580087', N'Gym Red', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMCJ70820001', N'SM', NULL, N'Nike', N'CJ7082', N'Black', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMCJ70820002', N'SM', NULL, N'Nike', N'CJ7082', N'White', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMCJ70820003', N'SM', NULL, N'Nike', N'CJ7082', N'College Navy', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMCJ70820004', N'SM', NULL, N'Nike', N'CJ7082', N'Game Royal', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMCJ70820005', N'SM', NULL, N'Nike', N'CJ7082', N'Anthracite', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE2070001', N'SM', NULL, N'New Era', N'NE207', N'Coral/White', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE2070002', N'SM', NULL, N'New Era', N'NE207', N'Heather Gry/Bk', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE2070003', N'SM', NULL, N'New Era', N'NE207', N'Black', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE2070004', N'SM', NULL, N'New Era', N'NE207', N'Deep Navy/Wht', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE2070005', N'SM', NULL, N'New Era', N'NE207', N'Camo/Black', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4000001', N'SM', NULL, N'New Era', N'NE400', N'Black', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4000002', N'SM', NULL, N'New Era', N'NE400', N'Charcoal', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4000003', N'SM', NULL, N'New Era', N'NE400', N'Charcoal/Dp Ny', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4000004', N'SM', NULL, N'New Era', N'NE400', N'White', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4000005', N'SM', NULL, N'New Era', N'NE400', N'Deep Navy', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4000006', N'SM', NULL, N'New Era', N'NE400', N'Camo', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4040001', N'SM', NULL, N'New Era', N'NE404', N'Deep Navy', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4040002', N'SM', NULL, N'New Era', N'NE404', N'Deep Orange', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMNE4040003', N'SM', NULL, N'New Era', N'NE404', N'Scarlet', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4230001', N'SM', NULL, N'TravisMathew', N'TM1MU423', N'Black', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4230002', N'SM', NULL, N'TravisMathew', N'TM1MU423', N'BlkHthr', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4230003', N'SM', NULL, N'TravisMathew', N'TM1MU423', N'HthrGrey', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4230004', N'SM', NULL, N'TravisMathew', N'TM1MU423', N'VintageIdg', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4230005', N'SM', NULL, N'TravisMathew', N'TM1MU423', N'White', NULL, NULL, NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4260001', N'SM', NULL, N'TravisMathew', N'TM1MU426', N'Black', NULL, NULL, N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4260002', N'SM', NULL, N'TravisMathew', N'TM1MU426', N'Black', NULL, NULL, N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4260003', N'SM', NULL, N'TravisMathew', N'TM1MU426', N'BlueNights', NULL, NULL, N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4260004', N'SM', NULL, N'TravisMathew', N'TM1MU426', N'BlueNights', NULL, NULL, N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4260005', N'SM', NULL, N'TravisMathew', N'TM1MU426', N'QuiShaGrey', NULL, NULL, N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4260006', N'SM', NULL, N'TravisMathew', N'TM1MU426', N'QuiShaGrey', NULL, NULL, N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4260007', N'SM', NULL, N'TravisMathew', N'TM1MU426', N'White', NULL, NULL, N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SMTM1MU4260008', N'SM', NULL, N'TravisMathew', N'TM1MU426', N'White', NULL, NULL, N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB05924090', N'SS', N'B05924090', N'Columbia', N'10281', NULL, N'Chalk', N'9', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB05924500', N'SS', N'B05924500', N'Columbia', N'10281', NULL, N'Black', N'50', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB05924750', N'SS', N'B05924750', N'Columbia', N'10281', NULL, N'Nocturnal', N'75', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB11595000', N'SS', N'B11595000', N'Imperial', N'11606', NULL, N'White', N'0', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB11595090', N'SS', N'B11595090', N'Imperial', N'11606', NULL, N'Grey', N'9', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB11595500', N'SS', N'B11595500', N'Imperial', N'11606', NULL, N'Black', N'50', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB11595650', N'SS', N'B11595650', N'Imperial', N'11606', NULL, N'Navy', N'65', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB11595700', N'SS', N'B11595700', N'Imperial', N'11606', NULL, N'Red', N'70', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB11595750', N'SS', N'B11595750', N'Imperial', N'11606', NULL, N'Royal', N'75', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095010', N'SS', N'B18095010', N'Richardson', N'4332', NULL, N'Navy/ White', N'1', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095011', N'SS', N'B18095011', N'Richardson', N'4332', NULL, N'Navy/ White', N'1', N'XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095020', N'SS', N'B18095020', N'Richardson', N'4332', NULL, N'Black/ White', N'2', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095021', N'SS', N'B18095021', N'Richardson', N'4332', NULL, N'Black/ White', N'2', N'XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095110', N'SS', N'B18095110', N'Richardson', N'4332', NULL, N'Charcoal/ Neon Orange', N'11', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095130', N'SS', N'B18095130', N'Richardson', N'4332', NULL, N'Charcoal/ Neon Yellow', N'13', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095150', N'SS', N'B18095150', N'Richardson', N'4332', NULL, N'Charcoal/ White', N'15', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095170', N'SS', N'B18095170', N'Richardson', N'4332', NULL, N'Red/ Black', N'17', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095180', N'SS', N'B18095180', N'Richardson', N'4332', NULL, N'Charcoal/ Neon Blue', N'18', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095470', N'SS', N'B18095470', N'Richardson', N'4332', NULL, N'Heather Grey/ Birch/ Amber Gold', N'47', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095480', N'SS', N'B18095480', N'Richardson', N'4332', NULL, N'Heather Grey/ Birch/ Army Olive', N'48', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095500', N'SS', N'B18095500', N'Richardson', N'4332', NULL, N'Black', N'50', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095501', N'SS', N'B18095501', N'Richardson', N'4332', NULL, N'Black', N'50', N'XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095510', N'SS', N'B18095510', N'Richardson', N'4332', NULL, N'Heather Grey/ Black', N'51', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095511', N'SS', N'B18095511', N'Richardson', N'4332', NULL, N'Heather Grey/ Black', N'51', N'XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095690', N'SS', N'B18095690', N'Richardson', N'4332', NULL, N'Loden/ Black', N'69', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB18095890', N'SS', N'B18095890', N'Richardson', N'4332', NULL, N'Biscuit/ True Blue', N'89', N'OSFM')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49295100', N'SS', N'B49295100', N'Richardson', N'6847', NULL, N'Mossy Oak Country DNA/ Black', N'10', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49295300', N'SS', N'B49295300', N'Richardson', N'6847', NULL, N'Mossy Oak Bottomland/ Black', N'30', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49295480', N'SS', N'B49295480', N'Richardson', N'6847', NULL, N'Shadow Grass Habitat/ Brown', N'48', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49295540', N'SS', N'B49295540', N'Richardson', N'6847', NULL, N'Army Camo/ Black', N'54', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49295960', N'SS', N'B49295960', N'Richardson', N'6847', NULL, N'Realtree Timber/ Black', N'96', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49695000', N'SS', N'B49695000', N'47 Brand', N'9182', NULL, N'White', N'0', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49695090', N'SS', N'B49695090', N'47 Brand', N'9182', NULL, N'Charcoal', N'9', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49695260', N'SS', N'B49695260', N'47 Brand', N'9182', NULL, N'Columbia Blue', N'26', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49695500', N'SS', N'B49695500', N'47 Brand', N'9182', NULL, N'Black', N'50', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49695650', N'SS', N'B49695650', N'47 Brand', N'9182', NULL, N'Navy', N'65', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB49695750', N'SS', N'B49695750', N'47 Brand', N'9182', NULL, N'Royal', N'75', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB51195010', N'SS', N'B51195010', N'Imperial', N'9186', NULL, N'Navy/ White', N'1', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB51195020', N'SS', N'B51195020', N'Imperial', N'9186', NULL, N'Black/ White', N'2', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB51195040', N'SS', N'B51195040', N'Imperial', N'9186', NULL, N'Grey/ Black', N'4', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB51195060', N'SS', N'B51195060', N'Imperial', N'9186', NULL, N'White/ Black', N'6', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB51195080', N'SS', N'B51195080', N'Imperial', N'9186', NULL, N'White/ Navy-Red', N'8', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB51195110', N'SS', N'B51195110', N'Imperial', N'9186', NULL, N'White/ Red', N'11', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58195140', N'SS', N'B58195140', N'Atlantis Headwear', N'10242', NULL, N'Mustard Yellow', N'14', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58195240', N'SS', N'B58195240', N'Atlantis Headwear', N'10242', NULL, N'Light Beige', N'24', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58195500', N'SS', N'B58195500', N'Atlantis Headwear', N'10242', NULL, N'Black', N'50', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58195530', N'SS', N'B58195530', N'Atlantis Headwear', N'10242', NULL, N'Burgundy', N'53', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58195540', N'SS', N'B58195540', N'Atlantis Headwear', N'10242', NULL, N'Green Bottle', N'54', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58195590', N'SS', N'B58195590', N'Atlantis Headwear', N'10242', NULL, N'Dark Grey', N'59', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58195650', N'SS', N'B58195650', N'Atlantis Headwear', N'10242', NULL, N'Navy', N'65', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58195660', N'SS', N'B58195660', N'Atlantis Headwear', N'10242', NULL, N'Rusty', N'66', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58895140', N'SS', N'B58895140', N'Imperial', N'10657', NULL, N'Sunshine/ White', N'14', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58895170', N'SS', N'B58895170', N'Imperial', N'10657', NULL, N'Sea Green/ White', N'17', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58895240', N'SS', N'B58895240', N'Imperial', N'10657', NULL, N'Putty/ White', N'24', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB58895250', N'SS', N'B58895250', N'Imperial', N'10657', NULL, N'Powder Blue/ White', N'25', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB59795120', N'SS', N'B59795120', N'Imperial', N'10309', NULL, N'Blue Hawai''in ', N'12', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB59795150', N'SS', N'B59795150', N'Imperial', N'10309', NULL, N'Throwback Black', N'15', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB59795160', N'SS', N'B59795160', N'Imperial', N'10309', NULL, N'Blue Waves', N'16', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB59795170', N'SS', N'B59795170', N'Imperial', N'10309', NULL, N'Desert', N'17', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB79195300', N'SS', N'B79195300', N'Richardson', N'6423', NULL, N'Hot Pink/ Black', N'30', N'M/L')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB79195480', N'SS', N'B79195480', N'Richardson', N'6423', NULL, N'Heather Grey/ Birch/ Cardinal', N'48', N'M/L')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB79195550', N'SS', N'B79195550', N'Richardson', N'6423', NULL, N'Brown/ Khaki', N'55', N'M/L')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB79195580', N'SS', N'B79195580', N'Richardson', N'6423', NULL, N'Heather Grey/ Birch/ Amber Gold', N'58', N'M/L')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB79195640', N'SS', N'B79195640', N'Richardson', N'6423', NULL, N'Loden', N'64', N'M/L')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB79195680', N'SS', N'B79195680', N'Richardson', N'6423', NULL, N'Smoke Blue/ Aluminum', N'68', N'M/L')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395093', N'SS', N'B90395093', N'Richardson', N'6461', NULL, N'Charcoal/ Black Split', N'9', N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395095', N'SS', N'B90395095', N'Richardson', N'6461', NULL, N'Charcoal/ Black Split', N'9', N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395503', N'SS', N'B90395503', N'Richardson', N'6461', NULL, N'Black/ White Split', N'50', N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395505', N'SS', N'B90395505', N'Richardson', N'6461', NULL, N'Black/ White Split', N'50', N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395513', N'SS', N'B90395513', N'Richardson', N'6461', NULL, N'Black/ Charcoal Split', N'51', N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395515', N'SS', N'B90395515', N'Richardson', N'6461', NULL, N'Black/ Charcoal Split', N'51', N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395703', N'SS', N'B90395703', N'Richardson', N'6461', NULL, N'Red/ Charcoal/ Black Tri', N'70', N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395705', N'SS', N'B90395705', N'Richardson', N'6461', NULL, N'Red/ Charcoal/ Black Tri', N'70', N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395743', N'SS', N'B90395743', N'Richardson', N'6461', NULL, N'Royal/ White Split', N'74', N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395823', N'SS', N'B90395823', N'Richardson', N'6461', NULL, N'White/ Black/ Orange Tri', N'82', N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395825', N'SS', N'B90395825', N'Richardson', N'6461', NULL, N'White/ Black/ Orange Tri', N'82', N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395833', N'SS', N'B90395833', N'Richardson', N'6461', NULL, N'White/ Columbia Blue/ Navy Tri', N'83', N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395835', N'SS', N'B90395835', N'Richardson', N'6461', NULL, N'White/ Columbia Blue/ Navy Tri', N'83', N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395843', N'SS', N'B90395843', N'Richardson', N'6461', NULL, N'White/ Columbia Blue/ Red Tri', N'84', N'S/M')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB90395845', N'SS', N'B90395845', N'Richardson', N'6461', NULL, N'White/ Columbia Blue/ Red Tri', N'84', N'L/XL')
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB97295000', N'SS', N'B97295000', N'Infinity Her', N'11290', NULL, N'White/ Floral', N'0', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB97295250', N'SS', N'B97295250', N'Infinity Her', N'11290', NULL, N'Cashmere Blue/ Floral', N'25', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB97295300', N'SS', N'B97295300', N'Infinity Her', N'11290', NULL, N'Dusty Pink/ Floral', N'30', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB97295400', N'SS', N'B97295400', N'Infinity Her', N'11290', NULL, N'Sunset Yellow/ Polka Dots', N'40', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB97295500', N'SS', N'B97295500', N'Infinity Her', N'11290', NULL, N'Black/ Leopard', N'50', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB97295670', N'SS', N'B97295670', N'Infinity Her', N'11290', NULL, N'Lavender/ Stripes', N'67', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB97295700', N'SS', N'B97295700', N'Infinity Her', N'11290', NULL, N'Red/ Leopard', N'70', NULL)
GO
INSERT [dbo].[DistributorSkuMap] ([Sku], [DistributorCode], [DistributorSku], [Brand], [StyleCode], [PartId], [Color], [ColorCode], [SizeCode]) VALUES (N'SSB97295750', N'SS', N'B97295750', N'Infinity Her', N'11290', NULL, N'Royal/ Floral', N'75', NULL)
GO
INSERT [dbo].[Settings] ([InventoryCheckHours], [FulfillmentCheckHours], [NextPOSequence], [StatusEmailRecipient1], [StatusEmailRecipient2], [StatusEmailRecipient3], [CriticalEmailRecipient1], [CriticalEmailRecipient2], [CriticalEmailRecipient3]) VALUES (8, 1, 1, N'beth@makemycap.com', NULL, N'laforet@chrislaforetsoftware.com', N'beth@makemycap.com', N'john@makemycap.com', N'laforet@chrislaforetsoftware.com')
GO
SET IDENTITY_INSERT [dbo].[Shipping] ON 
GO
INSERT [dbo].[Shipping] ([Id], [ShipTo], [Name], [ShipAddress], [ShipCity], [ShipState], [ShipZip], [ShipEmail], [ShipMethod], [Attention]) VALUES (2, N'Cap America Annex', N'CapAmerica', N'1 Cap America Drive', N'Fredrickstown', N'MO', N'63645', N'webmaster', N'FedEx', NULL)
GO
SET IDENTITY_INSERT [dbo].[Shipping] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Distributor]    Script Date: 11/22/2023 8:55:08 AM ******/
ALTER TABLE [dbo].[Distributor] ADD  CONSTRAINT [IX_Distributor] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Distributor_1]    Script Date: 11/22/2023 8:55:08 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Distributor_1] ON [dbo].[Distributor]
(
	[LookupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DistributorSkuMap]    Script Date: 11/22/2023 8:55:08 AM ******/
CREATE NONCLUSTERED INDEX [IX_DistributorSkuMap] ON [dbo].[DistributorSkuMap]
(
	[DistributorSku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DistributorSkuMap_1]    Script Date: 11/22/2023 8:55:08 AM ******/
CREATE NONCLUSTERED INDEX [IX_DistributorSkuMap_1] ON [dbo].[DistributorSkuMap]
(
	[StyleCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EmailQueue]    Script Date: 11/22/2023 8:55:08 AM ******/
CREATE NONCLUSTERED INDEX [IX_EmailQueue] ON [dbo].[EmailQueue]
(
	[SentDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Order]    Script Date: 11/22/2023 8:55:08 AM ******/
CREATE NONCLUSTERED INDEX [IX_Order] ON [dbo].[Order]
(
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product]    Script Date: 11/22/2023 8:55:08 AM ******/
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [IX_Product] UNIQUE NONCLUSTERED 
(
	[VariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Product_1]    Script Date: 11/22/2023 8:55:08 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Product_1] ON [dbo].[Product]
(
	[Sku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
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

USE [master]
GO
ALTER DATABASE [MakeMyCapServer] SET  READ_WRITE 
GO
