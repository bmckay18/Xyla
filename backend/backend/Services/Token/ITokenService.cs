namespace Backend.Services.Token
{
    public interface ITokenService
    {
        string GenerateToken(Guid playerId);
    }
}