using System.Net;
using BobsCorn.Application.DTOs;

namespace BobsCorn.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<(HttpStatusCode, string)> LoginAsync(UserLoginDto user);
        Task<(HttpStatusCode,string)> AddUserAsync(UserRegisterDto user);
    }
}
