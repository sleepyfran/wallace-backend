FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY wallace/*.csproj ./wallace/
RUN dotnet restore

# copy everything else and build app
COPY wallace/. ./wallace/
WORKDIR /source/wallace
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:$PORT
ENTRYPOINT ["dotnet", "Wallace.dll"]
