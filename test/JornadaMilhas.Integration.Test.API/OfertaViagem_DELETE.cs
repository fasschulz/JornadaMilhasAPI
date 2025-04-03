using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integration.Test.API;

public class OfertaViagem_DELETE : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public OfertaViagem_DELETE(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Deletar_OfertaViagem_PorId()
    {
        var ofertaExistente = new OfertaViagem()
        {
            Preco = 100,
            Rota = new Rota("Origem", "Destino"),
            Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        };
        app.Context.Add(ofertaExistente);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();
                
        HttpRequestMessage request = new HttpRequestMessage
        {            
            Method = HttpMethod.Delete,
            RequestUri = new Uri("/ofertas-viagem/" + ofertaExistente.Id, UriKind.Relative)
        };
        var response = await client.SendAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
