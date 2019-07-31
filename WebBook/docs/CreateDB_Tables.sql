CREATE DATABASE [WebBook];
GO

USE [WebBook];
GO

CREATE TABLE [dbo].[Users] (
    [id]       INT          IDENTITY (1, 1) NOT NULL,
    [Password] VARCHAR (15) NOT NULL,
    [Email]    VARCHAR (35) NOT NULL,
    [Role]     VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[Books] (
    [id]             INT             IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)    NOT NULL,
    [Picture]        VARBINARY (MAX) NULL,
    [Category]       VARCHAR (20)    NOT NULL,
    [Author]         VARCHAR (20)    NOT NULL,
    [Description]    VARCHAR (MAX)   NULL,
    [Publish]        DATE            NOT NULL,
    [Publish_Number] INT             NOT NULL,
    [Publisher]      VARCHAR (50)    NOT NULL,
    [Pages]          INT             NOT NULL,
    [ISBN]           VARCHAR (30)    NOT NULL,
    [Barcode]        VARCHAR (30)    NOT NULL,
    [Reg_Date]       DATE            NOT NULL,
    [Out_Date]       DATE            NULL,
    [Issued]         BIT             NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[History] (
    [id]          INT  IDENTITY (1, 1) NOT NULL,
    [Book_Id]     INT  NOT NULL,
    [User_Id]     INT  NOT NULL,
    [Issue]       DATE NOT NULL,
    [Return_Date] DATE NULL,
    CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_History_Books] FOREIGN KEY ([Book_Id]) REFERENCES [dbo].[Books] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_History_Users1] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);
