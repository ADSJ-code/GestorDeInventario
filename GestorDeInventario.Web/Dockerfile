FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY GestorDeInventario.Web/*.csproj ./GestorDeInventario.Web/
RUN dotnet restore ./GestorDeInventario.Web/GestorDeInventario.Web.csproj

COPY . .

RUN dotnet publish ./GestorDeInventario.Web/GestorDeInventario.Web.csproj -c Release -o out

COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]