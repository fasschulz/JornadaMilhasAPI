using JornadaMilhas.API.DTO.Auth;
using System.Net;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.API
{
    public class JornadaMilhasAuthTest
    {
        [Fact]
        public async Task POST_Efetua_Login_Com_Sucesso()
        {
            var app = new JornadaMilhasWebApplicationFactory();
            var user = new UserDTO { Email = "tester@email.com", Password = "Senha123@" };

            using var client = app.CreateClient();

            var resultado = await client.PostAsJsonAsync("/auth-login", user);

            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }

        [Fact]
        public async Task POST_Retorna_Login_Invalido()
        {
            var app = new JornadaMilhasWebApplicationFactory();
            var user = new UserDTO { Email = "tester@email.com", Password = "ssd@" };

            using var client = app.CreateClient();

            var resultado = await client.PostAsJsonAsync("/auth-login", user);
            
            Assert.Equal(HttpStatusCode.BadRequest, resultado.StatusCode);
            Assert.Equal(await resultado.Content.ReadAsStringAsync(), "\"Login inválido.\"");
        }
    }
}