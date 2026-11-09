# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
WORKDIR /src

# Copia e restaura dependências
COPY ["cliente-imonitor-web/cliente-imonitor-web.csproj", "cliente-imonitor-web/"]
RUN dotnet restore "cliente-imonitor-web/cliente-imonitor-web.csproj"

# Copia todo o código fonte
COPY . .

# Compila o projeto publicando o wwwroot direto na pasta /app/publish
RUN dotnet publish "cliente-imonitor-web/cliente-imonitor-web.csproj" -c Release -o /app/publish /p:BlazorBootIntegrityCheck=false /p:BlazorEnableCompression=false

# Estágio 2: NGINX
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Limpa arquivos padrão do nginx
RUN rm -rf /usr/share/nginx/html/*

# Copia o CONTEÚDO da pasta wwwroot gerada pelo publish para a raiz do Nginx
COPY --from=build /app/publish/wwwroot/ .

# Copia a configuração de rotas do Nginx
COPY cliente-imonitor-web/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]