# Política de Segurança

Obrigado por ajudar a manter o ecossistema **SimansoftMZ/SAF-T** seguro. Esta política explica **como reportar vulnerabilidades**, **quais versões recebem correções**, e **como conduzimos a divulgação responsável**.

---

## Índice

* [Como abrir um PVR (Private Vulnerability Report) no GitHub](#como-abrir-um-pvr-private-vulnerability-report-no-github)
* [Checklist rápido para PVR](#checklist-rápido-para-pvr)
* [Se não conseguir usar o PVR](#se-não-conseguir-usar-o-pvr)
* [Como enviar PoC de forma segura](#como-enviar-poc-de-forma-segura)
* [Escopo](#escopo)
* [Versões suportadas / EOL / Backport](#versões-suportadas--eol--backport)
* [SLA de resposta e triagem](#sla-de-resposta-e-triagem)
* [Como priorizamos (CVSS)](#como-priorizamos-cvss)
* [Recomendações ao reportar](#recomendações-ao-reportar)
* [Boas práticas para utilizadores](#boas-práticas-para-utilizadores)
* [Divulgação responsável e CVE](#divulgação-responsável-e-cve)
* [Safe Harbor](#safe-harbor)
* [Retenção e confidencialidade](#retenção-e-confidencialidade)
* [Alterações a esta política](#alterações-a-esta-política)

---

## Como abrir um PVR (Private Vulnerability Report) no GitHub

Siga estes passos para criar um relatório privado (PVR) no GitHub. Este é o caminho recomendado porque mantém o reporte privado entre o autor e os mantenedores até existir correção.

1. Abra o repositório no GitHub: `https://github.com/SimansoftMZ/SAF-T`.
2. No topo da página do repositório clique em **Security**.

   * Se não vir **Security**, verifique o menu **More** (⋯).
3. Na área lateral de **Security** clique em **Advisories**.
4. Clique em **New draft security advisory**.
5. Preencha o formulário com:

   * **Title** (título curto e descritivo).
   * **Description** (impacto, passos para reproduzir, versões/commits afetados).
   * **Affected products/versions** (componentes/versões afetadas).
   * (Opcional) **Severity**, **References**, anexos (PoC, logs).
6. Clique em **Submeter rascunho**. O rascunho fica privado e visível apenas para os administradores do repositório e para o autor com permissões.
7. Após triagem e correção, os mantenedores podem publicar o advisory público e solicitar CVE, se aplicável.

### Caminho alternativo

* Alguns repositórios exibem **Security → Report a vulnerability**. Esse formulário também abre um PVR privado.

---

## Checklist rápido para PVR

* Título curto e impacto (1 linha).
* Versão/commit afetado.
* Passos mínimos para reproduzir.
* PoC ou logs (anexar se seguro).
* Preferência sobre divulgação (manter privado até patch?).
* Contacto do autor (email).

---

## Se não conseguir usar o PVR

Se a funcionalidade de Advisories estiver desativada ou não tiver permissões:

* **Não** publique detalhes sensíveis em *issues* públicas.
* Envie e-mail para: `devsecurity@simansoft.co.mz` com o assunto: `Vulnerability Report – SAF-T`.
* Inclua os mesmos campos do checklist acima.

O e-mail é monitorizado em dias úteis; responda normalmente dentro do SLA indicado abaixo.

---

## Como enviar PoC de forma segura

Para proteger dados sensíveis e facilitar a triagem privada, siga isto:

1. Coloque a PoC num ficheiro `.zip` e proteja com palavra‑passe.
2. Anexe o `.zip` ao advisory privado ou ao e-mail para `devsecurity@simansoft.co.mz`.
3. Envie a palavra‑passe por um canal separado (Signal, SMS, ou por GPG).
4. Se preferir GPG, cifre a PoC com a chave pública da equipa. Fingerprint da chave pública: `TO-ADD-FINGERPRINT-HERE` (será publicado quando disponível).
5. Nunca inclua dados de utentes reais ou credenciais; substitua por dados de teste.

---

## Escopo

**Em escopo:**

* Código deste repositório `SimansoftMZ/SAF-T` e artefactos publicados a partir dele.
* Defeitos que possam causar: execução de código, elevação de privilégios, bypass de validações fiscais/criptográficas, fuga de informação sensível, corrupção de ficheiros SAF‑T, DoS significativo.

**Fora de escopo (exemplos):**

* Vulnerabilidades em dependências já corrigidas em versões suportadas (actualize dependências).
* Problemas de configuração do utilizador/infraestrutura (permissões de servidor, chaves privadas externas).
* Social engineering, phishing, ou problemas de terceiros/fornecedores.
* DoS que exija condições irreais.

---

## Versões suportadas / EOL / Backport

Mantemos correções de segurança conforme a política abaixo:

* `main` — linha ativa, recebe correções.
* Última release estável (ex.: `vX.Y.Z`) — recebe correções.
* Release anterior (N‑1) — recebe correções críticas apenas; suporte limitado por **12 meses** após a data de release da última versão estável.
* Versões mais antigas — EOL; não recebem correções.

**Backport:** patches críticos podem ser retroportados para N‑1 a critério da equipa, dependendo do impacto e custo de manutenção.

---

## SLA de resposta e triagem

* **Confirmação de receção:** até **72 horas úteis**.
* **Triagem inicial:** até **5 dias úteis** (pode incluir pedido de mais detalhes).
* **Plano de correção:** comunicado após triagem; prazos variam conforme gravidade.
* **Divulgação coordenada:** por norma até **90 dias** após confirmação, sujeito a ajuste.

---

## Como priorizamos (CVSS)

Usamos CVSS como referência e consideramos contexto fiscal/operacional.

* **Crítica (9.0–10.0):** RCE, quebra de assinaturas/validações essenciais → correção urgente; possível release fora de calendário.
* **Alta (7.0–8.9):** Bypass de validações, fuga de dados sensíveis, DoS consistente.
* **Média (4.0–6.9):** Falhas exploráveis em condições limitadas.
* **Baixa (0.1–3.9):** Impacto mínimo ou exploração improvável.

---

## Recomendações ao reportar

Inclua sempre:

1. Descrição clara e impacto.
2. Versões/commits afetados e ambiente (.NET, SO, etc.).
3. Passos de reprodução e PoC minimal.
4. Logs/stack traces relevantes.
5. Mitigações temporárias (se souber).

> Envie PoCs de forma não destrutiva e sem dados reais de utentes. Anonimize quando necessário.

---

## Boas práticas para utilizadores

* Atualize dependências e releases regularmente.
* Proteja chaves e certificados; nunca committe credenciais.
* Valide entradas antes de gerar ficheiros SAF‑T.
* Use ambientes segregados para testes.
* Verifique origem de pacotes NuGet antes de aceitar.

---

## Divulgação responsável e CVE

A equipa valoriza contribuições de segurança. Após correção, o maintainer pode publicar advisory público e solicitar CVE (via GitHub/MITRE) se aplicável. O reporte pode optar por crédito público (Hall of Fame) — indicar preferência no reporte.

---

## Safe Harbor

Se agir de boa fé e dentro deste escopo:

* Não iremos tomar ações legais contra testes de segurança conduzidos de boa fé.
* Não autorize testes que degradem serviços, causem perda de dados, ou acedam a dados de terceiros.
* Interrompa imediatamente se encontrar dados reais e reporte-os sem os reter.

---

## Retenção e confidencialidade

* Relatórios privados serão mantidos enquanto necessários para triagem e correção.
* Após resolução, os dados do relatório são retidos por **12 meses** por padrão, salvo pedido expresso de eliminação.
* Pedidos de eliminação devem ser enviados para `devsecurity@simansoft.co.mz`.

---

## Alterações a esta política

Podemos atualizar esta política. Consulte sempre a versão no `SECURITY.md` da branch `main`.

---

## Template curto para colar no Advisory / E-mail

```
Título: [Curto e descritivo]
Versão/Commit afetado: [ex.: v1.2.3 / commit abc123]
Resumo: [1–2 linhas sobre o que acontece]
Passos mínimos para reproduzir:
1. [Passo 1]
2. [Passo 2]
PoC / Logs: [anexar ou indicar que PoC disponível mediante pedido]
Impacto: [ex.: RCE / exfiltração / corrupção de SAF‑T]
Preferência divulgação: [Privada até patch / Publicar após fix]
Contacto: [seu.email@exemplo.tld]
```

---

Obrigado por ajudar a manter o **SAF‑T** seguro para todos.
