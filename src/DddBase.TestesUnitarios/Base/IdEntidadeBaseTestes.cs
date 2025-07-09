using DddBase.Base;
using NUnit.Framework;
using System;

namespace DddBase.TestesUnitarios.Base;

/// <summary>
/// Implementação concreta de IdEntidadeBaseInt para testes.
/// </summary>
public record IdEntidadeBaseIntTeste : IdEntidadeBaseInt
{
    public IdEntidadeBaseIntTeste(int valor) : base(valor)
    {
    }

    public static IdEntidadeBaseIntTeste Criar(int valor) => new(valor);
    
    public static implicit operator IdEntidadeBaseIntTeste(int valor) => new(valor);
    public static implicit operator int(IdEntidadeBaseIntTeste id) => id.ValorInteiro;
}

/// <summary>
/// Testes unitários para as classes IdEntidadeBase e IdEntidadeBaseInt.
/// </summary>
[TestFixture]
public class IdEntidadeBaseTestes
{
    /// <summary>
    /// Testa se IdEntidadeBaseInt é criado corretamente com valor válido.
    /// </summary>
    [Test]
    public void IdEntidadeBaseInt_ComValorValido_DeveCriarCorretamente()
    {
        // Arrange
        var valor = 123;

        // Act
        var id = new IdEntidadeBaseIntTeste(valor);

        // Assert
        Assert.That(id.ValorInteiro, Is.EqualTo(valor));
        Assert.That(id.Valor, Is.EqualTo(valor));
    }

    /// <summary>
    /// Testa se IdEntidadeBaseInt lança exceção para valor inválido (zero ou negativo).
    /// </summary>
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public void IdEntidadeBaseInt_ComValorInvalido_DeveLancarExcecao(int valorInvalido)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new IdEntidadeBaseIntTeste(valorInvalido));
    }

    /// <summary>
    /// Testa se dois identificadores com o mesmo valor são considerados iguais.
    /// </summary>
    [Test]
    public void IdEntidadeBaseInt_ComMesmosValores_DevemSerIguais()
    {
        // Arrange
        var id1 = new IdEntidadeBaseIntTeste(123);
        var id2 = new IdEntidadeBaseIntTeste(123);

        // Act & Assert
        Assert.That(id1, Is.EqualTo(id2));
        Assert.That(id1.GetHashCode(), Is.EqualTo(id2.GetHashCode()));
    }

    /// <summary>
    /// Testa se dois identificadores com valores diferentes são considerados diferentes.
    /// </summary>
    [Test]
    public void IdEntidadeBaseInt_ComValoresDiferentes_DevemSerDiferentes()
    {
        // Arrange
        var id1 = new IdEntidadeBaseIntTeste(123);
        var id2 = new IdEntidadeBaseIntTeste(456);

        // Act & Assert
        Assert.That(id1, Is.Not.EqualTo(id2));
    }

    /// <summary>
    /// Testa a conversão implícita de inteiro para IdEntidadeBaseInt.
    /// </summary>
    [Test]
    public void IdEntidadeBaseInt_ConversaoImplicitaDeInteiro_DeveConverter()
    {
        // Arrange
        int valor = 123;

        // Act
        IdEntidadeBaseIntTeste id = valor;

        // Assert
        Assert.That(id.ValorInteiro, Is.EqualTo(valor));
    }

    /// <summary>
    /// Testa a conversão implícita de IdEntidadeBaseInt para inteiro.
    /// </summary>
    [Test]
    public void IdEntidadeBaseInt_ConversaoImplicitaParaInteiro_DeveConverter()
    {
        // Arrange
        var id = new IdEntidadeBaseIntTeste(123);

        // Act
        int valor = id;

        // Assert
        Assert.That(valor, Is.EqualTo(123));
    }

    /// <summary>
    /// Testa o método estático Criar.
    /// </summary>
    [Test]
    public void IdEntidadeBaseInt_MetodoCriar_DeveCriarCorretamente()
    {
        // Arrange
        var valor = 456;

        // Act
        var id = IdEntidadeBaseIntTeste.Criar(valor);

        // Assert
        Assert.That(id.ValorInteiro, Is.EqualTo(valor));
    }

    /// <summary>
    /// Testa se o ToString retorna a representação correta.
    /// </summary>
    [Test]
    public void IdEntidadeBaseInt_ToString_DeveRetornarRepresentacaoCorreta()
    {
        // Arrange
        var id = new IdEntidadeBaseIntTeste(789);

        // Act
        var resultado = id.ToString();

        // Assert
        Assert.That(resultado, Does.Contain("789"));
        Assert.That(resultado, Does.Contain("IdEntidadeBaseIntTeste"));
    }

    /// <summary>
    /// Testa se IdEntidadeBase lança exceção quando valor é nulo.
    /// </summary>
    [Test]
    public void IdEntidadeBase_ComValorNulo_DeveLancarExcecao()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new IdEntidadeTesteConcreta(null!));
    }

    /// <summary>
    /// Implementação concreta de IdEntidadeBase para testes.
    /// </summary>
    private record IdEntidadeTesteConcreta : IdEntidadeBase<object>
    {
        public IdEntidadeTesteConcreta(object valor) : base(valor)
        {
        }
    }
}
