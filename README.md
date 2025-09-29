# Wanderlust Memoir API

Uma API RESTful para gerenciar destinos de viagem e lugares visitados, construída com .NET 8 e arquitetura limpa.

## ??? Arquitetura

O projeto segue os princípios da **Arquitetura Limpa** (Clean Architecture), organizado em camadas bem definidas:

### Projetos

- **WanderlustMemoir.Domain** - Entidades de domínio, interfaces de repositório e regras de negócio
- **WanderlustMemoir.Application** - Casos de uso, DTOs, serviços de aplicação, handlers e validações
- **WanderlustMemoir.Infrastructure** - Implementação de repositórios, contexto de banco de dados e integrações externas
- **WanderlustMemoir.API** - Controllers, configuração da API e ponto de entrada da aplicação

### Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM com InMemory Database para desenvolvimento
- **MediatR** - Padrão Mediator para CQRS
- **AutoMapper** - Mapeamento entre objetos
- **FluentValidation** - Validação de dados
- **Swagger/OpenAPI** - Documentação da API

## ?? Como Executar

### Pré-requisitos

- .NET 8 SDK
- Visual Studio 2022 ou VS Code

### Passos

1. Clone o repositório:
```bash
git clone https://github.com/Saulohan/wanderlust-memoir-api.git
cd wanderlust-memoir-api
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Execute a aplicação:
```bash
dotnet run --project src/WanderlustMemoir.API
```

4. Acesse a documentação da API:
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
- ? Validação de dados com FluentValidation
- ? Padrão Repository
- ? Padrão Service Layer
- ? CQRS com MediatR
- ? AutoMapper para mapeamento de DTOs
- ? Documentação automática com Swagger
- ? Arquitetura limpa e bem estruturada

## ?? Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -am 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## ?? Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.