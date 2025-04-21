## Contribuindo para o Projeto

Este projeto segue uma estrutura de branches baseada no Git Flow para garantir uma organização clara e eficiente no desenvolvimento. Por favor, siga estas orientações ao contribuir.

---

### Branches e seus Propósitos

#### `main`
- **Descrição**: Contém o código de produção. Sempre estável.
- **Commits Permitidos**: Apenas *merges* vindos de `release/*` ou `hotfix/*`.
- **Importante**: Cada commit deve representar uma versão estável e publicada.

#### `develop`
- **Descrição**: Código em desenvolvimento contínuo. Serve como base para novas features.
- **Commits Permitidos**: Apenas *merges* de `feature/*`, `bugfix/*` ou `release/*`.
- **Importante**: Não fazer commits diretos.

#### `feature/*`
- **Descrição**: Desenvolvimento de novas funcionalidades.
- **Base**: `develop`
- **Destino do Merge**: `develop`
- **Exemplo de nome**: `feature/login-social`

#### `bugfix/*`
- **Descrição**: Correção de bugs identificados durante o desenvolvimento.
- **Base**: `develop`
- **Destino do Merge**: `develop`
- **Exemplo de nome**: `bugfix/erro-login`

#### `release/*`
- **Descrição**: Preparação para lançar uma nova versão estável.
- **Base**: `develop`
- **Destino do Merge**: `main` e `develop`
- **Exemplo de nome**: `release/v1.2.0`
- **Notas**: Usado para ajustes finais antes da publicação.

#### `hotfix/*`
- **Descrição**: Correções críticas diretamente em produção.
- **Base**: `main`
- **Destino do Merge**: `main` e `develop`
- **Exemplo de nome**: `hotfix/corrige-crash-pagamento`

---

### Regras Gerais
- Faça *pull requests* em vez de commits diretos.
- Escreva mensagens de commit claras e descritivas.
- Adicione testes se aplicável.
- Atualize a documentação quando necessário.

---

## 🔭 Convenção de Nomes de Branches

Para garantir **compatibilidade máxima com ferramentas, CI/CD e sistemas operativos**, usamos **hífens (`-`) em vez de barras (`/`)** nos nomes dos branches.

---

### 📁 Estrutura de branches

| Tipo de Branch                        | Prefixo      | Exemplo                     | Criado a partir de... |
|--------------------------------------|--------------|-----------------------------|------------------------|
| **Desenvolvimento principal**        | `develop`    | `develop`                   | `main`                 |
| **Funcionalidade nova**              | `feature-`   | `feature-login-page`        | `develop`              |
| **Correção de bug**                    | `bugfix-`    | `bugfix-session-timeout`    | `develop`              |
| **Preparação de release**              | `release-`   | `release-v1.2.0`            | `develop`              |
| **Correção urgente em produção**        | `hotfix-`    | `hotfix-v1.2.1`             | `main`                 |

---

### ✅ Regras gerais

1. **Usar apenas letras minúsculas, números e hífens** (`-`)
2. **Evitar espaços, acentos ou símbolos especiais**
3. **Nome descritivo e objetivo**
4. **Incluir versão no nome, quando aplicável** (`release-v1.0.0`)
5. **Evitar nomes muito longos** (limite recomendado: 30–40 caracteres)

---

### 🚫 Exemplo de nomes inválidos

| Nome                  | Problema                 |
|-----------------------|--------------------------|
| `feature/LoginPage`   | Contém barra `/`         |
| `feature login page`  | Contém espaços           |
| `feature-LoginPage`   | Uso de maiúsculas         |
| `fix@bug`             | Símbolos não suportados   |

---

### 📌 Exemplo de uso

```bash
# Criar uma nova funcionalidade
git checkout -b feature-user-authentication

# Iniciar uma nova release
git checkout -b release-v1.3.0

# Corrigir bug crítico
git checkout -b hotfix-v1.3.1
```

---

### Dúvidas ou Sugestões?
Abra uma *issue* ou entre em contacto com os mantenedores.

Obrigado por contribuir!
