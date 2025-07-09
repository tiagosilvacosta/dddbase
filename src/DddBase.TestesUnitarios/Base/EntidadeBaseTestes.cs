using DddBase.Base;
using NUnit.Framework;
using System;

namespace DddBase.TestesUnitarios.Base;

/// <summary>
/// Implementação concreta de EntidadeBase para testes.
/// </summary>
public class EntidadeTeste : EntidadeBase<IdEntidadeBaseInt>
{
    public string Nome { get; set; }

    public EntidadeTeste(IdEntidadeBaseInt id, string nome) : base(id)
    {
        Nome = nome;
    }

    // Construtor para ORMs
    protected EntidadeTeste() : base()
    {
        Nome = string.Empty;
    }
}

/// <summary>
/// Implementação de entidade que é raiz de agregado para testes.
/// </summary>
public class EntidadeRaizAgregadoTeste : EntidadeBase<IdEntidadeBaseInt>, IRaizAgregado
{
    public string Descricao { get; set; }

    public EntidadeRaizAgregadoTeste(IdEntidadeBaseInt id, string descricao) : base(id)
    {
        Descricao = descricao;
    }

    // Construtor para ORMs
    protected EntidadeRaizAgregadoTeste() : base()
    {
        Descricao = string.Empty;
    }
}

/// <summary>
/// Testes unitários para a classe EntidadeBase.
/// </summary>
[TestFixture]
public class EntidadeBaseTestes
{
    /// <summary>
    /// Testa se a entidade é criada corretamente com identificador válido.
    /// </summary>
    [Test]
    public void EntidadeBase_ComIdValido_DeveCriarCorretamente()
    {
        // Arrange
        var id = new IdEntidadeBaseInt(123);
        var nome = "Teste";

        // Act
        var entidade = new EntidadeTeste(id, nome);

        // Assert
        Assert.That(entidade.Id, Is.EqualTo(id));
        Assert.That(entidade.Nome, Is.EqualTo(nome));
    }

    /// <summary>
    /// Testa se a entidade lança exceção quando o identificador é nulo.
    /// </summary>
    [Test]
    public void EntidadeBase_ComIdNulo_DeveLancarExcecao()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EntidadeTeste(null!, "Teste"));
    }

    /// <summary>
    /// Testa se duas entidades com o mesmo identificador são consideradas iguais.
    /// </summary>
    [Test]
    public void EntidadeBase_ComMesmoId_DevemSerIguais()
    {
        // Arrange
        var id = new IdEntidadeBaseInt(123);
        var entidade1 = new EntidadeTeste(id, "Nome1");
        var entidade2 = new EntidadeTeste(id, "Nome2");

        // Act & Assert
        Assert.That(entidade1, Is.EqualTo(entidade2));
        Assert.That(entidade1.GetHashCode(), Is.EqualTo(entidade2.GetHashCode()));
    }

    /// <summary>
    /// Testa se duas entidades com identificadores diferentes são consideradas diferentes.
    /// </summary>
    [Test]
    public void EntidadeBase_ComIdsDiferentes_DevemSerDiferentes()
    {
        // Arrange
        var id1 = new IdEntidadeBaseInt(123);
        var id2 = new IdEntidadeBaseInt(456);
        var entidade1 = new EntidadeTeste(id1, "Nome");
        var entidade2 = new EntidadeTeste(id2, "Nome");

        // Act & Assert
        Assert.That(entidade1, Is.Not.EqualTo(entidade2));
    }

    /// <summary>
    /// Testa se a comparação com nulo retorna falso.
    /// </summary>
    [Test]
    public void EntidadeBase_ComparandoComNulo_DeveRetornarFalso()
    {
        // Arrange
        var id = new IdEntidadeBaseInt(123);
        var entidade = new EntidadeTeste(id, "Nome");

        // Act & Assert
        Assert.That(entidade.Equals(null), Is.False);
    }

    /// <summary>
    /// Testa se a comparação com objeto de tipo diferente retorna falso.
    /// </summary>
    [Test]
    public void EntidadeBase_ComparandoComTipoDiferente_DeveRetornarFalso()
    {
        // Arrange
        var id = new IdEntidadeBaseInt(123);
        var entidade = new EntidadeTeste(id, "Nome");
        var objeto = new object();

        // Act & Assert
        Assert.That(entidade.Equals(objeto), Is.False);
    }

    /// <summary>
    /// Testa se a mesma instância é considerada igual a si mesma.
    /// </summary>
    [Test]
    public void EntidadeBase_MesmaInstancia_DeveSerIgual()
    {
        // Arrange
        var id = new IdEntidadeBaseInt(123);
        var entidade = new EntidadeTeste(id, "Nome");

        // Act & Assert
        Assert.That(entidade.Equals(entidade), Is.True);
    }

    /// <summary>
    /// Testa se o ToString retorna a representação correta.
    /// </summary>
    [Test]
    public void EntidadeBase_ToString_DeveRetornarRepresentacaoCorreta()
    {
        // Arrange
        var id = new IdEntidadeBaseInt(123);
        var entidade = new EntidadeTeste(id, "Nome");

        // Act
        var resultado = entidade.ToString();

        // Assert
        Assert.That(resultado, Does.Contain("EntidadeTeste"));
        Assert.That(resultado, Does.Contain("123"));
    }

    /// <summary>
    /// Testa se a interface IRaizAgregado é implementada corretamente.
    /// </summary>
    [Test]
    public void EntidadeRaizAgregado_DeveImplementarInterface()
    {
        // Arrange
        var id = new IdEntidadeBaseInt(123);
        var entidade = new EntidadeRaizAgregadoTeste(id, "Descrição");

        // Act & Assert
        Assert.That(entidade, Is.InstanceOf<IRaizAgregado>());
        Assert.That(entidade, Is.InstanceOf<EntidadeBase<IdEntidadeBaseInt>>());
    }

    /// <summary>
    /// Testa se operadores de igualdade funcionam corretamente.
    /// </summary>
    [Test]
    public void EntidadeBase_OperadoresIgualdade_DevemFuncionar()
    {
        // Arrange
        var id = new IdEntidadeBaseInt(123);
        var entidade1 = new EntidadeTeste(id, "Nome1");
        var entidade2 = new EntidadeTeste(id, "Nome2");
        var entidade3 = new EntidadeTeste(new IdEntidadeBaseInt(456), "Nome3");

        // Act & Assert
        Assert.That(entidade1 == entidade2, Is.True);
        Assert.That(entidade1 != entidade3, Is.True);
        Assert.That(entidade1 == null, Is.False);
        Assert.That(null == entidade1, Is.False);
    }
}
