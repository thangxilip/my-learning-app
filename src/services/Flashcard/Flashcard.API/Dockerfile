FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/", "src/"]
RUN dotnet restore "src/services/Flashcard/Flashcard.API/Flashcard.API.csproj"

WORKDIR "/src/src/services/Flashcard/Flashcard.API"
RUN dotnet build "Flashcard.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Flashcard.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Flashcard.API.dll"]
