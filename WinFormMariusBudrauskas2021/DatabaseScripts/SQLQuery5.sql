create proc DeleteUser
@Id int
as
begin
Delete Users where ID=@Id
end