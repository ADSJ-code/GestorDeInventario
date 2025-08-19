#!/bin/sh
set -e

echo "A executar migrações da base de dados..."
dotnet ef database update --project ./GestorDeInventario.Web/GestorDeInventario.Web.csproj

echo "Migrações concluídas. A iniciar aplicação..."
exec dotnet /app/out/GestorDeInventario.Web.dll