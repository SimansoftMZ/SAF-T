# Política de Segurança

Obrigado por ajudar a manter o ecossistema **SimansoftMZ/SAF-T** seguro. Esta política explica **como reportar vulnerabilidades**, **quais versões recebem correções**, e **como conduzimos a divulgação responsável**.

---

## 🛡️ Onde reportar vulnerabilidades

> **Use de preferência o canal privado do GitHub**
- Abra um **Private Vulnerability Report** (PVR) neste repositório:
  - Vá a **Security → Report a vulnerability** e descreva o problema com detalhes (passos de reprodução, impacto, versão afetada, PoC, ambiente).
- Se não conseguir usar o PVR, contacte por e-mail:
  - **assunto**: `Vulnerability Report – SAF-T`
  - **conteúdo**: descrição, impacto, versões afetadas, logs/anexos, PoC (se existir)
  - **endereço**: `devsecurity@simansoft.co.mz`

> **Não** abra *issues* públicas nem *pull requests* com detalhes sensíveis. Evite enviar dados pessoais ou credenciais.

---

## 🧭 Escopo

O que está **em escopo**:
- Código deste repositório `SimansoftMZ/SAF-T` e pacotes/artefatos publicados a partir dele.
- Defeitos que possam causar: execução de código, elevação de privilégios, bypass de validações fiscais/criptográficas, fuga de informação sensível, corrupção de ficheiros SAF-T, DoS significativo.

O que está **fora de escopo** (exemplos):
- Vulnerabilidades de dependências **já corrigidas** em versões suportadas (actualize antes).
- Problemas de configuração do utilizador/ambiente (ex.: permissões incorretas em servidores, certificados inválidos, chaves privadas expostas fora do projeto).
- Ataques de *social engineering*, *phishing*, ou problemas de terceiros/fornecedores.
- DoS que exija condições irrealistas de rede/recursos.

---

## 📦 Versões suportadas

Recebem *patches* de segurança as **branches e versões ativas**:

| Linha/Branch              | Estado           | Recebe correções? |
|---------------------------|------------------|-------------------|
| `main`                    | Suportada        | ✅                |
| Última release estável    | Suportada        | ✅                |
| Release anterior (N-1)    | Manutenção       | ✅ Correções críticas apenas |
| Versões mais antigas      | EOL              | ❌                |

> Regra geral: mantemos **até duas** linhas estáveis. Se estiver numa versão EOL, **actualize** para continuar a receber patches.

---

## ⏱️ SLA de resposta

- **Confirmação de receção**: até **72 horas** úteis.
- **Classificação/Triagem inicial**: até **5 dias úteis** (pode incluir pedido de mais detalhes).
- **Plano de correção**: comunicado após triage; prazos variam com a gravidade.
- **Divulgação coordenada**: por norma em **≤ 90 dias** após confirmação, podendo ser ajustado conforme impacto/complexidade.

---

## 🧮 Como priorizamos (CVSS)

Usamos uma avaliação baseada em **CVSS** e contexto fiscal/operacional.

- **Crítica (9.0–10.0)**: RCE, quebra de assinatura/validação criptográfica SAF-T, violação grave de integridade/confidencialidade → correção expedita e possível *out-of-band release*.
- **Alta (7.0–8.9)**: bypass de validações, fuga de dados sensíveis, DoS consistente.
- **Média (4.0–6.9)**: falhas com exploração limitada ou mitigáveis por configuração.
- **Baixa (0.1–3.9)**: impacto mínimo ou requisitos de exploração pouco realistas.

---

## ✅ Recomendações ao reportar

Inclua sempre:
1. **Descrição clara** e impacto.
2. **Versões/commits** afetados e ambiente (.NET, SO, etc.).
3. **Passos de reprodução** e PoC minimal.
4. **Logs**/*stack traces* relevantes.
5. Mitigações temporárias (se souber).

> Envie PoCs de forma **não destrutiva** e sem dados reais de contribuintes/empresas. Quando necessário, anonimize.

---

## 🔐 Boas práticas para utilizadores

- **Mantenha-se actualizado** (patches de segurança, Dependabot, releases).
- Proteja **chaves e certificados**; nunca committe credenciais.
- Valide **entradas** antes de gerar ficheiros SAF-T.
- Utilize **ambientes segregados** para desenvolvimento/testes.
- Active **verificação de origem** dos pacotes NuGet.

---

## 🤝 Divulgação responsável

A equipa aprecia e **reconhece contribuições** de segurança após correção e release (Hall of Fame nas *release notes*). Se desejar **crédito público**, indique o nome/link preferido no reporte.

Pedimos que **não** publique detalhes técnicos antes de:
- existir correção disponibilizada e
- decorrer o período acordado de divulgação coordenada.

---

## 📄 Safe Harbor

Desde que actue de boa fé e dentro deste escopo:
- Não iniciaremos ações legais por testes destinados a melhorar a segurança.
- Não accione testes que degradem serviços/infraestrutura ou acedam dados de terceiros.
- Interrompa imediatamente se encontrar dados reais e reporte **sem os reter/copiar**.

---

## 📜 Alterações a esta política

Podemos atualizar esta política. Verifique sempre a versão mais recente no ficheiro `SECURITY.md` da branch `main`.

Obrigado por ajudar a manter o **SAF-T** seguro para todos.
