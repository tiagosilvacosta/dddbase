using DddBase.Base;
using System.Linq.Expressions;

namespace DddBase.Repositorio;

/// <summary>
/// Interface genérica para repositórios que implementam o padrão Repository.
/// Fornece operações básicas de acesso a dados para entidades que são raiz de agregado.
/// </summary>
/// <typeparam name="TEntidade">Tipo da entidade que deve herdar de EntidadeBase e implementar IRaizAgregado</typeparam>
/// <typeparam name="TId">Tipo do identificador da entidade</typeparam>
public interface IRepositorio<TEntidade, TId> 
    where TEntidade : EntidadeBase<TId>, IRaizAgregado
    where TId : ObjetoDeValor
{
    /// <summary>
    /// Obtém uma entidade pelo seu identificador de forma assíncrona.
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Entidade encontrada ou null se não existir</returns>
    Task<TEntidade?> ObterPorIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as entidades de forma assíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Coleção de entidades</returns>
    Task<IEnumerable<TEntidade>> ObterTodosAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém entidades que satisfazem uma condição específica de forma assíncrona.
    /// </summary>
    /// <param name="predicado">Expressão que define a condição de busca</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Coleção de entidades que satisfazem a condição</returns>
    Task<IEnumerable<TEntidade>> ObterPorCondicaoAsync(
        Expression<Func<TEntidade, bool>> predicado, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma nova entidade de forma assíncrona.
    /// </summary>
    /// <param name="entidade">Entidade a ser adicionada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Entidade adicionada</returns>
    Task<TEntidade> AdicionarAsync(TEntidade entidade, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente de forma assíncrona.
    /// </summary>
    /// <param name="entidade">Entidade a ser atualizada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Entidade atualizada</returns>
    Task<TEntidade> AtualizarAsync(TEntidade entidade, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma entidade pelo seu identificador de forma assíncrona.
    /// </summary>
    /// <param name="id">Identificador da entidade a ser removida</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se a entidade foi removida, false se não foi encontrada</returns>
    Task<bool> RemoverAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma entidade de forma assíncrona.
    /// </summary>
    /// <param name="entidade">Entidade a ser removida</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se a entidade foi removida, false se não foi encontrada</returns>
    Task<bool> RemoverAsync(TEntidade entidade, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se uma entidade existe pelo seu identificador de forma assíncrona.
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se a entidade existir, caso contrário false</returns>
    Task<bool> ExisteAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o total de entidades de forma assíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número total de entidades</returns>
    Task<int> ContarAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o total de entidades que satisfazem uma condição específica de forma assíncrona.
    /// </summary>
    /// <param name="predicado">Expressão que define a condição de contagem</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de entidades que satisfazem a condição</returns>
    Task<int> ContarAsync(
        Expression<Func<TEntidade, bool>> predicado, 
        CancellationToken cancellationToken = default);
}
