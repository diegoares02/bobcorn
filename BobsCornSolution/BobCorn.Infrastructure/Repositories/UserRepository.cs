using System.Net;
using BobsCorn.Application.Validations;
using BobsCorn.Infrastructure.Data;
using BobsCorn.Infrastructure.Utilities;
using BobsCorn.Application.DTOs;
using BobsCorn.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using BobsCorn.Domain.Entities;

namespace BobsCorn.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BobCornDbContext _context;
        private readonly UserRegisterValidator _userRegistervalidator = new UserRegisterValidator();
        private readonly UserLoginValidator _userLoginValidator = new UserLoginValidator();
        public UserRepository(BobCornDbContext context)
        {
            _context = context;
        }
        public async Task<(HttpStatusCode, string)> AddUserAsync(UserRegisterDto user)
        {
            var userExists = _context.Users.Any(u => u.Email == user.Email);

            if (userExists)
            {
                return (HttpStatusCode.BadRequest, "User already exists.");
            }

            var validationResult = _userRegistervalidator.Validate(user);

            if (!validationResult.IsValid)
            {
                return (HttpStatusCode.BadRequest, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var newUser = new Domain.Entities.User
            {
                Email = user.Email,
                Password = EncryptionUtils.Encrypt(user.Password),
                Name = user.Name,
                Lastname = user.Lastname
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return (HttpStatusCode.OK, "User registered successfully.");
        }

        public int? GetUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email)?.UserId;
        }

        public async Task<(HttpStatusCode, string)> LoginAsync(UserLoginDto user)
        {
            var validationResult = _userLoginValidator.Validate(user);

            if (!validationResult.IsValid)
            {
                return (HttpStatusCode.BadRequest, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var userFound = await _context.Users
                .Where(u => u.Email == user.Email && u.Password == EncryptionUtils.Encrypt(user.Password))
                .FirstOrDefaultAsync();
            if (userFound == null)
            {
                return (HttpStatusCode.NotFound, "User not found");
            }

            return (HttpStatusCode.OK, JsonSerializer.Serialize(userFound));
        }
    }
}
