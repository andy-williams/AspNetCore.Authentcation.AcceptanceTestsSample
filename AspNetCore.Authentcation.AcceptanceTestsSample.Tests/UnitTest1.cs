using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace AspNetCore.Authentcation.AcceptanceTestsSample.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Unauthorized()
        {
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var result = await client.GetAsync("/api/values");

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task Ok()
        {
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, GetToken());

            var result = await client.GetAsync("/api/values");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        private string GetToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "andy@thinkprogramming.co.uk")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myyyysecret_keyyyyy"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("web", "user", claims, expires: DateTime.Now.AddDays(7), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
