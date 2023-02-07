using Microsoft.EntityFrameworkCore;
using StudentAdminPortalAPI.DataModels;
using StudentAdminPortalAPI.DomainModels;
using StudentAdminPortalAPI.Services;
using System.Security.Claims;

namespace StudentAdminPortalAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StudentAdminContext context;
        private readonly IUserService userService;

        public UserRepository(StudentAdminContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<User> AddUser(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(string Email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }

        public async Task<bool> VerifyUserByToken(string token)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user == null)
            {
                return false;
            }
            user.VerifiedAt= DateTime.Now;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserExists(string requestEmail)
        {
            return await context.Users.AnyAsync(u => u.Email == requestEmail);
        }

        public async Task<bool> CreatePasswordToken(User user)
        {
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return false;
            }
            userService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            await context.SaveChangesAsync();
            return true;

        }
    }
}
