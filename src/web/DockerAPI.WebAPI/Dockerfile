#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf

# Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./web/DockerAPI.WebAPI/DockerAPI.WebAPI.csproj", "web/DockerAPI.WebAPI/"]

# Copia os projetos de dependência
COPY ["./dominio/DockerAPI.Dominio/DockerAPI.Dominio.csproj", "dominio/DockerAPI.Dominio/"]

# Checa as dependências do projeto e faz o build
RUN dotnet restore "web/DockerAPI.WebAPI/DockerAPI.WebAPI.csproj"
COPY . .
WORKDIR "web/DockerAPI.WebAPI"

RUN dotnet build "DockerAPI.WebAPI.csproj" -c Release -o /app/build

# Publica o projeto
FROM build AS publish
RUN dotnet publish "DockerAPI.WebAPI.csproj" -c Release -o /app/publish

# Copia os arquivos de produção e executa a dll
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerAPI.WebAPI.dll"]