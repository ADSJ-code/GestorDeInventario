FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

RUN dotnet publish ./GestorDeInventario.Web/GestorDeInventario.Web.csproj -c Release -o out

RUN dotnet ef database update --project ./GestorDeInventario.Web/GestorDeInventario.Web.csproj --no-build

WORKDIR /app/out

ENTRYPOINT ["dotnet", "GestorDeInventario.Web.dll"]