FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

WORKDIR /app/GestorDeInventario.Web

ENV ASPNETCORE_URLS="http://+:8080"

CMD ["sh", "-c", "dotnet ef database update && dotnet run --urls http://*:$PORT"]