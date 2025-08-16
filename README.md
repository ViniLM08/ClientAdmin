# ClientAdmin
Aplicação ASP.NET Core 8.0 para administração de clientes, utilizando Entity Framework Core e SQLite como banco de dados.

Antes de rodar o projeto, certifique-se de ter instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [SQLite](https://www.sqlite.org/download.html) (opcional, já vem incluído como dependência do EF Core, mas pode ser útil para gerenciar o banco)  
- Visual Studio 2022 (com workload **ASP.NET and web development**) ou [Visual Studio Code](https://code.visualstudio.com/) com extensão C#  

---

Estrutura do projeto

- `Program.cs` → Ponto de entrada da aplicação  
- `appsettings.json` → Configurações de conexão com o banco  
- `ClientAdmin.db` → Banco SQLite local  
- `ClientAdmin.csproj` → Arquivo de projeto .NET  

---

Dependências principais

As dependências são gerenciadas automaticamente pelo .NET. As principais são:  

- `Microsoft.EntityFrameworkCore`  
- `Microsoft.EntityFrameworkCore.Sqlite`  
- `Microsoft.EntityFrameworkCore.Design`  
- `Humanizer`  

Para restaurar as dependências:  

```bash
dotnet restore

Extraia o projeto e acesse a pasta:

cd ClientAdmin


Restaure as dependências:

dotnet restore


(Opcional) Se precisar recriar o banco de dados com migrations:

dotnet ef database update


Execute a aplicação:

dotnet run


Abra no navegador em:
👉 https://localhost:7165/

(Caso haja aviso de link não seguro, clicar em "Avançado" e em "Ir para localhost (não seguro)")
