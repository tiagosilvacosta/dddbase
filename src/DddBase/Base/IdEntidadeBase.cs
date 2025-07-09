using System.Collections.Generic;

namespace DddBase.Base;

/// <summary>
/// Classe abstrata que representa o identificador base de uma entidade.
/// Esta classe deve ser herdada para definir tipos específicos de identificadores.
/// </summary>
public abstract record IdEntidadeBase : ObjetoDeValor
{
    /// <summary>
    /// Valor do identificador.
    /// </summary>
    public object Valor { get; protected init; }

    /// <summary>
    /// Inicializa uma nova instância do identificador base.
    /// </summary>
    /// <param name="valor">Valor do identificador</param>
    protected IdEntidadeBase(object valor)
    {
        if (valor == null)
            throw new ArgumentNullException(nameof(valor), "O valor do identificador não pode ser nulo");

        Valor = valor;
    }

    /// <summary>
    /// Obtém os componentes de igualdade do identificador.
    /// </summary>
    /// <returns>Enumerable com os componentes que definem a igualdade</returns>
    protected override IEnumerable<object?> ObterComponentesDeIgualdade()
    {
        yield return Valor;
    }

    /// <summary>
    /// Retorna uma representação em string do identificador.
    /// </summary>
    /// <returns>String representando o valor do identificador</returns>
    public override string ToString()
    {
        return Valor?.ToString() ?? string.Empty;
    }
}
