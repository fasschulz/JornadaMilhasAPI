using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using JornadaMilhas.Integration.Test.API.DataBuilders;
using Microsoft.EntityFrameworkCore;

namespace JornadaMilhas.Integration.Test.API;

public class OfertaViagem_GET : IClassFixture<JornadaMilhasWebApplicationFactory>
{
    private readonly JornadaMilhasWebApplicationFactory app;

    public OfertaViagem_GET(JornadaMilhasWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Recupera_OfertaViagem_PorId()
    {
        var ofertaExistente = app.Context.OfertasViagem.FirstOrDefault();
        if (ofertaExistente is null)
        {
            ofertaExistente = new OfertaViagem()
            {
                Preco = 100,
                Rota = new Rota("Origem", "Destino"),
                Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
            };
            app.Context.Add(ofertaExistente);
            app.Context.SaveChanges();
        }

        using var client = await app.GetClientWithAccessTokenAsync();

        var response = await client.GetFromJsonAsync<OfertaViagem>("/ofertas-viagem/" + ofertaExistente.Id);

        Assert.NotNull(response);
        Assert.Equal(ofertaExistente.Preco, response.Preco, 0.001);
        Assert.Equal(ofertaExistente.Rota.Origem, response.Rota.Origem);
        Assert.Equal(ofertaExistente.Rota.Destino, response.Rota.Destino);
    }

    [Fact]
    public async Task Recupera_OfertasViagem_Na_Consulta_Paginada()
    {
        app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

        var dataBuilder = new OfertaViagemDataBuilder();
        var listaDeOfertas = dataBuilder.Generate(80);
        app.Context.AddRange(listaDeOfertas);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        int pagina = 1;
        int tamanhoPorPagina = 80;
        
        var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");
        
        Assert.True(response != null);
        Assert.Equal(tamanhoPorPagina, response.Count());
    }

    [Fact]
    public async Task Recupera_OfertasViagem_Na_Consulta_Ultima_Pagina()
    {
        app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

        var dataBuilder = new OfertaViagemDataBuilder();
        var listaDeOfertas = dataBuilder.Generate(80);
        app.Context.AddRange(listaDeOfertas);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        int pagina = 4;
        int tamanhoPorPagina = 25;

        var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        Assert.True(response != null);
        Assert.Equal(5, response.Count());
    }

    [Fact]
    public async Task Recupera_OfertasViagem_Na_Consulta_Com_Pagina_Inexistente()
    {
        app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

        var dataBuilder = new OfertaViagemDataBuilder();
        var listaDeOfertas = dataBuilder.Generate(80);
        app.Context.AddRange(listaDeOfertas);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        int pagina = 5;
        int tamanhoPorPagina = 25;

        var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        Assert.True(response != null);
        Assert.Equal(0, response.Count());
    }

    [Fact]
    public async Task Recupera_OfertasViagem_Na_Consulta_Com_Pagina_Com_Valor_Negativo()
    {
        app.Context.Database.ExecuteSqlRaw("Delete from OfertasViagem");

        var dataBuilder = new OfertaViagemDataBuilder();
        var listaDeOfertas = dataBuilder.Generate(80);
        app.Context.AddRange(listaDeOfertas);
        app.Context.SaveChanges();

        using var client = await app.GetClientWithAccessTokenAsync();

        int pagina = -5;
        int tamanhoPorPagina = 25;

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            var response = await client.GetFromJsonAsync<ICollection<OfertaViagem>>
                ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");
        });
        
    }
}
