
using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System.Net.Http.Json;

namespace JornadaMilhas.Integration.Test.API;

public class Rota_GET : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public Rota_GET(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Recupera_Rota_PorId()
    {
        var rotaExistente = new Rota()
        {
            Origem = "Rio de Janeiro",
            Destino = "São Paulo"
        };
        app.Context.Add(rotaExistente);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        var response = await client.GetFromJsonAsync<Rota>("/rota-viagem/" + rotaExistente.Id);

        Assert.NotNull(response);
        Assert.Equal(rotaExistente.Origem, response.Origem);
        Assert.Equal(rotaExistente.Destino, response.Destino);
    }
}