# Guia de Implementação DDD

## Introdução

Este documento fornece um guia prático para utilizar as estruturas do DddBase em seus projetos que seguem Domain Driven Design.

## Conceitos Fundamentais

### Objetos de Valor vs Entidades

#### Objetos de Valor
- São imutáveis
- Sua identidade é definida pelos seus valores
- Não possuem ciclo de vida próprio
- Exemplos: Endereco, Money, Email

#### Entidades
- Possuem identidade única
- Podem ser mutáveis
- Possuem ciclo de vida
- Exemplos: Cliente, Pedido, Produto

### Agregados e Raízes de Agregado

Um agregado é um conjunto de entidades que são tratadas como uma unidade para mudanças de dados. A raiz do agregado é a única entidade do agregado que objetos externos podem referenciar.

## Padrões de Implementação

### 1. Criando Identificadores Customizados

```csharp
// Para identificadores baseados em GUID
public record ClienteId : IdEntidadeBase<Guid>
{
    public Guid ValorGuid => Valor;

    public ClienteId(Guid valor) : base(valor)
    {
        if (valor == Guid.Empty)
            throw new ArgumentException("ClienteId não pode ser vazio", nameof(valor));
    }

    public static ClienteId Novo() => new(Guid.NewGuid());
    
    public static implicit operator ClienteId(Guid valor) => new(valor);
    public static implicit operator Guid(ClienteId id) => id.ValorGuid;
}

// Para identificadores baseados em string
public record ProdutoSku : IdEntidadeBase<string>
{
    public string ValorString => Valor;

    public ProdutoSku(string valor) : base(valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("SKU não pode ser vazio", nameof(valor));
        
        if (valor.Length > 20)
            throw new ArgumentException("SKU não pode ter mais de 20 caracteres", nameof(valor));
    public static implicit operator ProdutoSku(string valor) => new(valor);
    public static implicit operator string(ProdutoSku sku) => sku.ValorString;
}
```

### 2. Implementando Objetos de Valor Complexos

```csharp
public record Money : ObjetoDeValor
{
    public decimal Valor { get; }
    public string Moeda { get; }

    public Money(decimal valor, string moeda = "BRL")
    {
        if (valor < 0)
            throw new ArgumentException("Valor não pode ser negativo", nameof(valor));
        
        if (string.IsNullOrWhiteSpace(moeda))
            throw new ArgumentException("Moeda é obrigatória", nameof(moeda));

        Valor = valor;
        Moeda = moeda.ToUpper();
    }

    public Money Somar(Money outraMoeda)
    {
        if (Moeda != outraMoeda.Moeda)
            throw new InvalidOperationException("Não é possível somar moedas diferentes");

        return new Money(Valor + outraMoeda.Valor, Moeda);
    }

    public static Money operator +(Money left, Money right) => left.Somar(right);
}

public record Email : ObjetoDeValor
{
    public string Valor { get; }

    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Email não pode ser vazio", nameof(valor));

        if (!IsValidEmail(valor))
            throw new ArgumentException("Email inválido", nameof(valor));

        Valor = valor.ToLowerInvariant();
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static implicit operator Email(string valor) => new(valor);
    public static implicit operator string(Email email) => email.Valor;
}
```

### 3. Criando Entidades com Validações

```csharp
public class Cliente : EntidadeBase<ClienteId>, IRaizAgregado
{
    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public Endereco EnderecoEntrega { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Ativo { get; private set; }

    // Lista privada para manter encapsulamento
    private readonly List<Pedido> _pedidos = new();
    public IReadOnlyList<Pedido> Pedidos => _pedidos.AsReadOnly();

    public Cliente(ClienteId id, string nome, Email email, Endereco enderecoEntrega) : base(id)
    {
        AtualizarNome(nome);
        AtualizarEmail(email);
        AtualizarEnderecoEntrega(enderecoEntrega);
        DataCadastro = DateTime.UtcNow;
        Ativo = true;
    }

    // Construtor para ORMs
    protected Cliente() : base()
    {
        Nome = string.Empty;
        Email = null!;
        EnderecoEntrega = null!;
    }

    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("Nome não pode ser vazio", nameof(novoNome));

        if (novoNome.Length > 200)
            throw new ArgumentException("Nome não pode ter mais de 200 caracteres", nameof(novoNome));

        Nome = novoNome.Trim();
    }

    public void AtualizarEmail(Email novoEmail)
    {
        Email = novoEmail ?? throw new ArgumentNullException(nameof(novoEmail));
    }

    public void AtualizarEnderecoEntrega(Endereco novoEndereco)
    {
        EnderecoEntrega = novoEndereco ?? throw new ArgumentNullException(nameof(novoEndereco));
    }

    public void Ativar()
    {
        Ativo = true;
    }

    public void Desativar()
    {
        if (_pedidos.Any(p => p.Status == StatusPedido.Pendente))
            throw new InvalidOperationException("Não é possível desativar cliente com pedidos pendentes");

        Ativo = false;
    }

    public void AdicionarPedido(Pedido pedido)
    {
        if (!Ativo)
            throw new InvalidOperationException("Cliente inativo não pode fazer pedidos");

        _pedidos.Add(pedido);
    }
}
```

