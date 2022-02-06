#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DiscordLayer/DiscordLayer.csproj", "DiscordLayer/"]
COPY ["PV178StudyBot/PV178StudyBotDAL.csproj", "PV178StudyBot/"]
RUN dotnet restore "DiscordLayer/DiscordLayer.csproj"
COPY . .
WORKDIR "/src/DiscordLayer"
RUN dotnet build "DiscordLayer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordLayer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordLayer.dll"]