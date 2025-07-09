namespace DddBase.Base;

/// <summary>
/// Classe abstrata para identificadores de entidade baseados em inteiro.
/// Deve ser herdada por cada entidade concreta para definir sua própria identidade.
/// </summary>
public abstract record IdEntidadeBaseInt : IdEntidadeBase<int>
{
    /// <summary>
    /// Valor inteiro do identificador.
    /// </summary>
    public int ValorInteiro => Valor;

    /// <summary>
    /// Inicializa uma nova instância do identificador inteiro.
    /// </summary>
    /// <param name="valor">Valor inteiro do identificador</param>
    protected IdEntidadeBaseInt(int valor) : base(valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do identificador deve ser maior que zero", nameof(valor));
    }
}
