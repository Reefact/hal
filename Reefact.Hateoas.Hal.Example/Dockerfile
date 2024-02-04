#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LICENSE", "LICENSE"]
COPY ["Reefact.Hateoas.Hal.Example/Reefact.Hateoas.Hal.Example.csproj", "Reefact.Hateoas.Hal.Example/"]
COPY ["Reefact.Hateoas.Hal.AspNetCore/Reefact.Hateoas.Hal.AspNetCore.csproj", "Reefact.Hateoas.Hal.AspNetCore/"]
COPY ["Reefact.Hateoas.Hal/Reefact.Hateoas.Hal.csproj", "Reefact.Hateoas.Hal/"]
RUN dotnet restore "Reefact.Hateoas.Hal.Example/Hal.Example.csproj"
COPY . .
WORKDIR "/Reefact.Hateoas.Hal.Example"
RUN dotnet build "Reefact.Hateoas.Hal.Example.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reefact.Hateoas.Hal.Example.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reefact.Hateoas.Hal.Example.dll"]