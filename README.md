# SAF-T 🌍

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

### Builds
[![SAF-T Base CI/CD](https://github.com/SimansoftMZ/SAF-T/actions/workflows/saft-base-ci-cd.yml/badge.svg)](https://github.com/SimansoftMZ/SAF-T/actions/workflows/saft-base-ci-cd.yml)

## Descrição do Projeto 📄

**SAF-T** é uma biblioteca .NET para geração de ficheiros **SAF-T (Standard Audit File for Tax)**, focada em **extensibilidade multi-país**. Desenvolvida em **C#/.NET 10+**, permite que desenvolvedores e empresas gerem documentos fiscais em conformidade com as regras locais de países como Moçambique, com suporte a **XML**, **JSON** e **hashes SHA-1**. Projeto open-source sob licença MIT.

### Tecnologias:
- **.NET 10**: Performance e modernidade.
- **XML/JSON**: Suporte aos formatos exigidos por autoridades fiscais.
- **SHA-1**: Segurança ao gerar de hashes.

### Objetivos:
- Facilitar a adesão às normas fiscais de cada país.
- Reduzir a complexidade na integração com sistemas ERP.
- Ser modular: adicione novos países sem modificar o núcleo do projeto.

### Público-Alvo:
- Desenvolvedores de software empresarial.
- Empresas com operações multi-país.
- Consultores fiscais e contabilísticos.

---

## Funcionalidades Principais ✨
- [X] **Suporte Multi-País**: Implemente SAF-T para Moçambique, Angola, Portugal, ou qualquer outro país.
- [X] **Formatos de Saída**: Gere ficheiros em **XML**, **JSON**, **XLSX** e **XLS** conforme as especificações locais.
- [X] **Extensível**: Adicione novos países com estrutura modular.
- [X] **Hash**: Gera hashes seguros para assinar documentos de faturação.
- [ ] **Testes automatizados**: Implementação de testes automatizados
- [ ] **Validação Integrada**: Valide dados contra esquemas XSD/JSON Schema.

---

## Instalação 🚀

```bash
# Instale o pacote principal
dotnet add package Simansoft.SAFT.Core

# Instale o pacote de criptografia dos dados
dotnet add package Simansoft.SAFT.Cryptography

# Instale a implementação para Moçambique
dotnet add package Simansoft.SAFT.Mozambique
