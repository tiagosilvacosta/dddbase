# DddBase - Guia para Desenvolvedores

## Como Contribuir

### Pré-requisitos

- .NET 8.0 SDK ou superior
- Visual Studio 2022 ou Visual Studio Code
- Git

### Estrutura do Projeto

```
DddBase/
├── src/
│   ├── DddBase/                          # Projeto principal
│   │   ├── Base/                         # Estruturas base
│   │   └── Repositorio/                  # Interfaces de repositório
│   └── DddBase.TestesUnitarios/          # Testes unitários
├── docs/                                 # Documentação
├── build-package.ps1                     # Script de build
├── CHANGELOG.md                          # Histórico de mudanças
└── README.md                             # Documentação principal
```

### Como Desenvolver

1. **Clone o repositório**
   ```bash
   git clone https://github.com/usuario/dddbase.git
   cd dddbase
   ```

2. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

3. **Execute os testes**
   ```bash
   dotnet test
   ```

4. **Build do projeto**
   ```bash
   dotnet build
   ```

### Criação de Pacote NuGet

#### Usando o Script PowerShell (Recomendado)

```powershell
# Build e teste
.\build-package.ps1

# Criar pacote
.\build-package.ps1 -Pack

# Criar pacote com versão específica
.\build-package.ps1 -Pack -Version "1.0.1"

# Criar e publicar (precisa de API Key)
.\build-package.ps1 -Pack -Push -ApiKey "sua-api-key-aqui"
```

#### Usando Comandos Diretos

```bash
# Criar pacote
dotnet pack src/DddBase/DddBase.csproj --configuration Release

# Publicar no NuGet.org
dotnet nuget push src/DddBase/bin/Release/DddBase.1.0.0.nupkg --api-key SUA_API_KEY --source https://api.nuget.org/v3/index.json
```

### Publicação no NuGet

1. **Obtenha uma API Key**
   - Registre-se em [nuget.org](https://www.nuget.org/)
   - Vá para Account Settings > API Keys
   - Crie uma nova API Key

2. **Configure a API Key localmente**
   ```bash
   dotnet nuget setapikey SUA_API_KEY --source https://api.nuget.org/v3/index.json
   ```

3. **Publique o pacote**
   ```bash
   .\build-package.ps1 -Pack -Push -ApiKey "SUA_API_KEY"
   ```

### Versionamento

Este projeto segue [Semantic Versioning](https://semver.org/):

- **MAJOR**: Mudanças incompatíveis na API
- **MINOR**: Novas funcionalidades compatíveis
- **PATCH**: Correções de bugs compatíveis

### Processo de Release

1. **Atualize a versão** em `DddBase.csproj`
2. **Atualize o CHANGELOG.md** com as mudanças
3. **Execute todos os testes** para garantir que passam
4. **Crie um commit** com as mudanças
5. **Crie uma tag** com a versão: `git tag v1.0.0`
6. **Push das mudanças**: `git push && git push --tags`
7. **Crie o pacote**: `.\build-package.ps1 -Pack`
8. **Publique**: `.\build-package.ps1 -Push -ApiKey "SUA_API_KEY"`

### Padrões de Codificação

- Nomes de métodos em PascalCase
- Nomes de variáveis em camelCase
- Métodos assíncronos com sufixo "Async"
- Nomes em português do Brasil
- Documentação XML obrigatória para APIs públicas
- Testes unitários para todas as funcionalidades

### Estrutura de Testes

- Um arquivo de teste por classe
- Nomes descritivos: `NomeClasse_Condicao_ComportamentoEsperado`
- Usar padrão AAA: Arrange, Act, Assert
- Mock com NSubstitute quando necessário
