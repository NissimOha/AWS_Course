#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Arineta.Aws.SnsProxy.App/Arineta.Aws.SnsProxy.App.csproj", "Arineta.Aws.SnsProxy.App/"]
RUN dotnet restore "Arineta.Aws.SnsProxy.App/Arineta.Aws.SnsProxy.App.csproj"
COPY . .
WORKDIR "/src/Arineta.Aws.SnsProxy.App"
RUN dotnet build "Arineta.Aws.SnsProxy.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Arineta.Aws.SnsProxy.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arineta.Aws.SnsProxy.App.dll"]