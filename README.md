# DesenvolvimentoIA-DBM

## 📌 Objetivo do Projeto
Este projeto consiste em dois sistemas integrados
- Uma API em C# para importar dados de um arquivo CSV para um banco de dados SQL Server e disponibilizar endpoints para consulta.
- Um cliente em Python que consome a API, processa as informações e exibe os dados e categorizados por idade(Jovem, Adulto e Sênior).

## 🛠️ Tecnologias Utilizadas
- **API C#**
1. Visual Studio 2022
2. ASP.NET Core Web API
3. Entity Framework Core
4. SQL Server
5. CsvHelper
6. Swagger

- **Cliente Python**
1. Visual Studio Code
2. Python 3.x
3. Requests
4. Dataclasses

## 🏗️ Como Rodar o Projeto
- **Passos para Executar a API C#**
1. Clone o repositório
   ```sh
   git clone https://github.com/WellingtonRuanDaSilva/DesenvolvimentoIA-DBM.git
   ```
3. Abra o projeto localizado no diretório /api-csharp no Visual Studio
4. Instale os pacotes NuGet necessários:

   Execute os comandos um de cada vez no terminal em: Ferramentas >> Gerenciamento de Pacotes do NuGet >> Console do Gerenciador de Pacotes
   ```sh
   Install-Package Microsoft.EntityFrameworkCore.SqlServer
   Install-Package Microsoft.EntityFrameworkCore.Tools
   Install-Package CsvHelper
   ```
6. Execute as migrações do banco de dados no terminal:
   ```sh
   Add-Migration InitialCreate
   Update-Database
   ```
7. Execute o projeto (F5 no Visual Studio)

- **Passos para Executar o Cliente Python**
1. Abra o projeto localizado no diretório /cliente-python no Visual Studio Code
2. Instale a extensão do Python (se necessário)
3. Instale as dependências no termianl CTRL+J:
   ```sh
   pip install requests
   ```
4. Execute o cliente:
   ```sh
   python client.py
   ```

## 📱 Como Usar

- **Executado API C# - Swagger**
1. Ao executar o API C# será aberto em seu navegador o Swagger.
- Clique em POST /api/Person/import >> Try it out >> Escolher arquivo
- Escolha o arquivo BancoDeDados.csv pressione Abrir e Execute
- Resposta de Sucesso json:
   ```sh
   "Importados 10 registros com sucesso"
   ```
2. Pode buscar as pessoas pelo ID no GET /api/people/{id}
3. Ou listar todoas as pessoas no GET /api/people
- **Executado o Cliente Python**
1. No terminal do Visual Studio Code irá aparecer a mensagem:
   ```sh
   Digite o ID da pessoa (ou 'q' para sair):
   ```
2. Ao Digitar 1 (como exemplo) será exibido:
   ```sh
   === Informações da Pessoa ===
   Categoria: Jovem
   ID: 1
   Nome: Ana Silva
   Idade: 29 anos
   Cidade: São Paulo
   Profissão: Engenheira
   ==========================

   ```
3. Ou digite 'q' para sair da aplicação.

## 🚀 Notas de Desenvolvimento
1.A API utiliza Entity Framework Core com SQL Server para persistência dos dados
2. O cliente Python desabilita a verificação SSL para desenvolvimento local
3. O sistema suporta arquivos CSV com ou sem a coluna ID
4. Todos os campos são validados antes da importação
5. O sistema categoriza as pessoas de acordo com a idade (Jovem, Adulto e Sênior).
6. A API retorna respostas apropriadas para diferentes cenários:
- 200 OK: Operação realizada com sucesso
- 400 Bad Request: Erro na requisição (arquivo inválido, dados incorretos)
- 404 Not Found: Registro não encontrado
- 500 Internal Server Error: Erro interno do servidor
