using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using System.Threading.Tasks;

namespace Repository.JwtConfig
{
    public  class JWTSettings
    {
        public string SecKey { get; set; }
    }
    public class JwtTool
    {
        public static string BuildToken(List<Claim> claims, string key, DateTime expire) //配置jwt根据key 和有效载荷加缪
        {
            byte[] secBytes = Encoding.UTF8.GetBytes(key); 
            var secKey = new SymmetricSecurityKey(secBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature); //创建签名凭证（指定算法）,就是告诉jwt 如何生成令牌
            var token = new JwtSecurityToken(claims: claims, expires: expire, signingCredentials: credentials); // 生成 JWT 令牌
            string jwt = new JwtSecurityTokenHandler().WriteToken(token); //jwt令牌字符化
            return jwt;
        }
    }
}
