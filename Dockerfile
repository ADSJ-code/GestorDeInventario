FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

CMD cd GestorDeInventario.Web && dotnet ef database update && dotnet run --urls http://0.0.0.0:$PORT
