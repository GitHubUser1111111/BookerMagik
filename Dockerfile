FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS builder
WORKDIR /source

COPY ./src/EAS.sln .
COPY ./src/WebApi/SystemStateService.csproj ./WebApi/
COPY ./src/Tests/Tests.csproj ./Tests/
COPY ./src/EntityLibrary/EntityLibrary.csproj ./EntityLibrary/

RUN dotnet restore

COPY ./src/WebApi ./WebApi
COPY ./src/Tests ./Tests
COPY ./src/EntityLibrary ./EntityLibrary

RUN dotnet test ./EAS.sln --configuration Release --no-restore

RUN dotnet publish "./WebApi/SystemStateService.csproj" --output "../dist" --configuration Release --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim
WORKDIR /app
COPY --from=builder /dist . 
EXPOSE 80 443
ENTRYPOINT ["dotnet", "ServiceStateService.dll"]
