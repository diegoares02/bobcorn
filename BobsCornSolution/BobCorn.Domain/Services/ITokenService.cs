namespace BobsCorn.Domain.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userId);
    }
}
