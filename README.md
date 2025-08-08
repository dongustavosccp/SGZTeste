# Supplier Delivery API & Auth API

## 1. Supplier Delivery API

API responsável pelo gerenciamento de entregas realizadas por fornecedores.

### Funcionalidades implementadas

- **Cadastro de Entregas:** Permite registrar uma nova entrega, informando fornecedor, data, status, endereço e produtos.
- **Consulta de Entregas:** Permite listar entregas filtrando por fornecedor, período ou buscar por ID.
- **Detalhamento Completo:** Cada entrega retorna todos os dados relacionados, incluindo fornecedor, produtos (com quantidade), endereço e status.
- **Cadastro e Consulta de Produtos e Fornecedores:** Endpoints para incluir e consultar fornecedores e produtos, facilitando o vínculo nas entregas.
- **Estrutura organizada:** Separação entre camadas de domínio, aplicação, infraestrutura e DTOs.

---

## 2. Auth API

API dedicada à autenticação de usuários, fornecendo tokens JWT para acesso seguro às rotas protegidas das demais APIs.

### Funcionalidades implementadas

- **Cadastro de Usuário (Register):** Permite criar novos usuários autenticáveis.
- **Login:** Gera um token JWT válido ao autenticar credenciais corretas.
- **Proteção das rotas:** Endpoints das APIs de entrega, fornecedores e produtos exigem autenticação JWT.
- **Validação e Middleware:** O token é validado em todas as requisições protegidas, garantindo acesso apenas a usuários autenticados.

---

## 3. Biblioteca Utils (Class Library)

Implementei também uma **class library chamada Utils** para concentrar funcionalidades, modelos e utilitários comuns entre os projetos, promovendo reutilização e padronização do código.

**Principais itens compartilhados:**
- Notificações de erro
- Mensagens padrão
- Validações
- Modelos auxiliares usados por ambas as APIs

---

## Observações técnicas

- Ambas as APIs utilizam Entity Framework Core para persistência e modelagem dos dados.
- A separação em projetos distintos garante organização, escalabilidade e facilidade de manutenção.
- O uso de JWT proporciona segurança e controle de acesso às funcionalidades sensíveis do sistema.
- As APIs foram desenvolvidas seguindo boas práticas REST e de arquitetura em .NET.
- A class library Utils favorece o reuso e a manutenção centralizada dos componentes comuns.

---

Desenvolvido por Gustavo Ibraim.