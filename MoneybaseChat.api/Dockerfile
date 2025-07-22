# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY *.sln .
COPY MoneybaseChat.api/*.csproj ./MoneybaseChat.api/
COPY MoneybaseChat.application/*.csproj ./MoneybaseChat.application/
COPY MoneybaseChat.domain/*.csproj ./MoneybaseChat.domain/
COPY MoneybaseChat.infrastructure/*.csproj ./MoneybaseChat.infrastructure/

RUN dotnet restore

# Copy the entire project and build
COPY . .
WORKDIR /app/MoneybaseChat.api
RUN dotnet publish -c Release -o /out

# Use ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# no need to use wait for it
#COPY wait-for-it.sh .
#RUN chmod +x wait-for-it.sh

# ENTRYPOINT ["./wait-for-it.sh", "rabbitmq:5672", "--timeout=120", "--strict", "--", "dotnet", "MoneybaseChat.api.dll"]
ENTRYPOINT ["dotnet", "MoneybaseChat.api.dll"]
