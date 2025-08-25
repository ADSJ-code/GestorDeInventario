FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.csproj .
COPY GestorDeInventario.Web/*.csproj ./GestorDeInventario.Web/
RUN dotnet restore "./GestorDeInventario.Web/GestorDeInventario.Web.csproj"

COPY . .

WORKDIR /source/GestorDeInventario.Web
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV PORT 8080
ENTRYPOINT ["dotnet", "GestorDeInventario.Web.dll"]