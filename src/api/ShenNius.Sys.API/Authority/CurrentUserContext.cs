using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ShenNius.Sys.API.Authority
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUserContext
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => GetName();

        private string GetName()
        {
            if (IsAuthenticated())
            {
                return _accessor.HttpContext.User.Identity.Name;
            }
            else
            {
                if (!string.IsNullOrEmpty(GetToken()))
                {
                    return GetUserInfoFromToken("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").FirstOrDefault().ToString();
                }
            }

            return "";
        }

        public string Mobile => GetClaimValueByType("mobile").FirstOrDefault()?.ToString();

        public int Id => Convert.ToInt32(GetClaimValueByType("jti").FirstOrDefault());

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }


        public string GetToken()
        {
            return _accessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        public List<string> GetUserInfoFromToken(string claimType)
        {

            var jwtHandler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(GetToken()))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());

                return (from item in jwtToken.Claims
                        where item.Type == claimType
                        select item.Value).ToList();
            }
            else
            {
                return new List<string>() { };
            }
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public List<string> GetClaimValueByType(string claimType)
        {

            return (from item in GetClaimsIdentity()
                    where item.Type == claimType
                    select item.Value).ToList();

        }

        public bool IsAdmin()
        {
            return GetClaimsIdentity().FirstOrDefault(x => x.Type.Equals("IsAdmin"))?.Value ==
                   "1";
        }

        public bool InRole(string roleType)
        {
            return GetClaimsIdentity().FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value ==
                   roleType;
        }
    }
}
