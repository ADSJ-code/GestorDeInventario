FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

RUN dotnet publish ./GestorDeInventario.Web/GestorDeInventario.Web.csproj -c Release -o out

ENV PORT 8080
ENV ASPNETCORE_URLS="http://+:8080"

WORKDIR /app/out

CMD ["sh", "-c", "dotnet ef database update --project /app/GestorDeInventario.Web/GestorDeInventario.Web.csproj && dotnet GestorDeInventario.Web.dll"]