namespace DddBase.Base;

/// <summary>
/// Implementação concreta de um identificador de entidade baseado em inteiro.
/// </summary>
public record IdEntidadeBaseInt : IdEntidadeBase<int>
{
    /// <summary>
    /// Valor inteiro do identificador.
    /// </summary>
    public int ValorInteiro => Valor;

    /// <summary>
    /// Inicializa uma nova instância do identificador inteiro.
    /// </summary>
    /// <param name="valor">Valor inteiro do identificador</param>
    public IdEntidadeBaseInt(int valor) : base(valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do identificador deve ser maior que zero", nameof(valor));
    }

    /// <summary>
    /// Cria um novo identificador inteiro com o valor especificado.
    /// </summary>
    /// <param name="valor">Valor inteiro do identificador</param>
    /// <returns>Nova instância do identificador</returns>
    public static IdEntidadeBaseInt Criar(int valor)
    {
        return new IdEntidadeBaseInt(valor);
    }

    /// <summary>
    /// Converte implicitamente um inteiro para IdEntidadeBaseInt.
    /// </summary>
    /// <param name="valor">Valor inteiro</param>
    public static implicit operator IdEntidadeBaseInt(int valor)
    {
        return new IdEntidadeBaseInt(valor);
    }

    /// <summary>
    /// Converte implicitamente um IdEntidadeBaseInt para inteiro.
    /// </summary>
    /// <param name="id">Identificador</param>
    public static implicit operator int(IdEntidadeBaseInt id)
    {
        return id.ValorInteiro;
    }
}
