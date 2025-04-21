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

### Dúvidas ou Sugestões?
Abra uma *issue* ou entre em contacto com os mantenedores.

Obrigado por contribuir!
