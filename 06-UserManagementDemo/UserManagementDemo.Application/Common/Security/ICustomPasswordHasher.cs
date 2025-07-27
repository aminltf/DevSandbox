namespace UserManagementDemo.Application.Common.Security;

public interface ICustomPasswordHasher<TUser>
{
    string HashPassword(TUser user, string password);
    bool VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword);
}
