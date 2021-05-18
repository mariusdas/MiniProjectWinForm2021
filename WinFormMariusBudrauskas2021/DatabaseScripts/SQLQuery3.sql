create proc AddUser
@Name nvarchar(100),
@LastName nvarchar(100),
@Phone nvarchar(100),
@Date dateTime
as
begin
insert Users (Name,LastName,Phone,Date) values
(@Name,@LastName,@Phone,@Date)
end
