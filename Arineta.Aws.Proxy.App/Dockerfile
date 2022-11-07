#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Arineta.Aws.Proxy.App/Arineta.Aws.Proxy.App.csproj", "Arineta.Aws.Proxy.App/"]
COPY ["Arineta.Aws.Proxy/Arineta.Aws.Proxy.Service.csproj", "Arineta.Aws.Proxy/"]
COPY ["Arineta.Aws.Dto/Arineta.Aws.Dto.csproj", "Arineta.Aws.Dto/"]
RUN dotnet restore "Arineta.Aws.Proxy.App/Arineta.Aws.Proxy.App.csproj"
COPY . .
WORKDIR "/src/Arineta.Aws.Proxy.App"
RUN dotnet build "Arineta.Aws.Proxy.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Arineta.Aws.Proxy.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arineta.Aws.Proxy.App.dll"]