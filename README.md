# Wanderlust Memoir API

Uma API RESTful para gerenciar destinos de viagem e lugares visitados, constru�da com .NET 8 e arquitetura limpa.

## ??? Arquitetura

O projeto segue os princ�pios da **Arquitetura Limpa** (Clean Architecture), organizado em camadas bem definidas:

### Projetos

- **WanderlustMemoir.Domain** - Entidades de dom�nio, interfaces de reposit�rio e regras de neg�cio
- **WanderlustMemoir.Application** - Casos de uso, DTOs, servi�os de aplica��o, handlers e valida��es
- **WanderlustMemoir.Infrastructure** - Implementa��o de reposit�rios, contexto de banco de dados e integra��es externas
- **WanderlustMemoir.API** - Controllers, configura��o da API e ponto de entrada da aplica��o

### Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM com InMemory Database para desenvolvimento
- **MediatR** - Padr�o Mediator para CQRS
- **AutoMapper** - Mapeamento entre objetos
- **FluentValidation** - Valida��o de dados
- **Swagger/OpenAPI** - Documenta��o da API

## ?? Como Executar

### Pr�-requisitos

- .NET 8 SDK
- Visual Studio 2022 ou VS Code

### Passos

1. Clone o reposit�rio:
```bash
git clone https://github.com/Saulohan/wanderlust-memoir-api.git
cd wanderlust-memoir-api
```

2. Restaure as depend�ncias:
```bash
dotnet restore
```

3. Execute a aplica��o:
```bash
dotnet run --project src/WanderlustMemoir.API
```

4. Acesse a documenta��o da API:
   - Swagger UI: `https://localhost:5001/swagger`

## ?? Endpoints

### Destinations

- `GET /api/destinations` - Lista todos os destinos
- `GET /api/destinations/{id}` - Busca um destino por ID
- `POST /api/destinations` - Cria um novo destino
- `PUT /api/destinations/{id}` - Atualiza um destino
- `DELETE /api/destinations/{id}` - Remove um destino
- `PATCH /api/destinations/{id}/toggle-visited` - Alterna status de visitado

### Visited Places

- `GET /api/visitedplaces` - Lista todos os lugares visitados
- `GET /api/visitedplaces/{id}` - Busca um lugar visitado por ID
- `POST /api/visitedplaces` - Cria um novo lugar visitado
- `PUT /api/visitedplaces/{id}` - Atualiza um lugar visitado
- `DELETE /api/visitedplaces/{id}` - Remove um lugar visitado

## ?? Estrutura do Projeto

```
src/
??? WanderlustMemoir.Domain/
?   ??? Common/
?   ??? Entities/
?   ??? Repositories/
??? WanderlustMemoir.Application/
?   ??? DTOs/
?   ??? Extensions/
?   ??? Features/
?   ??? Interfaces/
?   ??? Mappings/
?   ??? Services/
?   ??? Validators/
??? WanderlustMemoir.Infrastructure/
?   ??? Data/
?   ??? Extensions/
?   ??? Repositories/
??? WanderlustMemoir.API/
    ??? Controllers/
```

## ?? Funcionalidades

- ? CRUD completo para Destinos
- ? CRUD completo para Lugares Visitados
- ? Valida��o de dados com FluentValidation
- ? Padr�o Repository
- ? Padr�o Service Layer
- ? CQRS com MediatR
- ? AutoMapper para mapeamento de DTOs
- ? Documenta��o autom�tica com Swagger
- ? Arquitetura limpa e bem estruturada

## ?? Contribui��o

1. Fa�a um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudan�as (`git commit -am 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## ?? Licen�a

Este projeto est� sob a licen�a MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.