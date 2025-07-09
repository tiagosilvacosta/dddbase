# DddBase - Base para Projetos DDD

## Objetivo

Este projeto implementa as estruturas fundamentais para projetos que utilizem a abordagem Domain Driven Design (DDD), fornecendo uma base sólida e bem testada para o desenvolvimento de aplicações.

## Estruturas Implementadas

### 📋 ObjetoDeValor
- **Tipo**: Record abstrato e imutável
- **Função**: Classe base para todos os objetos de valor do domínio
- **Características**:
  - Imutabilidade garantida pelo uso de records
  - Igualdade, GetHashCode e ToString gerados automaticamente pelo record
  - Estrutura simples que permite foco na lógica de domínio

### 🆔 IdEntidadeBase<T>
- **Tipo**: Objeto de valor genérico abstrato
- **Função**: Base para identidades únicas de entidades com type safety
- **Características**:
  - Genérico para garantir type safety no valor do identificador
  - Deve ser herdado para definir tipos específicos de identificadores
  - Validação para valores não nulos
  - Eliminação de casting desnecessário

### 🔢 IdEntidadeBaseInt
- **Tipo**: Record abstrato que herda de IdEntidadeBase<int>
- **Função**: Base para identificadores baseados em números inteiros
- **Características**:
  - Herda de IdEntidadeBase<int> para type safety
  - Deve ser herdado por cada entidade para definir sua própria identidade
  - Validação para valores maiores que zero
  - Propriedade ValorInteiro para acesso direto ao valor inteiro
  - Cada implementação concreta deve fornecer conversões implícitas e métodos estáticos

### 🏗️ EntidadeBase<TId>
- **Tipo**: Classe abstrata genérica
- **Função**: Base para todas as entidades do domínio
- **Características**:
  - Propriedade Id do tipo genérico que herda de IdEntidadeBase
  - Igualdade baseada no identificador
  - Construtor protegido para ORMs
  - Sobrescrita de ToString para depuração

### 🏛️ IRaizAgregado
- **Tipo**: Interface marcador
- **Função**: Identifica entidades que são raízes de agregado
- **Características**:
  - Interface sem métodos (marker interface)
  - Indica que a entidade controla o acesso às entidades filhas
  - Usada como restrição genérica no repositório

### 📊 IRepositorio<TEntidade, TId>
- **Tipo**: Interface genérica
- **Função**: Contrato para acesso a dados seguindo o padrão Repository
- **Características**:
  - Restrições genéricas que garantem DDD (TEntidade deve implementar IRaizAgregado)
  - Operações CRUD completas assíncronas
  - Métodos de consulta com expressões lambda
  - Suporte a CancellationToken

## Estrutura do Projeto

```
DddBase/
├── src/
│   ├── DddBase/                           # Projeto principal
│   │   ├── Base/                          # Estruturas base do DDD
│   │   │   ├── ObjetoDeValor.cs
│   │   │   ├── IdEntidadeBase.cs
│   │   │   ├── IdEntidadeBaseInt.cs
│   │   │   ├── EntidadeBase.cs
│   │   │   └── IRaizAgregado.cs
│   │   └── Repositorio/                   # Interfaces de repositório
│   │       └── IRepositorio.cs
│   └── DddBase.TestesUnitarios/           # Testes unitários
│       ├── Base/                          # Testes das estruturas base
│       └── Repositorio/                   # Testes dos repositórios
├── docs/                                  # Documentação
├── DddBase.sln                            # Arquivo de solução
└── README.md                              # Este arquivo
```

## Como Usar

### 1. Criando um Objeto de Valor

```csharp
public record Endereco : ObjetoDeValor
{
    public string Rua { get; }
    public string Cidade { get; }
    public string CEP { get; }

    public Endereco(string rua, string cidade, string cep)
    {
        Rua = rua;
        Cidade = cidade;
        CEP = cep;
    }
}
```

### 2. Criando um Identificador de Entidade

```csharp
// Primeiro, crie um identificador específico para sua entidade
public record IdCliente : IdEntidadeBaseInt
{
    public IdCliente(int valor) : base(valor)
    {
    }

    public static IdCliente Criar(int valor) => new(valor);
    
    public static implicit operator IdCliente(int valor) => new(valor);
    public static implicit operator int(IdCliente id) => id.ValorInteiro;
}
```

### 3. Criando uma Entidade

```csharp
public class Cliente : EntidadeBase<IdCliente>, IRaizAgregado
{
    public string Nome { get; set; }
    public Endereco Endereco { get; set; }

    public Cliente(IdCliente id, string nome, Endereco endereco) : base(id)
    {
        Nome = nome;
        Endereco = endereco;
    }

    // Construtor para ORMs
    protected Cliente() : base()
    {
        Nome = string.Empty;
        Endereco = null!;
    }
}
```

### 4. Implementando um Repositório

```csharp
public interface IClienteRepositorio : IRepositorio<Cliente, IdCliente>
{
    Task<Cliente?> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default);
}

public class ClienteRepositorio : IClienteRepositorio
{
    // Implementação dos métodos da interface IRepositorio
    // e do método específico ObterPorNomeAsync
}
```

## Tecnologias Utilizadas

- **.NET 9.0**: Framework principal
- **C# 12**: Linguagem de programação
- **NUnit**: Framework de testes
- **NSubstitute**: Framework de mocking para testes

## Execução dos Testes

```bash
# Compilar o projeto
dotnet build

# Executar todos os testes
dotnet test

# Executar testes com relatório detalhado
dotnet test --verbosity normal
```

## Estatísticas dos Testes

- **Total de Testes**: 38
- **Cobertura**: Todas as funcionalidades principais
- **Frameworks**: NUnit + NSubstitute
- **Status**: ✅ Todos os testes passando

## Princípios Aplicados

### SOLID
- **S**ingle Responsibility: Cada classe tem uma responsabilidade específica
- **O**pen/Closed: Estruturas abertas para extensão, fechadas para modificação
- **L**iskov Substitution: Subtipos podem substituir tipos base
- **I**nterface Segregation: Interfaces específicas e coesas
- **D**ependency Inversion: Dependência de abstrações, não implementações

### Clean Code
- Nomes descritivos em português do Brasil
- Métodos pequenos e focados
- Comentários XML para documentação
- Validação de entradas
- Tratamento adequado de exceções

### Domain Driven Design
- Separação clara entre entidades e objetos de valor
- Padrão Repository para acesso a dados
- Interface marcador para raízes de agregado
- Foco na linguagem ubíqua do domínio

## Contribuição

Este projeto segue as boas práticas definidas nos padrões de codificação da organização. Para contribuir:

1. Mantenha os nomes em português do Brasil
2. Implemente testes unitários para novas funcionalidades
3. Siga os princípios SOLID e Clean Code
4. Documente o código com comentários XML
5. Valide todas as entradas de métodos

## Licença

Este projeto está sob a licença especificada no arquivo LICENSE.