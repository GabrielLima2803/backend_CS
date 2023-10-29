// // Services/AuthService.cs

// using System;
// using System.Threading.Tasks;

// public class AuthService : IAuthService
// {
//     private readonly ApplicationDbContext _context; // Injete o contexto do banco de dados aqui

//     public AuthService(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<string> AuthenticateUserAsync(string username, string password)
//     {
//         User user = await _context.Users.FirstOrDefaultAsync(u => u.Usuario == username);

//         if (user != null && VerifyPassword(password, user.Senha))
//         {
//             return GenerateJwtToken(user);
//         }

//         return null;
//     }

//     private bool VerifyPassword(string inputPassword, string userPassword)
//     {
//         return inputPassword == userPassword;
//     }

// }
