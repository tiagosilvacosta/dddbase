# Script para build e empacotamento do Tsc.DddBase
param(
    [string]$Configuration = "Release",
    [string]$Version = "",
    [switch]$Pack = $false,
    [switch]$Push = $false,
    [string]$ApiKey = "",
    [string]$Source = "https://api.nuget.org/v3/index.json"
)

Write-Host "=== Build e Empacotamento Tsc.DddBase ===" -ForegroundColor Green

# Limpar build anterior
Write-Host "Limpando builds anteriores..." -ForegroundColor Yellow
dotnet clean

# Restaurar dependências
Write-Host "Restaurando dependências..." -ForegroundColor Yellow
dotnet restore

# Executar testes
Write-Host "Executando testes..." -ForegroundColor Yellow
dotnet test --configuration $Configuration --no-restore --verbosity normal
if ($LASTEXITCODE -ne 0) {
    Write-Host "Testes falharam! Abortando build." -ForegroundColor Red
    exit 1
}

# Build do projeto
Write-Host "Compilando projeto..." -ForegroundColor Yellow
if ($Version) {
    dotnet build --configuration $Configuration --no-restore -p:Version=$Version
} else {
    dotnet build --configuration $Configuration --no-restore
}

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build falhou!" -ForegroundColor Red
    exit 1
}

# Criar pacote se solicitado
if ($Pack) {
    Write-Host "Criando pacote NuGet..." -ForegroundColor Yellow
    if ($Version) {
        dotnet pack src/DddBase/DddBase.csproj --configuration $Configuration --no-build -p:Version=$Version
    } else {
        dotnet pack src/DddBase/DddBase.csproj --configuration $Configuration --no-build
    }
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Empacotamento falhou!" -ForegroundColor Red
        exit 1
    }
}

# Publicar pacote se solicitado
if ($Push -and $ApiKey) {
    Write-Host "Publicando pacote..." -ForegroundColor Yellow
    $packagePath = Get-ChildItem "src/DddBase/bin/$Configuration/*.nupkg" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
    if ($packagePath) {
        dotnet nuget push $packagePath.FullName --api-key $ApiKey --source $Source
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Pacote publicado com sucesso!" -ForegroundColor Green
        } else {
            Write-Host "Falha na publicação do pacote!" -ForegroundColor Red
            exit 1
        }
    } else {
        Write-Host "Pacote não encontrado!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "=== Processo concluído com sucesso! ===" -ForegroundColor Green

# Exemplos de uso:
# .\build-package.ps1 -Pack                                    # Apenas criar o pacote
# .\build-package.ps1 -Pack -Version "1.0.1"                  # Criar pacote com versão específica
# .\build-package.ps1 -Pack -Push -ApiKey "sua-api-key"       # Criar e publicar pacote
