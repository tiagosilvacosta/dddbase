namespace DddBase.Base;

/// <summary>
/// Interface marcador que identifica uma entidade como raiz de agregado.
/// Entidades que implementam esta interface são pontos de entrada para o agregado
/// e controlam o acesso às entidades filhas.
/// </summary>
public interface IRaizAgregado
{
    // Interface marcador - não possui métodos
    // A implementação desta interface indica que a entidade é uma raiz de agregado
}
