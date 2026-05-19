# ⚽ Campeonato de Futebol API

API backend para gerenciamento de campeonatos de futebol, desenvolvida com .NET 8, ASP.NET Core Web API, Entity Framework Core e Oracle Database.

---

## 👥 Integrantes

| Nome | RM |
|------|----|
| Marchel Augusto | RM99856 |
| David Cordeiro | RM 557538 |

---

## 📋 Sobre o Sistema

Esta API permite gerenciar de forma completa um campeonato de futebol:

- **Times** — cadastro e gerenciamento com validações
- **Jogadores e Técnicos** — herança polimórfica de `Pessoa`
- **Campeonatos** — com sistema de pontos corridos
- **Geração automática de confrontos** — todos contra todos (ida e volta)
- **Registro de partidas** — com atualização automática de estatísticas
- **Classificação em tempo real** — pontos, vitórias, saldo de gols
- **Easter Egg** 🥚 — tente cadastrar o Palmeiras...

---

## 🏗️ Diagrama de Classes

```
Pessoa (abstrata - TPH)
├── Jogador
│   ├── Posicao
│   ├── NumeroCamisa
│   └── TimeId → Time
└── Tecnico
    ├── Especialidade
    └── TimeId → Time

Campeonato (TPH base)
└── CampeonatoPontosCorridos
    ├── TotalRodadas
    └── ConfrontosGerados

Time ──[N:N via TimeCampeonato]──► Campeonato
Time ──[1:N]──► Trofeu
Campeonato ──[1:N]──► Partida
Campeonato ──[1:N]──► Estatistica
Time ──[1:N]──► Estatistica
```

---

## 🚀 Como Rodar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Acesso ao Oracle Database (FIAP ou local)
- Ferramenta EF Core CLI

### Instalar EF Core CLI

```bash
dotnet tool install --global dotnet-ef
```

### Restaurar dependências

```bash
cd Fiap.Banco.API
dotnet restore
```

### Rodar a API

```bash
dotnet run
```

A API estará disponível em:
- `http://localhost:5000`
- `https://localhost:7000`
- Swagger UI: `http://localhost:5000/swagger`

---

## 🗄️ Configurar Oracle

Edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_RM; Password=SUA_SENHA; Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

Para Oracle local:

```json
"OracleConnection": "User Id=usuario; Password=senha; Data Source=localhost:1521/XEPDB1"
```

---

## 📦 Como Aplicar Migrations

```bash
# Dentro da pasta Fiap.Banco.API
cd Fiap.Banco.API

# Criar a migration inicial
dotnet ef migrations add Initial

# Aplicar ao banco
dotnet ef database update
```

> ⚠️ As migrations antigas do projeto banco foram removidas. Sempre crie uma nova migration `Initial` neste projeto.

---

## 🔌 Endpoints Disponíveis

### Times
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/times` | Lista todos os times |
| `GET` | `/api/times/{id}` | Busca time por ID |
| `POST` | `/api/times` | Cadastra novo time |
| `PUT` | `/api/times/{id}` | Atualiza time |
| `DELETE` | `/api/times/{id}` | Remove time |

### Jogadores
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/jogadores` | Lista todos os jogadores |
| `GET` | `/api/jogadores/{id}` | Busca jogador por ID |
| `POST` | `/api/jogadores` | Cadastra jogador |
| `PUT` | `/api/jogadores/{id}` | Transfere jogador de time |

### Técnicos
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/tecnicos` | Lista todos os técnicos |
| `GET` | `/api/tecnicos/{id}` | Busca técnico por ID |
| `POST` | `/api/tecnicos` | Cadastra técnico |

### Campeonatos
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/campeonatos` | Lista todos os campeonatos |
| `GET` | `/api/campeonatos/{id}` | Busca campeonato por ID |
| `POST` | `/api/campeonatos` | Cria campeonato (pontos corridos) |
| `POST` | `/api/campeonatos/{id}/times/{timeId}` | Inscreve time no campeonato |
| `POST` | `/api/campeonatos/{id}/gerarconfrontos` | Gera todas as partidas automaticamente |
| `GET` | `/api/campeonatos/{id}/classificacao` | Retorna tabela de classificação |

### Partidas
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/partidas` | Lista todas as partidas |
| `GET` | `/api/partidas/{id}` | Busca partida por ID |
| `POST` | `/api/partidas` | Registra partida com resultado |
| `PUT` | `/api/partidas/{id}/resultado` | Atualiza placar de partida gerada |

### Estatísticas
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/estatisticas` | Lista todas as estatísticas |
| `GET` | `/api/estatisticas/campeonato/{campeonatoId}` | Classificação por campeonato |

---

## 📝 Exemplos JSON

### Criar Time
```http
POST /api/times
Content-Type: application/json

{
  "nome": "Corinthians",
  "cidade": "São Paulo",
  "anoFundacao": 1910
}
```

**Response 201:**
```json
{
  "id": 1,
  "nome": "Corinthians",
  "cidade": "São Paulo",
  "anoFundacao": 1910
}
```

