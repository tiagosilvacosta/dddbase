using DddBase.Base;
using NUnit.Framework;
using System.Collections.Generic;

namespace DddBase.TestesUnitarios.Base;

/// <summary>
/// Implementação concreta de ObjetoDeValor para testes.
/// </summary>
public record EnderecoTeste : ObjetoDeValor
{
    public string Rua { get; }
    public string Cidade { get; }
    public string CEP { get; }

    public EnderecoTeste(string rua, string cidade, string cep)
    {
        Rua = rua;
        Cidade = cidade;
        CEP = cep;
    }

    protected override IEnumerable<object?> ObterComponentesDeIgualdade()
    {
        yield return Rua;
        yield return Cidade;
        yield return CEP;
    }
}

/// <summary>
/// Testes unitários para a classe ObjetoDeValor.
/// </summary>
[TestFixture]
public class ObjetoDeValorTestes
{
    /// <summary>
    /// Testa se dois objetos de valor com os mesmos valores são considerados iguais.
    /// </summary>
    [Test]
    public void ObjetosDeValor_ComMesmosValores_DevemSerIguais()
    {
        // Arrange
        var endereco1 = new EnderecoTeste("Rua A", "São Paulo", "01234-567");
        var endereco2 = new EnderecoTeste("Rua A", "São Paulo", "01234-567");

        // Act & Assert
        Assert.That(endereco1, Is.EqualTo(endereco2));
        Assert.That(endereco1.GetHashCode(), Is.EqualTo(endereco2.GetHashCode()));
    }

    /// <summary>
    /// Testa se dois objetos de valor com valores diferentes são considerados diferentes.
    /// </summary>
    [Test]
    public void ObjetosDeValor_ComValoresDiferentes_DevemSerDiferentes()
    {
        // Arrange
        var endereco1 = new EnderecoTeste("Rua A", "São Paulo", "01234-567");
        var endereco2 = new EnderecoTeste("Rua B", "São Paulo", "01234-567");

        // Act & Assert
        Assert.That(endereco1, Is.Not.EqualTo(endereco2));
    }

    /// <summary>
    /// Testa se objetos de valor nulos são tratados corretamente.
    /// </summary>
    [Test]
    public void ObjetoDeValor_ComparandoComNull_DeveRetornarFalse()
    {
        // Arrange
        var endereco = new EnderecoTeste("Rua A", "São Paulo", "01234-567");

        // Act & Assert
        Assert.That(endereco.Equals(null), Is.False);
    }

    /// <summary>
    /// Testa se objetos de valor de tipos diferentes são considerados diferentes.
    /// </summary>
    [Test]
    public void ObjetosDeValor_DeTiposDiferentes_DevemSerDiferentes()
    {
        // Arrange
        var endereco = new EnderecoTeste("Rua A", "São Paulo", "01234-567");
        var objeto = new object();

        // Act & Assert
        Assert.That(endereco.Equals(objeto), Is.False);
    }

    /// <summary>
    /// Testa se o hash code é consistente para objetos iguais.
    /// </summary>
    [Test]
    public void ObjetosDeValor_IguaisDevemTerMesmoHashCode()
    {
        // Arrange
        var endereco1 = new EnderecoTeste("Rua A", "São Paulo", "01234-567");
        var endereco2 = new EnderecoTeste("Rua A", "São Paulo", "01234-567");

        // Act & Assert
        Assert.That(endereco1.GetHashCode(), Is.EqualTo(endereco2.GetHashCode()));
    }
}
