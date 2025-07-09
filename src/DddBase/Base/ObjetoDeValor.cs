using System.Collections.Generic;
using System.Linq;

namespace DddBase.Base;

/// <summary>
/// Classe abstrata que representa um objeto de valor no Domain Driven Design.
/// Objetos de valor são imutáveis e sua igualdade é baseada nos valores dos seus atributos.
/// </summary>
public abstract record ObjetoDeValor
{
    /// <summary>
    /// Obtém os componentes de igualdade do objeto de valor.
    /// Implementações devem retornar todos os valores que definem a igualdade.
    /// </summary>
    /// <returns>Enumerable com os componentes que definem a igualdade</returns>
    protected abstract IEnumerable<object?> ObterComponentesDeIgualdade();

    /// <summary>
    /// Determina se o objeto atual é igual ao objeto especificado.
    /// </summary>
    /// <param name="obj">Objeto a ser comparado</param>
    /// <returns>True se os objetos forem iguais, caso contrário false</returns>
    public override int GetHashCode()
    {
        return ObterComponentesDeIgualdade()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }



    /// <summary>
    /// Determina se o objeto atual é igual ao objeto especificado.
    /// </summary>
    /// <param name="obj">Objeto a ser comparado</param>
    /// <returns>True se os objetos forem iguais, caso contrário false</returns>
    public virtual bool Equals(ObjetoDeValor? other)
    {
        if (other == null || GetType() != other.GetType())
            return false;

        return ObterComponentesDeIgualdade().SequenceEqual(other.ObterComponentesDeIgualdade());
    }
}
