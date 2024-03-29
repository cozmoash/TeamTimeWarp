
declare @accountId int 
SELECT @accountId  = Max(Id) from Account ;
SET @accountId = @accountId + 1;

declare @userStateId int 
SELECT @userStateId  = Max(Id) from UserState ;
SET @userStateId = @userStateId + 1;

declare @AccountPasswordId int 
SELECT @AccountPasswordId  = Max(Id) from UserState ;
SET @AccountPasswordId = @AccountPasswordId + 1;

INSERT [dbo].[Account] ([Id], [Name], [Email], [IsFake]) VALUES (@accountId, N'devUser', N'dev', 0)
INSERT [dbo].[UserState] ([Id], [Account_Id], [TimeWarpState], [PeriodStartTime], [TimeLeft]) VALUES (@userStateId, @accountId, N'None', CAST(0x00008EAC00000000 AS DateTime), 0)
INSERT [dbo].[AccountPassword] ([Id], [Account_Id], [Password]) VALUES (@AccountPasswordId, @accountId, N'dev')
