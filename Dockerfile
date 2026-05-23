# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY src/*/*.csproj ./
RUN for f in *.csproj; do mkdir -p "src/$(basename $f .csproj)" && mv "$f" "src/$(basename $f .csproj)/"; done
COPY tests/*/*.csproj ./
RUN for f in *.csproj; do mkdir -p "tests/$(basename $f .csproj)" && mv "$f" "tests/$(basename $f .csproj)/"; done

RUN dotnet restore

COPY . .
RUN dotnet build -c Release --no-restore
RUN dotnet test -c Release --no-build --no-restore
RUN dotnet publish src/CompraProgramada.API -c Release --no-build -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "CompraProgramada.API.dll"]
