using Azure.Core;
using StudentAdminPortalAPI.DomainModels;

namespace StudentAdminPortalAPI.Services
{
    public interface IUserService
    {
        void CreatePasswordHash(string Password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string Password, byte[] passwordHash, byte[] passwordSalt);
        string CreateRandomToken();


    }
}
