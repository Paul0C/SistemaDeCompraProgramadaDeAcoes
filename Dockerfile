# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution + csproj files first so restore is cached separately from source changes
COPY src/Backend/CompraProgramada.sln                                               src/Backend/
COPY src/Backend/CompraProgramada.API/CompraProgramada.API.csproj                   src/Backend/CompraProgramada.API/
COPY src/Backend/CompraProgramada.Application/CompraProgramada.Application.csproj   src/Backend/CompraProgramada.Application/
COPY src/Backend/CompraProgramada.Domain/CompraProgramada.Domain.csproj             src/Backend/CompraProgramada.Domain/
COPY src/Backend/CompraProgramada.Infrastructure/CompraProgramada.Infrastructure.csproj src/Backend/CompraProgramada.Infrastructure/
COPY tests/tests.csproj                                                              tests/

RUN dotnet restore src/Backend/CompraProgramada.sln

# Copy everything else and build
COPY . .
RUN dotnet build src/Backend/CompraProgramada.sln -c Release --no-restore
RUN dotnet test src/Backend/CompraProgramada.sln -c Release --no-build --verbosity normal
RUN dotnet publish src/Backend/CompraProgramada.API/CompraProgramada.API.csproj -c Release --no-build -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "CompraProgramada.API.dll"]
