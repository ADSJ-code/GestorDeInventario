#!/bin/bash
set -e

echo "A executar migrações da base de dados..."
dotnet ef database update

echo "Migrações concluídas. A iniciar aplicação..."
exec dotnet out/GestorDeInventario.Web.dll