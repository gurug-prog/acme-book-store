FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./aspnet-core ./aspnet-core
WORKDIR ./aspnet-core
RUN dotnet tool install --global Volo.Abp.Cli --version 7.2.2 && \
    export PATH="$PATH:/root/.dotnet/tools" && \
    abp install-libs
WORKDIR ./src/Acme.BookStore.HttpApi.Host
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Acme.BookStore.HttpApi.Host.dll"]
