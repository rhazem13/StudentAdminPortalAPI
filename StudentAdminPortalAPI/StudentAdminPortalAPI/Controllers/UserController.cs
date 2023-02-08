using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortalAPI.DataModels;
using StudentAdminPortalAPI.DomainModels;
using StudentAdminPortalAPI.Repositories;
using StudentAdminPortalAPI.Services;

namespace StudentAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IUserService userService;

        public UserController(IUserRepository userRepository, IUserService userService)
        {
            this.userRepository = userRepository;
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (await userRepository.UserExists(request.Email))
            {
                return BadRequest("User already exists");
            }
            userService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = userService.CreateRandomToken()
            };
            var result = await userRepository.AddUser(user);
            return Ok("User created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await userRepository.GetUser(request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            if (!userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect.");
            }
            if (user.VerifiedAt == null)
            {
                return BadRequest("Not verified!");
            }
            string token = userService.CreateToken(request);
            return Ok(token);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            if(await userRepository.VerifyUserByToken(token))
                return Ok("User verified!");
            return BadRequest("Invalid token");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await userRepository.GetUser(email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            user.PasswordResetToken = userService.CreateRandomToken();
            var result = await userRepository.CreatePasswordToken(user);
            return Ok("you may now reset your password");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await userRepository.ResetPassword(request);
            if (!result)
            {
                return BadRequest("invalid token");
            }
            return Ok("Password succesfully reset!");
        }
    }
}
