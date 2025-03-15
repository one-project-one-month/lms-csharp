namespace LearningManagementSystem.Domain.Services.AuthServices;

public class AuthService
{
    private readonly AppDbContext _contxt;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext contxt, IConfiguration configuration)
    {
        _contxt = contxt;
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(TblUsers user, TblRoles role)
    {
        var existingToken = await _contxt.Tokens
            .FirstOrDefaultAsync(t => t.user_id == user.id);
        var issue = _configuration["Jwt:Issuer"];
        var audienc = _configuration["Jwt:Audience"];
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        var cred = SecurityAlgorithms.HmacSha256;


        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, cred);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, role.role),
            new Claim("username", user.username)
        };

        // set expiration time based on existing token
        var expirationTime = existingToken != null
            ? DateTime.UtcNow.AddHours(1) // reduce time for existing user
            : DateTime.UtcNow.AddMonths(1); // full time for new token

        var token = new JwtSecurityToken
            (
                issuer: issue,
                audience: audienc,
                claims: claims,
                //expires: DateTime.UtcNow.AddMonths(1),
                expires: expirationTime,
                signingCredentials: credentials
            );

        var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
      
            // if (existingToken.updated_at < DateTime.UtcNow)
            // {
            //     existingToken.token = generatedToken;
            //     existingToken.updated_at = DateTime.UtcNow.AddMonths(1);

            //     await _contxt.SaveChangesAsync();
            // }

            // // only save to database if no valid token exist
            // if (existingToken == null)
            // {
            //     var tokenEntity = new TblTokens
            //     {
            //         user_id = user.id,
            //         token = generatedToken,
            //         created_at = DateTime.UtcNow,
            //         updated_at = DateTime.UtcNow.AddMonths(1)
            //     };

            //     await _contxt.Tokens.AddAsync(tokenEntity);

            //     await _contxt.SaveChangesAsync();
            // }

            if (existingToken != null)
            {
                if (existingToken.updated_at < DateTime.UtcNow)
                {
                    existingToken.token = generatedToken;
                    existingToken.updated_at = DateTime.UtcNow.AddMonths(1);

                    await _contxt.SaveChangesAsync();
                }
            }
            else
            {
                user_id = user.id,
                token = generatedToken,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow.AddMonths(1)
            };

            await _contxt.Tokens.AddAsync(tokenEntity);

            await _contxt.SaveChangesAsync();
        }
        return generatedToken;
    }
}
