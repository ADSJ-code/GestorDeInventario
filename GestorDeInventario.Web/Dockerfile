FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

WORKDIR /app/GestorDeInventario.Web

RUN dotnet publish -c Release -o /app/publish

RUN dotnet ef database update --startup-project /app/GestorDeInventario.Web --project /app/GestorDeInventario.Web

WORKDIR /app/publish

ENV PORT 8080
ENV ASPNETCORE_URLS="http://+:8080"

ENTRYPOINT ["dotnet", "GestorDeInventario.Web.dll"]