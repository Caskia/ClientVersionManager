FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/ClientVersionManager/ClientVersionManager.csproj", "src/ClientVersionManager/"]
RUN dotnet restore "src/ClientVersionManager/ClientVersionManager.csproj"
COPY . .
WORKDIR "/src/src/ClientVersionManager"
RUN dotnet build "ClientVersionManager.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ClientVersionManager.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClientVersionManager.dll"]