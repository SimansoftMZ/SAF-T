# SAF-T 🌍

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://github.com/simansoftmz/saft-t/actions/workflows/dotnet.yml/badge.svg)](https://github.com/simansoftmoz/saf-t/actions)

## Descrição do Projeto 📄

**SAF-T** é uma biblioteca .NET para geração de ficheiros **SAF-T (Standard Audit File for Tax)**, focada em **extensibilidade multi-país**. Desenvolvida em **C#/.NET 9+**, permite que desenvolvedores e empresas gerem documentos fiscais em conformidade com as regras locais de países como Moçambique, com suporte a **XML**, **JSON** e **hashes SHA-256**. Projeto open-source sob licença MIT.

### Tecnologias:
- **.NET 9**: Performance e modernidade.
- **XML/JSON**: Suporte aos formatos exigidos por autoridades fiscais.
- **SHA-256**: Segurança na geração de hashes.

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
- ✅ **Suporte Multi-País**: Implemente SAF-T para Moçambique, Portugal, ou qualquer outro país.
- ✅ **Formatos de Saída**: Gere ficheiros em **XML** e **JSON** conforme as especificações locais.
- ✅ **Hash SHA-256**: Gere hashes seguros para documentos de faturação.
- ✅ **Extensível**: Adicione novos países com estrutura modular.
- ✅ **Validação Integrada**: Valide dados contra esquemas XSD/JSON Schema.

---

## Instalação 🚀

```bash
# Instale o pacote principal
dotnet add package SAFT.Core

# Instale a implementação para Moçambique
dotnet add package SAFT.Mozambique
