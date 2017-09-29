/*
Navicat SQL Server Data Transfer

Source Server         : 10.1.25.30
Source Server Version : 110000
Source Host           : 10.1.25.30:1433
Source Database       : jym-government
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 110000
File Encoding         : 65001

Date: 2015-12-13 12:41:05
*/


-- ----------------------------
-- Table structure for Applications
-- ----------------------------
DROP TABLE [dbo].[Applications]
GO
CREATE TABLE [dbo].[Applications] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Configurations] nvarchar(MAX) NOT NULL ,
[ConfigurationVersion] varchar(255) NOT NULL ,
[KeyId] varchar(32) NOT NULL ,
[Keys] varchar(MAX) NOT NULL ,
[Role] varchar(255) NOT NULL ,
[ServiceEndpoint] varchar(255) NOT NULL ,
[CreatedBy] nvarchar(255) NOT NULL ,
[CreatedTime] datetime2(7) NOT NULL ,
[LastModifiedBy] nvarchar(255) NOT NULL ,
[LastModifiedTime] datetime2(7) NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Applications]', RESEED, 3)
GO

-- ----------------------------
-- Table structure for ConfigurationFetchLogs
-- ----------------------------
DROP TABLE [dbo].[ConfigurationFetchLogs]
GO
CREATE TABLE [dbo].[ConfigurationFetchLogs] (
[Id] int NOT NULL IDENTITY(1,1) ,
[FetchedVersion] varchar(255) NOT NULL ,
[SourceRole] varchar(255) NOT NULL ,
[SourceRoleInstance] varchar(255) NOT NULL ,
[SourceIP] varchar(255) NOT NULL ,
[SourceVersion] varchar(255) NOT NULL ,
[Time] datetime2(7) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for OperationLogs
-- ----------------------------
DROP TABLE [dbo].[OperationLogs]
GO
CREATE TABLE [dbo].[OperationLogs] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Operation] varchar(255) NOT NULL ,
[Operator] nvarchar(255) NOT NULL ,
[Parameters] nvarchar(MAX) NOT NULL ,
[Time] datetime2(7) NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[OperationLogs]', RESEED, 4)
GO

-- ----------------------------
-- Table structure for Permissions
-- ----------------------------
DROP TABLE [dbo].[Permissions]
GO
CREATE TABLE [dbo].[Permissions] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Expiry] datetime2(7) NOT NULL ,
[ObjectApplicationId] int NOT NULL ,
[PermissionLevel] int NOT NULL ,
[SubjectApplicationId] int NOT NULL ,
[CreatedBy] nvarchar(255) NOT NULL ,
[CreatedTime] datetime2(7) NOT NULL ,
[LastModifiedBy] nvarchar(255) NOT NULL ,
[LastModifiedTime] datetime2(7) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for Staves
-- ----------------------------
DROP TABLE [dbo].[Staves]
GO
CREATE TABLE [dbo].[Staves] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Cellphone] varchar(255) NOT NULL ,
[Email] varchar(255) NOT NULL ,
[Key] varchar(255) NOT NULL ,
[Name] nvarchar(255) NOT NULL ,
[CreatedBy] nvarchar(255) NOT NULL ,
[CreatedTime] datetime2(7) NOT NULL ,
[LastModifiedBy] nvarchar(255) NOT NULL ,
[LastModifiedTime] datetime2(7) NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Staves]', RESEED, 2)
GO

-- ----------------------------
-- Indexes structure for table Applications
-- ----------------------------
CREATE UNIQUE INDEX [IX_Applications_Role] ON [dbo].[Applications]
([Role] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE UNIQUE INDEX [IX_Applications_KeyId] ON [dbo].[Applications]
([KeyId] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO

-- ----------------------------
-- Primary Key structure for table Applications
-- ----------------------------
ALTER TABLE [dbo].[Applications] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table ConfigurationFetchLogs
-- ----------------------------
CREATE INDEX [IX_ConfigurationFetchLogs_SourceRole] ON [dbo].[ConfigurationFetchLogs]
([SourceRole] ASC) 
GO
CREATE INDEX [IX_ConfigurationFetchLogs_SourceRoleInstance] ON [dbo].[ConfigurationFetchLogs]
([SourceRoleInstance] ASC) 
GO
CREATE INDEX [IX_ConfigurationFetchLogs_SourceIP] ON [dbo].[ConfigurationFetchLogs]
([SourceIP] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table ConfigurationFetchLogs
-- ----------------------------
ALTER TABLE [dbo].[ConfigurationFetchLogs] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table OperationLogs
-- ----------------------------
CREATE INDEX [IX_OperationLog_Operator] ON [dbo].[OperationLogs]
([Operator] ASC) 
GO
CREATE INDEX [IX_OperationLog_Time] ON [dbo].[OperationLogs]
([Time] DESC) 
GO

-- ----------------------------
-- Indexes structure for table Permissions
-- ----------------------------
CREATE INDEX [IX_Permissions_ObjectApplicationId] ON [dbo].[Permissions]
([ObjectApplicationId] ASC) 
GO
CREATE INDEX [IX_Permissions_SubjectApplicationId] ON [dbo].[Permissions]
([SubjectApplicationId] ASC) 
GO
CREATE INDEX [IX_Permissions_ObjectApplicationId_SubjectApplicationId] ON [dbo].[Permissions]
([ObjectApplicationId] ASC, [SubjectApplicationId] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table Permissions
-- ----------------------------
ALTER TABLE [dbo].[Permissions] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table Staves
-- ----------------------------
CREATE UNIQUE INDEX [UQ_Staves_Name] ON [dbo].[Staves]
([Name] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IX_Staves_Cellphone] ON [dbo].[Staves]
([Cellphone] ASC) 
GO
CREATE INDEX [IX_Staves_Key] ON [dbo].[Staves]
([Key] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table Staves
-- ----------------------------
ALTER TABLE [dbo].[Staves] ADD PRIMARY KEY ([Id])
GO
