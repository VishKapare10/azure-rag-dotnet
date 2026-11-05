# Azure RAG Application using .NET and Azure AI

A backend-only **Retrieval-Augmented Generation (RAG)** application built with **ASP.NET Core Web API, Azure AI Search, and Azure AI Foundry** for intelligent document search and AI-powered question answering.

## Overview

This project demonstrates how to build a Retrieval-Augmented Generation (RAG) solution using Microsoft Azure services and .NET. It enables users to retrieve relevant information from uploaded documents and generate context-aware responses using Large Language Models (LLMs).

## Technology Stack

- **Backend:** ASP.NET Core Web API
- **Programming Language:** C#
- **AI Platform:** Azure AI Foundry
- **Search Engine:** Azure AI Search
- **AI Model:** Azure OpenAI
- **Architecture:** Retrieval-Augmented Generation (RAG)

## Key Features

- Document ingestion and processing
- Text extraction and chunking
- Embedding generation
- Vector indexing using Azure AI Search
- Semantic and vector search
- Context retrieval
- AI-powered question answering
- RESTful API endpoints
- API testing using Swagger

## Architecture

The application follows a Retrieval-Augmented Generation architecture:

1. Documents are ingested into the application.
2. Document content is extracted and divided into smaller chunks.
3. Embeddings are generated for the document chunks.
4. Embeddings and document metadata are indexed in Azure AI Search.
5. User queries are converted into embeddings.
6. Relevant document chunks are retrieved through Azure AI Search.
7. Retrieved context is provided to the LLM.
8. The LLM generates a context-aware response.

## Project Structure

```text
azure-rag-dotnet/
├── src/
│   └── RAG.Api/
├── documents/
│   └── sample-documents/
├── tests/
├── README.md
├── .gitignore
└── LICENSE
```

## Getting Started

Setup instructions and configuration details will be added as the project evolves.

## Security

- Azure credentials and API keys must not be committed to the repository.
- Use environment variables or Azure Key Vault to manage secrets.
- Keep sensitive configuration files excluded through `.gitignore`.

## Project Status

🚧 Under Development

## License

This project is licensed under the MIT License.
