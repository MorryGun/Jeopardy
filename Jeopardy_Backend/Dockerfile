#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Jeopardy_Backend/Jeopardy_Backend.csproj", "Jeopardy_Backend/"]
RUN dotnet restore "Jeopardy_Backend/Jeopardy_Backend.csproj"
COPY . .
WORKDIR "/src/Jeopardy_Backend"
RUN dotnet build "Jeopardy_Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jeopardy_Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jeopardy_Backend.dll"]
