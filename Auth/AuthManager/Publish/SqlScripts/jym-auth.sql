/*
Target Server Type    : SQL Server
Target Server Version : 110000
File Encoding         : 65001

Date: 2015-12-18 10:23:38
*/


-- ----------------------------
-- Table structure for Users
-- ----------------------------
DROP TABLE [dbo].[Users]
GO
CREATE TABLE [dbo].[Users] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserIdentifier] varchar(32) NOT NULL ,
[Cellphone] varchar(20) NOT NULL ,
[LoginNames] nvarchar(255) NOT NULL ,
[InviteBy] varchar(50) NOT NULL ,
[InviteFor] varchar(50) NOT NULL ,
[Salt] varchar(50) NOT NULL ,
[RegisterTime] datetime2(7) NOT NULL ,
[EncryptedPassword] varchar(255) NOT NULL ,
[ClientType] bigint NOT NULL ,
[ContractId] bigint NOT NULL ,
[OutletCode] varchar(50) NOT NULL ,
[Closed] bit NOT NULL ,
[LastModified] datetime2(7) NOT NULL ,
[Info] nvarchar(MAX) NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Users]', RESEED, 6)
GO

-- ----------------------------
-- Indexes structure for table Users
-- ----------------------------
CREATE UNIQUE INDEX [IN_UserIdentifier] ON [dbo].[Users]
([UserIdentifier] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_Cellphone] ON [dbo].[Users]
([Cellphone] ASC) 
GO
CREATE INDEX [IN_ContractId] ON [dbo].[Users]
([ContractId] ASC) 
GO
CREATE INDEX [IN_ClientType] ON [dbo].[Users]
([ClientType] ASC) 
GO
