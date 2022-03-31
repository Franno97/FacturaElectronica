#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY NuGet.Config ./
COPY ["src/Mre.Visas.FacturaElectronica.Api/Mre.Visas.FacturaElectronica.Api.csproj", "./Mre.Visas.FacturaElectronica.Api/"]
COPY ["src/Mre.Visas.FacturaElectronica.Application/Mre.Visas.FacturaElectronica.Application.csproj", "./Mre.Visas.FacturaElectronica.Application/"]
COPY ["src/Mre.Visas.FacturaElectronica.Domain/Mre.Visas.FacturaElectronica.Domain.csproj", "./Mre.Visas.FacturaElectronica.Domain/"]
COPY ["src/Mre.Visas.FacturaElectronica.Infrastructure/Mre.Visas.FacturaElectronica.Infrastructure.csproj", "./Mre.Visas.FacturaElectronica.Infrastructure/"]
RUN dotnet restore --configfile NuGet.Config "Mre.Visas.FacturaElectronica.Api/Mre.Visas.FacturaElectronica.Api.csproj"

COPY ["src/Mre.Visas.FacturaElectronica.Api", "./Mre.Visas.FacturaElectronica.Api/"]
COPY ["src/Mre.Visas.FacturaElectronica.Application", "./Mre.Visas.FacturaElectronica.Application/"]
COPY ["src/Mre.Visas.FacturaElectronica.Domain", "./Mre.Visas.FacturaElectronica.Domain/"]
COPY ["src/Mre.Visas.FacturaElectronica.Infrastructure", "./Mre.Visas.FacturaElectronica.Infrastructure/"]
RUN dotnet build "Mre.Visas.FacturaElectronica.Api/Mre.Visas.FacturaElectronica.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mre.Visas.FacturaElectronica.Api/Mre.Visas.FacturaElectronica.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mre.Visas.FacturaElectronica.Api.dll"]