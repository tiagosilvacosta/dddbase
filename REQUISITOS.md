# Projeto: Base para projetos DDD
## Objteivo: Implementar as estruturas que serão usadas em projetos que usem a abordagem DDD

### Estruturas a serem criadas:

- Nome do projeto: DddBase
- ObjetoDeValor: Um record, imutável, abstrato, que precisa ser herdado
- IdEntidadeBase: Um objeto de valor que será a identidade única de uma entidade, mas que precisa ser herdado, pois aqui ainda não temos os campos que definem a identidade
- EntidadeBase: Uma classe abstrata, que dever ter uma propriedade Id que seja herdada do IdEntidadeBase
- IdEntidadeBaseInt: Uma objeto de valor abstrato, herdada de IdEntidadeBase, que já define que o a Identidade será um Inteiro. 
- IRaizAgregado: Uma interface "marcador", sem métodos, mas que precisará ser indicada nas entidades que são as raizes dos agregados
- IRepositorio: Uma interface genérica para acesso à dados que precisa ser definida por uma classe concreta da EntidadeBase e que esta classe implemente a interface IRaizAgregado.
