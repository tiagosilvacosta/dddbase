namespace DddBase.Base;

/// <summary>
/// Classe abstrata que representa uma entidade base no Domain Driven Design.
/// Entidades possuem identidade única e podem ser mutáveis.
/// </summary>
/// <typeparam name="TId">Tipo do identificador da entidade</typeparam>
public abstract class EntidadeBase<TId> where TId : IdEntidadeBase
{
    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// Inicializa uma nova instância da entidade base.
    /// </summary>
    /// <param name="id">Identificador único da entidade</param>
    protected EntidadeBase(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id), "O identificador da entidade não pode ser nulo");
    }

    /// <summary>
    /// Construtor protegido sem parâmetros para uso por ORMs.
    /// </summary>
    protected EntidadeBase()
    {
        // Construtor para ORMs - Id será definido posteriormente
    }

    /// <summary>
    /// Determina se a entidade atual é igual à entidade especificada.
    /// A igualdade é baseada no identificador.
    /// </summary>
    /// <param name="obj">Entidade a ser comparada</param>
    /// <returns>True se as entidades forem iguais, caso contrário false</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        var outraEntidade = (EntidadeBase<TId>)obj;

        if (Id == null || outraEntidade.Id == null)
            return false;

        return Id.Equals(outraEntidade.Id);
    }

    /// <summary>
    /// Calcula o hash code da entidade baseado no identificador.
    /// </summary>
    /// <returns>Hash code da entidade</returns>
    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }

    /// <summary>
    /// Verifica se duas entidades são iguais.
    /// </summary>
    /// <param name="left">Primeira entidade</param>
    /// <param name="right">Segunda entidade</param>
    /// <returns>True se as entidades forem iguais, caso contrário false</returns>
    public static bool operator ==(EntidadeBase<TId>? left, EntidadeBase<TId>? right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            return true;

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Verifica se duas entidades são diferentes.
    /// </summary>
    /// <param name="left">Primeira entidade</param>
    /// <param name="right">Segunda entidade</param>
    /// <returns>True se as entidades forem diferentes, caso contrário false</returns>
    public static bool operator !=(EntidadeBase<TId>? left, EntidadeBase<TId>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Retorna uma representação em string da entidade.
    /// </summary>
    /// <returns>String representando a entidade</returns>
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}
