// Services/IAuthService.cs

using System.Threading.Tasks;

public interface IAuthService
{
    Task<string> AuthenticateUserAsync(string username, string password);
}
