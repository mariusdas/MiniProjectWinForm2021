create proc UpdateUser
@Id int,
@Name nvarchar(100),
@LastName nvarchar(100),
@Phone nvarchar(100),
@Date dateTime
as
begin
Update Users set Name=@Name,LastName=@LastName,Phone=@Phone,Date=@Date where ID=@Id
end
