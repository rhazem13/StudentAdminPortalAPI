using StudentAdminPortalAPI.DataModels;
using StudentAdminPortalAPI.DomainModels;

namespace StudentAdminPortalAPI.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string requestEmail);

        Task<User> AddUser(User user);
        Task<User> GetUser(string Email);

        Task<bool> VerifyUserByToken(string token);

        Task<bool> CreatePasswordToken(User user);

        Task<bool> ResetPassword(ResetPasswordRequest request);

    }
}
