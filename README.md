# 🚀 Template para Criação de API's REST em .NET 8

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet\&logoColor=white)](https://dotnet.microsoft.com/)
[![Build](https://github.com/asafeCode/Api-Template/actions/workflows/BuildWithSonarCloud.yml/badge.svg)](https://github.com//asafeCode/Api-Template/actions)
[![Docker](https://img.shields.io/badge/docker-ready-blue?logo=docker\&logoColor=white)](https://www.docker.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](#license)

---

## 📂 Estrutura da Solução

```
TemplateSolution
├── src
│   ├── Backend
│   │   ├── Template.API            # Camada de apresentação (Controllers, Middlewares, Swagger)
│   │   ├── Template.Application    # Casos de uso, validações, DTOs
│   │   ├── Template.Domain         # Entidades, agregados, regras de negócio
│   │   └── Template.Infrastructure # Serviços externos e persistência (sem migrations)
│   │
│   └── Shared
│       ├── Template.Communication  # Contratos de entrada/saída (DTOs, Responses, Requests)
│       └── Template.Exceptions     # Tratamento e padronização de erros
│
└── tests
    ├── CommonTestUtilities         # Utilitários para os testes
    ├── UseCases.Test               # Testes de Unidade
    ├── Validators.Test             # Testes de validações
    └── WebApi.Test                 # Testes de Integração
             
```

---

## 🛠️ O que já vem pronto

* **.NET 8** como framework base.
* **Arquitetura modular** inspirada em DDD.
* **Princípios SOLID** aplicados.
* **FluentValidation** para validação de dados.
* **Tratamento de erros centralizado** (exceptions + middlewares).
* **Injeção de dependências (DI)** configurada.
* **Swagger/OpenAPI** para documentação automática.
* **Testes automatizados**: unitários e de integração.
* **Pipeline CI/CD configurado** (build, testes e análise automática).
* **Dockerfile pronto** para containerização da aplicação.
* **Integração com SonarCloud** para análise contínua de qualidade do código.

---

## ⚡ Como Rodar Localmente

### Requisitos

* [.NET SDK 8.0+](https://dotnet.microsoft.com/)
* [Docker](https://www.docker.com/)

### Rodando com .NET CLI

```bash
git clone https://github.com/SEU_USUARIO/SEU_REPO.git
cd src/Backend/Template.API
dotnet run
```

👉 Acesse o Swagger: [https://localhost:5000/swagger](https://localhost:5000/swagger)

---

## 🐳 Rodando com Docker

```bash
docker build -t template-api .
docker run -d -p 5000:8080 --name template-api template-api
```
---

## 📖 Como Usar o Template

* Adicione seus **casos de uso em Application**.
* Defina suas **entidades e regras de negócio em Domain**.
* Configure integrações externas em **Infrastructure**.
* Exponha endpoints via **API (Controllers)**.

---

## 📜 License

Este template é de uso livre para estudos e projetos.
Adapte conforme suas necessidades!

---
