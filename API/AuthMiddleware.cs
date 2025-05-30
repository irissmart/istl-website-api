using System.IdentityModel.Tokens.Jwt;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IrisContext dbContext)
        {
            var token = context
                .Request.Headers["Authorization"]
                .FirstOrDefault()
                ?.Split(' ')
                .Last();

            if (token != null)
            {
                var userId = ValidateToken(token);

                if (userId.HasValue)
                {
                    var user = await dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                        x.Id == userId && x.IsActive);

                    if (user == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }

                    context.Items["UserId"] = userId;
                }
            }

            await _next(context);
        }

        private int? ValidateToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                return int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);
            }
            catch
            {
                return null;
            }
        }
    }
}
