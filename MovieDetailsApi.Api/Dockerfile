FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["MovieDetailsApi.Api/MovieDetailsApi.Api.csproj", "MovieDetailsApi.Api/"]
RUN dotnet restore "MovieDetailsApi.Api/MovieDetailsApi.Api.csproj"
COPY . .
WORKDIR "/src/MovieDetailsApi.Api"
RUN dotnet build "MovieDetailsApi.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MovieDetailsApi.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MovieDetailsApi.Api.dll"]