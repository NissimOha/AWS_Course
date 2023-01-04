#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Arineta.Aws.App/Arineta.Aws.App.csproj", "Arineta.Aws.App/"]
COPY ["Arineta.Aws.Logic/Arineta.Aws.Logic.csproj", "Arineta.Aws.Logic/"]
COPY ["Arineta.Aws.Common/Arineta.Aws.Common.csproj", "Arineta.Aws.Common/"]
COPY ["Arineta.Aws.Service/Arineta.Aws.Service.csproj", "Arineta.Aws.Service/"]
COPY ["Arineta.Aws.Dto/Arineta.Aws.Dto.csproj", "Arineta.Aws.Dto/"]
COPY ["Arineta.Aws.DataAccess/Arineta.Aws.DataAccess.csproj", "Arineta.Aws.DataAccess/"]
RUN dotnet restore "Arineta.Aws.App/Arineta.Aws.App.csproj"
COPY . .
WORKDIR "/src/Arineta.Aws.App"
RUN dotnet build "Arineta.Aws.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Arineta.Aws.App.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt update
RUN apt install -y vim
RUN apt install -y iproute2
RUN apt install -y curl
RUN apt install -y iputils-ping
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arineta.Aws.App.dll"]