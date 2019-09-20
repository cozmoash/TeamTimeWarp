USE [TeamTimeWarp]
GO

ALTER TABLE Account
ADD AccountType VARCHAR(50);
GO

UPDATE Account
SET AccountType = 'Fake'
WHERE IsFake = 1

UPDATE Account
SET AccountType = 'Full'
WHERE IsFake = 0

UPDATE Account
SET AccountType = 'Fake'
WHERE AccountType is NULL

declare @default sysname, @sql nvarchar(max)

select @default = name 
from sys.default_constraints 
where parent_object_id = object_id('Account')
AND type = 'D'
AND parent_column_id = (
    select column_id 
    from sys.columns 
    where object_id = object_id('Account')
    and name = 'IsFake'
    )

set @sql = N'alter table Account drop constraint ' + @default
exec sp_executesql @sql

alter table Account drop column IsFake

go


