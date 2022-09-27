FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS="http://*:80;https://*:443"
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="password"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/aspnetapp.pfx"


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Server/Server.csproj", "Server/"]
COPY ["Server.Data/Server.Data.csproj", "Server.Data/"]
RUN dotnet restore "Server/Server.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Server/Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server/Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.dll"]
