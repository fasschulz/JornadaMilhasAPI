using JornadaMilhas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API;

public class Rota_POST : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public Rota_POST(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Cadastra_Rota()
    {
        using var client = await app.GetClientWithAccessTokenAsync();

        var rota = new Rota()
        {
            Origem = "Rio de Janeiro",
            Destino = "São Paulo"
        };

        var response = await client.PostAsJsonAsync<Rota>($"/rota-viagem/", rota);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Cadastra_Rota_SemAutorizacao()
    {
        using var client = app.CreateClient();

        var rota = new Rota()
        {
            Origem = "Rio de Janeiro",
            Destino = "São Paulo"
        };

        var response = await client.PostAsJsonAsync<Rota>($"/rota-viagem/", rota);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}