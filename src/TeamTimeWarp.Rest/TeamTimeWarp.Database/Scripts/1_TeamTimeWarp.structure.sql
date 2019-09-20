
--structure
use TeamTimeWarp
GO

CREATE TABLE [dbo].[Account]
(
Id int NOT NULL PRIMARY KEY,
Name varchar(255),
Email varchar(255),
IsFake bit Default(0),
--Password varchar(255)
)

CREATE TABLE [dbo].[AccountPassword]
(
Id int NOT NULL PRIMARY KEY,
Account_Id int,
Password varchar(255)

FOREIGN KEY (Account_Id) REFERENCES Account(Id)
                      ON DELETE CASCADE
)

CREATE TABLE Room
(
Id int NOT NULL PRIMARY KEY,
Name varchar(255),
CreationTime DateTime,
IsFake bit Default(0),
) 

CREATE TABLE RoomAccounts
(
Room_Id int,
Account_Id int,
FOREIGN KEY (Room_Id) REFERENCES Room(Id)
                      ON DELETE CASCADE,
FOREIGN KEY (Account_Id) REFERENCES Account(Id)
                      ON DELETE CASCADE
)

CREATE TABLE UserState
(
Id int NOT NULL PRIMARY KEY,
Account_Id int,
TimeWarpState varchar(20),
PeriodStartTime DateTime,
TimeLeft bigint,
FOREIGN KEY (Account_Id) REFERENCES Account(Id)
                 ON DELETE CASCADE
)

CREATE TABLE TimeWarpState
(
	Id int,
	Name varchar(20)
)

CREATE TABLE NH_HiLo
(
	NextHi int default(0),
	TableKey varchar(50)
)


--data

insert into [dbo].[TimeWarpState] values (1,'Resting')
insert into [dbo].[TimeWarpState] values (1,'Working')
insert into [dbo].[TimeWarpState] values (1,'None')

insert into [dbo].[NH_HiLo] values (0,'UserState')
insert into [dbo].[NH_HiLo] values (0,'Account')
insert into [dbo].[NH_HiLo] values (0,'AccountPassword')
insert into [dbo].[NH_HiLo] values (0,'Room')