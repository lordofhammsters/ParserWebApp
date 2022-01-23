# https://docs.docker.com/samples/dotnetcore/
# https://www.jetbrains.com/dotnet/guide/tutorials/docker-dotnet/local-dotnet-development-docker/
# docker build -t lordofhammsters/parserwebapp .
# docker run -dp 8080:80 lordofhammsters/parserwebapp
# docker container ls
# docker stop {container_id}
# docker rm {container_id}
# docker rmi lordofhammsters/parserwebapp
# docker push lordofhammsters/parserwebapp

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .

# restore solution
RUN dotnet restore ParserWebApp.sln

WORKDIR "/src/src/WebUI"
RUN dotnet build "WebUI.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/src/WebUI"
RUN dotnet publish "WebUI.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParserWebApp.WebUI.dll"]