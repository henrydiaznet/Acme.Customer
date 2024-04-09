FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y curl
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Acme.Customers.API/Acme.Customers.API.csproj", "src/Acme.Customers.API/"]
COPY ["src/Acme.Customers.Infrastructure/Acme.Customers.Infrastructure.csproj", "src/Acme.Customers.Infrastructure/"]
COPY ["src/Acme.Customers.Domain/Acme.Customers.Domain.csproj", "src/Acme.Customers.Domain/"]
RUN dotnet restore "src/Acme.Customers.API/Acme.Customers.API.csproj"
COPY . .
WORKDIR "/src/src/Acme.Customers.API"
RUN dotnet build "Acme.Customers.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Acme.Customers.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acme.Customers.API.dll"]
