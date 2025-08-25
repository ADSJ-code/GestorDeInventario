FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

RUN dotnet publish "GestorDeInventario.Web/GestorDeInventario.Web.csproj" -c Release -o /app/publish

RUN dotnet ef database update --project "GestorDeInventario.Web/GestorDeInventario.Web.csproj"

WORKDIR /app/publish

ENV PORT 8080
ENV ASPNETCORE_URLS="http://+:8080"

ENTRYPOINT ["dotnet", "GestorDeInventario.Web.dll"]