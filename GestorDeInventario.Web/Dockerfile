FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

WORKDIR /app/GetorDeInventario.Web

RUN dotnet ef database update

RUN dotnet publish -c Release -o /app/publish

WORKDIR /app/publish

ENV ASPNETCORE_URLS="http://+:8080"

ENTRYPOINT ["dotnet", "GestorDeInventario.Web.dll"]