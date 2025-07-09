# Boas Práticas para em Projetos .NET

## Configuração Inicial

### Estrutura do Projeto

- Utilize uma estrutura de diretórios clara e organizada:

```
MeuProjeto/
├── src/
│   ├── MeuProjeto/
│   ├── MeuProjeto.TestesUnitarios/
├── docs/
├── .gitignore
├── README.md
└── MeuProjeto.sln
```

### Comentários e Documentação

- Documente seu código com comentários XML para melhorar as sugestões do Copilot:

```
/// <summary>
/// Processa os dados de entrada e retorna o resultado formatado.
/// </summary>
/// <param name="input">Dados de entrada a serem processados</param>
/// <returns>Resultado formatado como string</returns>
public string ProcessarDados(string input)
{
    // Implementação
}
```

## Padrões de Codificação

- Os nomes dos métodos devem ser em PascalCase  
- Nomes de variáveis devem ser em camelCase  
- Métodos assíncronos devem ter o sufixo "Async"  
- Os nomes dos métodos e variáveis devem ser sempre em português do Brasil  
- Use os conceitos SOLID e do Clean Code para criar código de fácil manutenção  
- As classes de serviço devem ter sempre uma Interface associada  
- Usar injeção de dependências (Microsoft.Extensions.DependencyInjection)  
- Usar os padrões do Domain Driven Design  
- Os subdiretórios dos componentes devem ser nomeados de acordo com o domínio e não com as questões técnicas (Por exemplo: não devem ter diretório "serviços" ou "repositório" e sim diretórios que expliquem o domínio que estamos tratando, como por exemplo, "contrato" ou "proposta" e, dentro deles, os serviços, modelos, repositórios, interfaces, etc...)

## Boas Práticas de Código

### Tratamento de Erros

- Sempre tratar as exceções e logar as mesmas fornecendo o máximo de informações para que o erro seja identificado e resolvido  
- Sempre checar potenciais erros como, por exemplo, acesso a um índice de array inexistente.

### Testes Unitários

- Sempre crie testes unitários para testar cada uma das funções  
- Use o componente NSubstitute para fazer o mock das classes que a classe de teste precisa para ser testada

### Padrões de Projeto

- Usar o padrão de Repositório para acesso aos dados

## Segurança e Boas Práticas

### Validação de Entrada

- Validar todas as entradas de métodos para que contenham valores válidos

## Arquivos de Projeto

### .gitignore

- Todo projeto .net deve ter  um arquivo `.gitignore` adequado para projetos .NET

### README.md

- Todos projetos precisam ter README.md e ser sempre atualizado com as modificações mais recentes