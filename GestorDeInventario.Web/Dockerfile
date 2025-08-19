FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

WORKDIR /app/out
ENV ASPNETCORE_URLS="http://+:8080"

CMD ["sh", "-c", "dotnet ef database update && dotnet GestorDeInventario.Web.dll"]