---

### 🥚 Easter Egg — Palmeiras sem mundial - Invenção do David, o Marchel não tem nada a ver com isso
```http
POST /api/times
Content-Type: application/json

{
  "nome": "Palmeiras",
  "cidade": "São Paulo",
  "anoFundacao": 1914
}
```

**Response 400:**
```json
{
  "erro": "Proibido times sem mundial!!!"
}
```
> Funciona com qualquer capitalização: `palmeiras`, `PALMEIRAS`, `PaLmEiRaS`

---

### Criar Campeonato
```http
POST /api/campeonatos
Content-Type: application/json

{
  "nome": "Brasileirão 2025",
  "temporada": "2025",
  "dataInicio": "2025-04-01T00:00:00",
  "dataFim": "2025-12-07T00:00:00"
}
```

**Response 201:**
```json
{
  "id": 1,
  "nome": "Brasileirão 2025",
  "temporada": "2025",
  "dataInicio": "2025-04-01T00:00:00",
  "dataFim": "2025-12-07T00:00:00",
  "totalRodadas": 0,
  "confrontosGerados": false
}
```

---

### Inscrever Time no Campeonato
```http
POST /api/campeonatos/1/times/1
```

**Response 200:**
```json
{
  "mensagem": "Time 'Corinthians' adicionado ao campeonato 'Brasileirão 2025'."
}
```

---

### Gerar Confrontos
```http
POST /api/campeonatos/1/gerarconfrontos
```

**Response 200:**
```json
{
  "mensagem": "Confrontos gerados com sucesso!",
  "totalPartidas": 6,
  "times": ["Corinthians", "Flamengo", "Grêmio"]
}
```

---

### Registrar Resultado de Partida
```http
PUT /api/partidas/1/resultado
Content-Type: application/json

{
  "golsCasa": 2,
  "golsVisitante": 1
}
```

**Response 200:**
```json
{
  "mensagem": "Resultado registrado e classificação atualizada.",
  "golsCasa": 2,
  "golsVisitante": 1
}
```

---

### Classificação do Campeonato
```http
GET /api/campeonatos/1/classificacao
```

**Response 200:**
```json
[
  {
    "time": "Corinthians",
    "pontos": 3,
    "vitorias": 1,
    "empates": 0,
    "derrotas": 0,
    "golsPro": 2,
    "golsContra": 1,
    "saldoGols": 1
  },
  {
    "time": "Flamengo",
    "pontos": 0,
    "vitorias": 0,
    "empates": 0,
    "derrotas": 1,
    "golsPro": 1,
    "golsContra": 2,
    "saldoGols": -1
  }
]
```

---

### Criar Jogador
```http
POST /api/jogadores
Content-Type: application/json

{
  "nome": "Romero",
  "dataNascimento": "1988-11-16T00:00:00",
  "posicao": "Atacante",
  "numeroCamisa": 11,
  "timeId": 1
}
```

---

### Criar Técnico
```http
POST /api/tecnicos
Content-Type: application/json

{
  "nome": "Ramón Díaz",
  "dataNascimento": "1959-08-29T00:00:00",
  "especialidade": "Futebol Ofensivo",
  "timeId": 1
}
```

---

## 🖥️ Prints Simulados

```
[Swagger UI]
────────────────────────────────────────
Campeonato de Futebol API  v1
────────────────────────────────────────
▼ Times
  POST   /api/times
  GET    /api/times
  GET    /api/times/{id}
  PUT    /api/times/{id}
  DELETE /api/times/{id}

▼ Jogadores
  POST   /api/jogadores
  GET    /api/jogadores

▼ Técnicos
  POST   /api/tecnicos
  GET    /api/tecnicos

▼ Campeonatos
  POST   /api/campeonatos
  GET    /api/campeonatos
  POST   /api/campeonatos/{id}/times/{timeId}
  POST   /api/campeonatos/{id}/gerarconfrontos
  GET    /api/campeonatos/{id}/classificacao

▼ Partidas
  POST   /api/partidas
  GET    /api/partidas
  PUT    /api/partidas/{id}/resultado

▼ Estatísticas
  GET    /api/estatisticas
  GET    /api/estatisticas/campeonato/{campeonatoId}
────────────────────────────────────────
```

---

## 🏆 Regras de Negócio

- Vitória = 3 pontos | Empate = 1 ponto | Derrota = 0 pontos
- Um time não pode jogar contra si mesmo
- Confrontos gerados uma única vez por campeonato
- Jogadores podem ser transferidos entre times (PUT /api/jogadores/{id})
- Estatísticas são atualizadas automaticamente ao registrar resultado

---

## 🛠️ Tecnologias

- **.NET 8** — runtime e SDK
- **ASP.NET Core Web API** — framework HTTP
- **Entity Framework Core 9** — ORM
- **Oracle.EntityFrameworkCore 9** — provider Oracle
- **Swashbuckle / Swagger** — documentação da API
- **TPH (Table Per Hierarchy)** — herança `Pessoa` e `Campeonato`
