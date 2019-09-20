USE [TeamTimeWarp]
GO

ALTER TABLE UserMessage
  ADD HasBeenReceived bit NOT NULL Default(0)
