USE [ExodusKorea]
GO
/****** Object:  Table [dbo].[Log_SiteException]    Script Date: 10/18/2018 6:43:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Log_SiteException](
	[ExceptionID] [bigint] IDENTITY(1,1) NOT NULL,
	[PageUrl] [nvarchar](500) NULL,
	[UserID] [nvarchar](500) NULL,
	[CreateDate] [smalldatetime] NULL,
	[Exception] [varchar](5000) NULL,
	[IpAddress] [varchar](15) NULL,
	[Remarks] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ExceptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
