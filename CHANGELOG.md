# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Semantic Versioning](https://semver.org/lang/pt-BR/).

## [1.0.1] - 2025-07-09

### Adicionado
- Arquivo README.md incluído no pacote NuGet
- Melhorada a documentação do pacote

### Alterado
- Nome do pacote alterado de "DddBase" para "Tsc.DddBase" para evitar conflitos

## [1.0.0] - 2025-07-09

### Adicionado
- Implementação inicial das estruturas base para Domain Driven Design
- **ObjetoDeValor**: Record abstrato e imutável para objetos de valor
- **IdEntidadeBase<T>**: Classe genérica abstrata para identificadores type-safe
- **IdEntidadeBaseInt**: Record abstrato para identificadores baseados em inteiros
- **EntidadeBase<TId>**: Classe abstrata base para entidades com igualdade baseada em identificador
- **IRaizAgregado**: Interface marcador para raízes de agregado
- **IRepositorio<TEntidade, TId>**: Interface genérica para repositórios
- Testes unitários abrangentes com 38+ testes
- Documentação completa com exemplos de uso
- Guia de implementação detalhado

### Características
- Type safety garantido através de genéricos
- Imutabilidade para objetos de valor
- Igualdade baseada em identificador para entidades
- Validações incorporadas
- Compatível com ORMs através de construtores protegidos
- Seguindo princípios SOLID e Clean Code
- Estrutura baseada em domínio, não em aspectos técnicos

### Tecnologias
- .NET 8.0
- NUnit para testes
- NSubstitute para mocking
- Source Link para debugging
- Nullable reference types habilitado
