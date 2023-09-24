FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ms18-applicatie/ms18-applicatie.csproj", "ms18-applicatie/"]
RUN dotnet restore "ms18-applicatie/ms18-applicatie.csproj"
COPY . .
WORKDIR "/src/ms18-applicatie"
RUN dotnet build "ms18-applicatie.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ms18-applicatie.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ms18-applicatie.dll"]
