# Usa a imagem completa do SDK, que contém as ferramentas (como 'ef') e o runtime.
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Copia todos os arquivos do projeto para o contentor
COPY . .

# Expõe a porta que a aplicação vai usar no Render
ENV PORT=8080

# Comando de arranque: primeiro atualiza a base de dados, depois inicia a aplicação
CMD ["sh", "-c", "dotnet ef database update && dotnet watch run --urls http://*:$PORT"]