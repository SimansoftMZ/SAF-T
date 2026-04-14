# CLAUDE.md

Este ficheiro fornece orientação ao Claude Code (claude.ai/code) quando trabalha com o código neste repositório.

## Sobre o Projecto

Biblioteca .NET 9+ open-source para geração de ficheiros SAF-T (Standard Audit File for Tax) com arquitectura extensível multi-país. Publicada como pacotes NuGet (`Simansoft.SAFT.*`). Actualmente implementada para Moçambique, com suporte a XML, JSON e hashes SHA-1.

## Comandos

```bash
# Compilar
dotnet build

# Testes (ainda em implementação)
dotnet test
```

## Arquitectura

```
src/
├── SAF-T.Core/         # Contratos e modelos base (independente de país)
├── SAF-T.Cryptography/ # Geração de hashes SHA-1 para assinatura de documentos
└── SAF-T.Mozambique/   # Implementação específica para Moçambique
samples/
└── SAF-T.Examples/     # Exemplos de utilização
```

- **`SAF-T.Core`** — Define as interfaces e modelos partilhados. Para adicionar suporte a um novo país, cria-se um novo projecto que implementa os contratos deste núcleo sem o modificar.
- **`SAF-T.Cryptography`** — Independente do país; fornece a lógica de hash usada para assinar documentos de facturação.
- **`SAF-T.Mozambique`** — Implementação concreta para as regras fiscais moçambicanas.

## Estratégia de Branches

Git Flow — usa hífens em vez de barras nos nomes de branches (compatibilidade com CI/CD):

```
main       ← produção estável; merge apenas de release-* ou hotfix-*
develop    ← integração contínua; merge apenas de feature-*, bugfix-* ou release-*
feature-*  ← novas funcionalidades (base: develop, destino: develop)
bugfix-*   ← correcções em desenvolvimento (base: develop, destino: develop)
release-*  ← preparação de versão (base: develop, destino: main + develop)
hotfix-*   ← correcções críticas em produção (base: main, destino: main + develop)
```
