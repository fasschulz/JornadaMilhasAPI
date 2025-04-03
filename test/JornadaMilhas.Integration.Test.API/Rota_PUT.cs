
using System.Net;
using JornadaMilhas.Dominio.Entidades;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.API;
public class Rota_PUT : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public Rota_PUT(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Atualizar_Rota_PorId()
    {
        var rotaExistente = new Rota()
        {
            Origem = "Rio de Janeiro",
            Destino = "São Paulo"
        };
        app.Context.Add(rotaExistente);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        var response = await client.PutAsJsonAsync($"/rota-viagem/", rotaExistente);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}