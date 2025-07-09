namespace DddBase.Base;

/// <summary>
/// Classe abstrata que representa o identificador base de uma entidade.
/// Esta classe deve ser herdada para definir tipos específicos de identificadores.
/// </summary>
/// <typeparam name="T">Tipo do valor do identificador</typeparam>
public abstract record IdEntidadeBase<T> : ObjetoDeValor
{
    /// <summary>
    /// Valor do identificador.
    /// </summary>
    public T Valor { get; protected init; }

    /// <summary>
    /// Inicializa uma nova instância do identificador base.
    /// </summary>
    /// <param name="valor">Valor do identificador</param>
    protected IdEntidadeBase(T valor)
    {
        if (valor == null)
            throw new ArgumentNullException(nameof(valor), "O valor do identificador não pode ser nulo");

        Valor = valor;
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
