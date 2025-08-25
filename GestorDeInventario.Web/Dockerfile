FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src

COPY ["GestorDeInventario.Web/GestorDeInventario.Web.csproj", "GestorDeInventario.Web/"]
RUN dotnet restore "GestorDeInventario.Web/GestorDeInventario.Web.csproj"

COPY . .
WORKDIR "/src/GestorDeInventario.Web"
RUN dotnet build "GestorDeInventario.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GestorDeInventario.Web.csproj" -c Release -o /app/publish

RUN dotnet ef database update --project /src/GestorDeInventario.Web/GestorDeInventario.Web.csproj --startup-project /src/GestorDeInventario.Web/GestorDeInventario.Web.csproj

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestorDeInventario.Web.dll"]