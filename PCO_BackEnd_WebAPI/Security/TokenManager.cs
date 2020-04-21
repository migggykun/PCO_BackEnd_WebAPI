using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace PCO_BackEnd_WebAPI.Security
{
    public class TokenManager
    {
        private static string Secret = @"904706c5/334c/4fd8/bbeb/0164b9dcec7e";

        public static string GenerateToken(string userName, bool accountType)
        {
            //string Secret = "sUXa82XSgk3q6n/Yr1630f/Xfrq44lr/kpzcYLMEPBRnCwktx8LrtaIjyCKeJEhp5QA=";
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName), new Claim(ClaimTypes.Role, accountType.ToString())}),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static string ValidateToken(string token)
        {
            string username = string.Empty;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
            {
                return null;
            }
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch(NullReferenceException)
            {
                return null;
            }

            Claim userClaim = identity.FindFirst(ClaimTypes.Name);
            username = userClaim.Value;
            return username;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var expiry = jwtToken.ValidTo;
                expiry.ToString();
                if (jwtToken == null)
                {
                    return null;
                }
                byte[] key = Convert.FromBase64String(Secret);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = expiry.TimeOfDay
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }

            catch
            {
                return null;
            }
        }
    }
}