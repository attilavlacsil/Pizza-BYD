FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
#EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Pizza.Api/Pizza.Api.csproj", "Pizza.Api/"]
RUN dotnet restore "Pizza.Api/Pizza.Api.csproj"
COPY . .
WORKDIR "/src/Pizza.Api"
RUN dotnet build "Pizza.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pizza.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pizza.Api.dll"]