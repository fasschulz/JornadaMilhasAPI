using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API;

public class OfertaViagem_POST : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public OfertaViagem_POST(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Cadastra_Oferta_Viagem()
    {      
        using var client = await app.GetClientWithAccessTokenAsync();

        var ofertaViagem = new OfertaViagem()
        {
            Preco = 100,
            Rota = new Rota("Origem", "Destino"),
            Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        };
        
        var response = await client.PostAsJsonAsync("/ofertas-viagem", ofertaViagem);
                
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Cadastra_Oferta_Viagem_SemAutorizacao()
    {        
        using var client = app.CreateClient();

        var ofertaViagem = new OfertaViagem()
        {
            Preco = 100,
            Rota = new Rota("Origem", "Destino"),
            Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        };

        var response = await client.PostAsJsonAsync("/ofertas-viagem", ofertaViagem);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
