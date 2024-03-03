# Use the official .NET SDK image as the base image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore AS build-env

# Set the working directory to /app
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln .
COPY Squares.API/*.csproj ./Squares.API/
COPY Squares.Business/*.csproj ./Squares.Business/
COPY Squares.Domain/*.csproj ./Squares.Domain/
COPY Squares.Infrastructure/*.csproj ./Squares.Infrastructure/
RUN dotnet restore

# Copy the remaining source code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET Runtime image as the base image for running
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore

# Set the working directory to /app
WORKDIR /app

# Copy the published output from the build image
COPY --from=build-env /app/out .

# Expose the port that the application will run on
EXPOSE 80

# Define the command to run the application
CMD ["Squares.API.dll"]