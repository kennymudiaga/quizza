#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Quizza.Users.WebApi/Quizza.Users.WebApi.csproj", "Quizza.Users.WebApi/"]
RUN dotnet restore "Quizza.Users.WebApi/Quizza.Users.WebApi.csproj"
COPY . .
WORKDIR "/src/Quizza.Users.WebApi"
RUN dotnet build "Quizza.Users.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Quizza.Users.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quizza.Users.WebApi.dll"]