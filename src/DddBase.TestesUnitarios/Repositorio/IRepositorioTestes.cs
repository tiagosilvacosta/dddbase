using DddBase.Base;
using DddBase.Repositorio;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DddBase.TestesUnitarios.Repositorio;

/// <summary>
/// Implementação concreta de IdEntidadeBaseInt para testes de repositório.
/// </summary>
public record IdProdutoTeste : IdEntidadeBaseInt
{
    public IdProdutoTeste(int valor) : base(valor)
    {
    }

    public static IdProdutoTeste Criar(int valor) => new(valor);
    
    public static implicit operator IdProdutoTeste(int valor) => new(valor);
    public static implicit operator int(IdProdutoTeste id) => id.ValorInteiro;
}

/// <summary>
/// Entidade de exemplo para testes do repositório.
/// </summary>
public class ProdutoTeste : EntidadeBase<IdProdutoTeste>, IRaizAgregado
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }

    public ProdutoTeste(IdProdutoTeste id, string nome, decimal preco) : base(id)
    {
        Nome = nome;
        Preco = preco;
    }

    // Construtor para ORMs
    protected ProdutoTeste() : base()
    {
        Nome = string.Empty;
    }
}

/// <summary>
/// Testes unitários para a interface IRepositorio.
/// </summary>
[TestFixture]
public class IRepositorioTestes
{
    private IRepositorio<ProdutoTeste, IdProdutoTeste> _repositorio;
    private ProdutoTeste _produtoExemplo;
    private IdProdutoTeste _idExemplo;

    /// <summary>
    /// Configuração inicial para cada teste.
    /// </summary>
    [SetUp]
    public void ConfigurarTeste()
    {
        _repositorio = Substitute.For<IRepositorio<ProdutoTeste, IdProdutoTeste>>();
        _idExemplo = new IdProdutoTeste(1);
        _produtoExemplo = new ProdutoTeste(_idExemplo, "Produto Teste", 99.99m);
    }

