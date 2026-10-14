FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /app

COPY FileIntegrityChecker.csproj .
RUN dotnet restore

COPY Program.cs .
COPY FileIntegrityService.cs .
COPY samples/ ./samples/
COPY baseline.json .

RUN dotnet build -c Release --no-restore

COPY check.sh .
RUN chmod +x check.sh

CMD ["./check.sh"]