USE [TeamTimeWarp]
GO

ALTER TABLE UserState 
ADD AgentType int NOT NULL
DEFAULT 1
GO