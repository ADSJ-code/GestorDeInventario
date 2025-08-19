FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

RUN dotnet publish -c Release -o out
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]