USE [TeamTimeWarp]
GO

CREATE TABLE [dbo].[UserMessage]
(
Id int IDENTITY(1,1) Not Null,
ToAccount_Id int Not Null,
FromAccount_Id int Not Null,
SendTime datetime Not Null,
TextMessage varchar(500) Not Null,

FOREIGN KEY (ToAccount_Id) REFERENCES Account(Id),
FOREIGN KEY (FromAccount_Id) REFERENCES Account(Id)
)
GO
insert into [dbo].[NH_HiLo] values (0,'UserMessage')