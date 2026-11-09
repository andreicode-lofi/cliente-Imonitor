FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
WORKDIR /src

COPY ["cliente-imonitor-web/cliente-imonitor-web.csproj", "cliente-imonitor-web/"]
RUN dotnet restore "cliente-imonitor-web/cliente-imonitor-web.csproj"

COPY . .
WORKDIR "/src/cliente-imonitor-web"
RUN dotnet publish "cliente-imonitor-web.csproj" -c Release -o /app/publish /p:BlazorBootIntegrityCheck=false /p:BlazorEnableCompression=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY cliente-imonitor-web/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]