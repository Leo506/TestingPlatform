namespace Client.Services.Interfaces;

public interface IRefreshTokenService
{
    Task<bool> RefreshTokenAsync();
}