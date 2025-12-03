
# 🏗️ Task API

[![.NET](https://img.shields.io/badge/.NET-8-blue?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)  
[![C#](https://img.shields.io/badge/C%23-9.0-blue?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)  
[![Entity Framework](https://img.shields.io/badge/Entity_Framework-Core-2C3E50?style=for-the-badge&logo=entity-framework&logoColor=white)](https://learn.microsoft.com/ef/)  
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=white)](https://swagger.io/)  
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)  
  

## ✨ Sobre

**Task API** é sua assistente flexível para gerenciar tarefas de forma rápida e eficiente. Ela entende sua rotina, permitindo criar, atualizar, consultar e remover tarefas sem complicação. Ideal para automatizar fluxos e nunca perder um prazo!  

## 🛠️ Funcionalidades

- CRUD completo de tarefas  
- Filtros e busca rápida  
- Status e prioridades personalizáveis  
- Estrutura pronta para testes automatizados  
- Documentação via Swagger/OpenAPI  
- Fácil de rodar localmente ou em Docker  

## 🚀 Endpoints principais

- `GET /tasks` – Lista todas as tarefas  
- `GET /tasks/{id}` – Consulta uma tarefa específica  
- `POST /tasks` – Cria uma nova tarefa  
- `PUT /tasks/{id}` – Atualiza uma tarefa existente  
- `DELETE /tasks/{id}` – Remove uma tarefa  

## ⚡ Como rodar

### Usando .NET CLI
```bash
git clone https://github.com/asafeCode/Api-Tarefas.git
cd src/Backend/Template.API
dotnet run


Abra no navegador: `http://localhost:5000/swagger`

### Usando Docker

```bash
docker build -t task-api .
docker run -d -p 5000:8080 --name task-api task-api
```
### 📄 Licença
- Projeto open source — use, adapte e aproveite à vontade!
