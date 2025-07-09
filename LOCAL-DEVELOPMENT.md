# Configuração para Desenvolvimento Local

## Testando o Pacote Localmente

### 1. Criar um Feed Local de NuGet

```bash
# Criar diretório para o feed local
mkdir C:\LocalNuGetFeed

# Adicionar o feed local ao NuGet
dotnet nuget add source C:\LocalNuGetFeed --name "Local"
```

### 2. Copiar o Pacote para o Feed Local

```powershell
# Após criar o pacote
Copy-Item "src\DddBase\bin\Release\*.nupkg" "C:\LocalNuGetFeed\"
```

### 3. Criar Projeto de Teste

```bash
# Criar projeto de teste
mkdir TesteDddBase
cd TesteDddBase
dotnet new console

# Adicionar referência ao pacote local
dotnet add package Tsc.DddBase --version 1.0.0 --source "C:\LocalNuGetFeed"
```

### 4. Exemplo de Uso no Projeto de Teste

```csharp
using DddBase.Base;
using DddBase.Repositorio;

// Criar identificador específico
public record IdCliente : IdEntidadeBaseInt
{
    public IdCliente(int valor) : base(valor) { }
    
    public static IdCliente Criar(int valor) => new(valor);
    
    public static implicit operator IdCliente(int valor) => new(valor);
    public static implicit operator int(IdCliente id) => id.ValorInteiro;
}

// Criar objeto de valor
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

// Criar entidade
public class Cliente : EntidadeBase<IdCliente>, IRaizAgregado
{
    public string Nome { get; set; }
    public Endereco Endereco { get; set; }

    public Cliente(IdCliente id, string nome, Endereco endereco) : base(id)
    {
        Nome = nome;
        Endereco = endereco;
    }

    protected Cliente() : base()
    {
        Nome = string.Empty;
        Endereco = null!;
    }
}

// Criar repositório
public interface IClienteRepositorio : IRepositorio<Cliente, IdCliente>
{
    Task<Cliente?> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default);
}

// Uso
class Program
{
    static void Main()
    {
        var id = IdCliente.Criar(1);
        var endereco = new Endereco("Rua A", "Cidade B", "12345-678");
        var cliente = new Cliente(id, "João Silva", endereco);
        
        Console.WriteLine($"Cliente: {cliente}");
    }
}
```

## Comandos Úteis

```bash
# Limpar cache do NuGet
dotnet nuget locals all --clear

# Listar sources do NuGet
dotnet nuget list source

# Remover source local
dotnet nuget remove source "Local"

# Verificar versões disponíveis
dotnet list package --outdated

# Atualizar pacote
dotnet add package Tsc.DddBase --version 1.0.1
```
