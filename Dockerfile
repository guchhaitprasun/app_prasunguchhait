# Dockerfile to build and generate dlls
# Author: PRASUN GUCHHAIT <prasun.guchhait@nagarro.com>

# Add Reference from microsoft container registry
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
LABEL MAINTAINER="PRASUNGUCHHAIT"
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY DevOps_WebAPI/DevOps_WebAPI.csproj ./
RUN dotnet restore

# Copy and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build Runtime Image 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DevOps_WebAPI.dll"]