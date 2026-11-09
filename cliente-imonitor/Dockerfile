FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["cliente-imonitor/cliente-imonitor.csproj", "cliente-imonitor/"]
RUN dotnet restore "cliente-imonitor/cliente-imonitor.csproj"

COPY . .
WORKDIR "/src/cliente-imonitor"
RUN dotnet publish "cliente-imonitor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV PORT=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "cliente-imonitor.dll"]