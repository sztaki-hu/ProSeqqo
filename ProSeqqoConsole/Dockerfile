#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SequencerConsole/SequencerConsole.csproj", "SequencerConsole/"]
COPY ["SequencePlanner/SequencePlanner.csproj", "SequencePlanner/"]
RUN dotnet restore "SequencerConsole/SequencerConsole.csproj"
COPY . .
WORKDIR "/src/SequencerConsole"
RUN dotnet build "SequencerConsole.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SequencerConsole.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SequencerConsole.dll"]