# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
WORKDIR /src

# Copia a solução e restaura a partir da raiz
COPY ["cliente-imonitor-web/cliente-imonitor-web.csproj", "cliente-imonitor-web/"]
RUN dotnet restore "cliente-imonitor-web/cliente-imonitor-web.csproj"

# Copia todo o código fonte
COPY . .

# Compila explicitamente o projeto a partir da raiz do container
RUN dotnet publish "cliente-imonitor-web/cliente-imonitor-web.csproj" -c Release -o /app/publish /p:BlazorBootIntegrityCheck=false /p:BlazorEnableCompression=false

# Estágio 2: Servidor NGINX
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Limpa a pasta padrão do Nginx antes de copiar
RUN rm -rf /usr/share/nginx/html/*

# Copia o wwwroot gerado no publish para a raiz web do Nginx
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Copia a configuração do Nginx com fallback SPA
COPY cliente-imonitor-web/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]