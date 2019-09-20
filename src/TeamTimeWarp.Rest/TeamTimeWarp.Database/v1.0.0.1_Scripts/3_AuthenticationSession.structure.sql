USE [TeamTimeWarp]
GO

CREATE TABLE [dbo].[AuthenticationSession]
(
Id int NOT NULL PRIMARY KEY,
Account_Id int,
Token varchar(255),
LastValidation DateTime,

FOREIGN KEY (Account_Id) REFERENCES Account(Id)
                      ON DELETE CASCADE
)
GO
insert into [dbo].[NH_HiLo] values (0,'AuthenticationSession')