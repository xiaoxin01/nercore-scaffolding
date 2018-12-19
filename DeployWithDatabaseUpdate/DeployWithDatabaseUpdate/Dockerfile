FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 3097
EXPOSE 44354

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY DeployWithDatabaseUpdate/DeployWithDatabaseUpdate.csproj DeployWithDatabaseUpdate/
RUN dotnet restore DeployWithDatabaseUpdate/DeployWithDatabaseUpdate.csproj
COPY . .
WORKDIR /src/DeployWithDatabaseUpdate
RUN dotnet build DeployWithDatabaseUpdate.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DeployWithDatabaseUpdate.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DeployWithDatabaseUpdate.dll"]