### 4. Implementação de Repositório

```csharp
public interface IClienteRepositorio : IRepositorio<Cliente, ClienteId>
{
    Task<Cliente?> ObterPorEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Cliente>> ObterClientesAtivosAsync(CancellationToken cancellationToken = default);
    Task<bool> EmailJaExisteAsync(Email email, ClienteId? excetoPara = null, CancellationToken cancellationToken = default);
}

// Implementação usando Entity Framework (exemplo)
public class ClienteRepositorioEF : IClienteRepositorio
{
    private readonly DbContext _context;

    public ClienteRepositorioEF(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Cliente?> ObterPorIdAsync(ClienteId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Cliente>()
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Cliente?> ObterPorEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Cliente>()
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Cliente>> ObterClientesAtivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Cliente>()
            .Where(c => c.Ativo)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> EmailJaExisteAsync(Email email, ClienteId? excetoPara = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Cliente>().Where(c => c.Email == email);
        
        if (excetoPara != null)
            query = query.Where(c => c.Id != excetoPara);

        return await query.AnyAsync(cancellationToken);
    }

    // Implementar outros métodos da interface IRepositorio...
}
```

## Melhores Práticas

### 1. Validação de Dados

```csharp
// Sempre valide dados de entrada
public void AtualizarIdade(int idade)
{
    if (idade < 0)
        throw new ArgumentException("Idade não pode ser negativa", nameof(idade));
    
    if (idade > 150)
        throw new ArgumentException("Idade não pode ser maior que 150 anos", nameof(idade));

    _idade = idade;
}
```

### 2. Encapsulamento

```csharp
// Use propriedades privadas para setters quando necessário
public class Pedido : EntidadeBase<PedidoId>, IRaizAgregado
{
    public StatusPedido Status { get; private set; }
    public Money ValorTotal { get; private set; }
    
    // Lista privada com propriedade somente leitura
    private readonly List<ItemPedido> _itens = new();
    public IReadOnlyList<ItemPedido> Itens => _itens.AsReadOnly();

    public void AdicionarItem(Produto produto, int quantidade)
    {
        if (Status != StatusPedido.Rascunho)
            throw new InvalidOperationException("Não é possível adicionar itens a um pedido confirmado");

        _itens.Add(new ItemPedido(produto, quantidade));
        RecalcularTotal();
    }

    private void RecalcularTotal()
    {
        ValorTotal = _itens.Sum(i => i.ValorTotal);
    }
}
```

### 3. Tratamento de Erros

```csharp
// Use exceções específicas para diferentes tipos de erro
public class ClienteNaoEncontradoException : Exception
{
    public ClienteId ClienteId { get; }

    public ClienteNaoEncontradoException(ClienteId clienteId) 
        : base($"Cliente com ID {clienteId} não foi encontrado")
    {
        ClienteId = clienteId;
    }
}

public class EmailJaExisteException : Exception
{
    public Email Email { get; }

    public EmailJaExisteException(Email email) 
        : base($"Já existe um cliente com o email {email}")
    {
        Email = email;
    }
}
```

### 4. Testes de Domínio

```csharp
[TestFixture]
public class ClienteTestes
{
    [Test]
    public void Cliente_AoTentarDesativarComPedidosPendentes_DeveLancarExcecao()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var pedido = CriarPedidoPendente();
        cliente.AdicionarPedido(pedido);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cliente.Desativar());
    }

    [Test]
    public void Cliente_AoAtualizarEmailValido_DeveAtualizarCorretamente()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var novoEmail = new Email("novo@exemplo.com");

        // Act
        cliente.AtualizarEmail(novoEmail);

        // Assert
        Assert.That(cliente.Email, Is.EqualTo(novoEmail));
    }

    private Cliente CriarClienteValido()
    {
        return new Cliente(
            ClienteId.Novo(),
            "João Silva",
            new Email("joao@exemplo.com"),
            CriarEnderecoValido()
        );
    }
}
```

## Integração com Frameworks

### Configuração Entity Framework

```csharp
public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        // Configuração da tabela
        builder.ToTable("Clientes");

        // Configuração da chave primária
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.ValorGuid,
                valor => new ClienteId(valor));

        // Configuração de propriedades
        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                email => email.Valor,
                valor => new Email(valor));

        // Configuração de objeto de valor complexo
        builder.OwnsOne(c => c.EnderecoEntrega, endereco =>
        {
            endereco.Property(e => e.Rua).HasColumnName("EnderecoRua").HasMaxLength(200);
            endereco.Property(e => e.Cidade).HasColumnName("EnderecoCidade").HasMaxLength(100);
            endereco.Property(e => e.CEP).HasColumnName("EnderecoCEP").HasMaxLength(10);
        });

        // Relacionamentos
        builder.HasMany(c => c.Pedidos)
            .WithOne()
            .HasForeignKey("ClienteId");
    }
}
```

Este guia fornece uma base sólida para implementar Domain Driven Design usando as estruturas do DddBase. Lembre-se sempre de manter o foco na linguagem ubíqua do domínio e nas regras de negócio.
