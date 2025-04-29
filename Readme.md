# Alteracao-SQL

![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue) ![C#](https://img.shields.io/badge/Language-C%23-blueviolet) ![MIT License](https://img.shields.io/badge/License-MIT-lightgrey)

> Ferramenta em console para ler arquivos SQL de inserção antigos e gerar comandos `INSERT` adaptados ao novo esquema de banco de dados.

---

## 📚 Sumário

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Configuração](#configuração)
- [Uso](#uso)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Classes Principais](#classes-principais)
- [Contribuindo](#contribuindo)
- [Licença](#licença)

---

## 💡 Sobre o Projeto

Este projeto é um **aplicativo de console em C#** que:

- Lê um arquivo de texto (`dados.txt`) contendo linhas de comandos SQL `INSERT INTO` nas tabelas antigas (`tb_igrejas`, `tb_membros`, `tb_cargofuncoes`, etc.).
- Processa cada linha, aplica regras de negócio para mapear campos (ex.: converter status, extrair datas, filtrar igreja com ID específico).
- Usa listas auxiliares (`Igrejas`, `IdMembros`) para manter relacionamentos e gerar novos IDs sequenciais.
- Escreve num arquivo de saída (`resultado`) os novos comandos `INSERT` já formatados para o esquema de destino.

Use este utilitário para realizar migrações ou transformações de scripts SQL legados.

---

## 🚀 Tecnologias

- .NET 8.0 (SDK `net8.0`)
- C# 11
- Regex (System.Text.RegularExpressions)
- IO (System.IO)

---

## ✅ Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows, Linux ou macOS com linha de comando

---

## 🛠️ Instalação

```bash
# 1. Clone o repositório
git clone https://github.com/pepes1234/Alteracao-SQL.git
cd Alteracao-SQL
```

---

## ⚙️ Configuração

No arquivo `Program.cs`, ajuste os caminhos de entrada e saída conforme seu ambiente:

```csharp
string inputFilePath  = @"C:\caminho\para\dados.txt";
string outputFilePath = @"C:\caminho\para\resultado";
```

- **`inputFilePath`**: arquivo contendo comandos SQL legados.
- **`outputFilePath`**: destino onde o novo script será gravado.

---

## ▶️ Uso

Compile e execute o projeto:

```bash
# No diretório raiz do projeto
dotnet build
dotnet run --project "Alteracao SQL.csproj"
```

Ao final, verá no console a mensagem:

```
Arquivo gerado com sucesso!
```

E o arquivo de saída `resultado` conterá os comandos `INSERT` atualizados.

---

## 🗂️ Estrutura do Projeto

```plain
Alteracao-SQL/
├── Alteracao SQL.sln      # Solução do Visual Studio
├── Alteracao SQL.csproj   # Projeto .NET 8.0 Console
├── Program.cs             # Lógica principal de leitura e gravação
├── IdMembros.cs           # Classe de mapeamento de membro antigo → novo ID
├── Igrejas.cs             # Classe de mapeamento de igreja antigo → novo ID
├── dados.txt              # Arquivo de entrada (scripts SQL antigos)
└── resultado              # Arquivo de saída gerado (novos INSERTs)
```

---

## 🔑 Classes Principais

- **`Program`**: contém o método `Main`, loops de leitura das linhas e geração dos comandos.
- **`Igrejas`**: armazena `Id` (string original) e `TrueId` (ID sequencial no novo banco).
- **`IdMembros`**: armazena `TrueId` do membro, `Id` original e `idIgreja` para filtrar por igreja.

---

## 🤝 Contribuindo

1. Faça um **fork** deste repositório
2. Crie uma branch para sua feature:  
   ```bash
git checkout -b feature/minha-feature
```  
3. Commit suas alterações:  
   ```bash
git commit -m "feat: descrição da feature"
```  
4. Push para seu fork:  
   ```bash
git push origin feature/minha-feature
```  
5. Abra um **Pull Request**

---
