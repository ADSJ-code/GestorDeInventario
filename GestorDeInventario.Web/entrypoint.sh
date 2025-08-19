#!/bin/sh
set -e

echo "A iniciar aplicação (migração será executada manualmente)..."
exec dotnet /app/out/GestorDeInventario.Web.dll