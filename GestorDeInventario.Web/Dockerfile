FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

ENV PORT=8080

CMD ["sh", "-c", "dotnet ef database update && dotnet watch run --urls http://*:$PORT"]