    /// <summary>
    /// Testa se ObterPorIdAsync retorna a entidade correta.
    /// </summary>
    [Test]
    public async Task ObterPorIdAsync_ComIdValido_DeveRetornarEntidade()
    {
        // Arrange
        _repositorio.ObterPorIdAsync(_idExemplo, Arg.Any<CancellationToken>())
                   .Returns(_produtoExemplo);

        // Act
        var resultado = await _repositorio.ObterPorIdAsync(_idExemplo);

        // Assert
        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado!.Id, Is.EqualTo(_idExemplo));
        Assert.That(resultado.Nome, Is.EqualTo("Produto Teste"));
    }

    /// <summary>
    /// Testa se ObterPorIdAsync retorna null para entidade inexistente.
    /// </summary>
    [Test]
    public async Task ObterPorIdAsync_ComIdInexistente_DeveRetornarNull()
    {
        // Arrange
        var idInexistente = new IdProdutoTeste(999);
        _repositorio.ObterPorIdAsync(idInexistente, Arg.Any<CancellationToken>())
                   .Returns((ProdutoTeste?)null);

        // Act
        var resultado = await _repositorio.ObterPorIdAsync(idInexistente);

        // Assert
        Assert.That(resultado, Is.Null);
    }

    /// <summary>
    /// Testa se ObterTodosAsync retorna todas as entidades.
    /// </summary>
    [Test]
    public async Task ObterTodosAsync_DeveRetornarTodasEntidades()
    {
        // Arrange
        var produtos = new List<ProdutoTeste>
        {
            _produtoExemplo,
            new ProdutoTeste(new IdProdutoTeste(2), "Produto 2", 149.99m)
        };
        _repositorio.ObterTodosAsync(Arg.Any<CancellationToken>())
                   .Returns(produtos);

        // Act
        var resultado = await _repositorio.ObterTodosAsync();

        // Assert
        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Count(), Is.EqualTo(2));
    }

    /// <summary>
    /// Testa se ObterPorCondicaoAsync retorna entidades que satisfazem a condição.
    /// </summary>
    [Test]
    public async Task ObterPorCondicaoAsync_ComCondicao_DeveRetornarEntidadesFiltradas()
    {
        // Arrange
        Expression<Func<ProdutoTeste, bool>> predicado = p => p.Preco > 100;
        var produtosFiltrados = new List<ProdutoTeste>
        {
            new ProdutoTeste(new IdProdutoTeste(2), "Produto Caro", 149.99m)
        };
        
        _repositorio.ObterPorCondicaoAsync(Arg.Any<Expression<Func<ProdutoTeste, bool>>>(), 
                                          Arg.Any<CancellationToken>())
                   .Returns(produtosFiltrados);

        // Act
        var resultado = await _repositorio.ObterPorCondicaoAsync(predicado);

        // Assert
        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Count(), Is.EqualTo(1));
    }

    /// <summary>
    /// Testa se AdicionarAsync adiciona a entidade corretamente.
    /// </summary>
    [Test]
    public async Task AdicionarAsync_ComEntidadeValida_DeveAdicionarERetornar()
    {
        // Arrange
        _repositorio.AdicionarAsync(_produtoExemplo, Arg.Any<CancellationToken>())
                   .Returns(_produtoExemplo);

        // Act
        var resultado = await _repositorio.AdicionarAsync(_produtoExemplo);

        // Assert
        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Id, Is.EqualTo(_produtoExemplo.Id));
    }

    /// <summary>
    /// Testa se AtualizarAsync atualiza a entidade corretamente.
    /// </summary>
    [Test]
    public async Task AtualizarAsync_ComEntidadeValida_DeveAtualizarERetornar()
    {
        // Arrange
        _produtoExemplo.Nome = "Produto Atualizado";
        _repositorio.AtualizarAsync(_produtoExemplo, Arg.Any<CancellationToken>())
                   .Returns(_produtoExemplo);

        // Act
        var resultado = await _repositorio.AtualizarAsync(_produtoExemplo);

        // Assert
        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Nome, Is.EqualTo("Produto Atualizado"));
    }

    /// <summary>
    /// Testa se RemoverAsync por ID remove a entidade corretamente.
    /// </summary>
    [Test]
    public async Task RemoverAsync_PorId_DeveRemoverERetornarTrue()
    {
        // Arrange
        _repositorio.RemoverAsync(_idExemplo, Arg.Any<CancellationToken>())
                   .Returns(true);

        // Act
        var resultado = await _repositorio.RemoverAsync(_idExemplo);

        // Assert
        Assert.That(resultado, Is.True);
    }

    /// <summary>
    /// Testa se RemoverAsync por entidade remove corretamente.
    /// </summary>
    [Test]
    public async Task RemoverAsync_PorEntidade_DeveRemoverERetornarTrue()
    {
        // Arrange
        _repositorio.RemoverAsync(_produtoExemplo, Arg.Any<CancellationToken>())
                   .Returns(true);

        // Act
        var resultado = await _repositorio.RemoverAsync(_produtoExemplo);

        // Assert
        Assert.That(resultado, Is.True);
    }

    /// <summary>
    /// Testa se ExisteAsync retorna true para entidade existente.
    /// </summary>
    [Test]
    public async Task ExisteAsync_ComIdExistente_DeveRetornarTrue()
    {
        // Arrange
        _repositorio.ExisteAsync(_idExemplo, Arg.Any<CancellationToken>())
                   .Returns(true);

        // Act
        var resultado = await _repositorio.ExisteAsync(_idExemplo);

        // Assert
        Assert.That(resultado, Is.True);
    }

    /// <summary>
    /// Testa se ExisteAsync retorna false para entidade inexistente.
    /// </summary>
    [Test]
    public async Task ExisteAsync_ComIdInexistente_DeveRetornarFalse()
    {
        // Arrange
        var idInexistente = new IdProdutoTeste(999);
        _repositorio.ExisteAsync(idInexistente, Arg.Any<CancellationToken>())
                   .Returns(false);

        // Act
        var resultado = await _repositorio.ExisteAsync(idInexistente);

        // Assert
        Assert.That(resultado, Is.False);
    }

    /// <summary>
    /// Testa se ContarAsync retorna o número correto de entidades.
    /// </summary>
    [Test]
    public async Task ContarAsync_DeveRetornarNumeroCorretoDeEntidades()
    {
        // Arrange
        _repositorio.ContarAsync(Arg.Any<CancellationToken>())
                   .Returns(5);

        // Act
        var resultado = await _repositorio.ContarAsync();

        // Assert
        Assert.That(resultado, Is.EqualTo(5));
    }

    /// <summary>
    /// Testa se ContarAsync com predicado retorna o número correto.
    /// </summary>
    [Test]
    public async Task ContarAsync_ComPredicado_DeveRetornarNumeroCorreto()
    {
        // Arrange
        Expression<Func<ProdutoTeste, bool>> predicado = p => p.Preco > 100;
        _repositorio.ContarAsync(Arg.Any<Expression<Func<ProdutoTeste, bool>>>(), 
                                Arg.Any<CancellationToken>())
                   .Returns(2);

        // Act
        var resultado = await _repositorio.ContarAsync(predicado);

        // Assert
        Assert.That(resultado, Is.EqualTo(2));
    }
}
