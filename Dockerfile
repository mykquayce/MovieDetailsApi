FROM microsoft/dotnet:2.1-sdk as build-env
WORKDIR /app

COPY . .
RUN dotnet restore MovieDetailsApi.sln -s https://api.nuget.org/v3/index.json
RUN dotnet publish MovieDetailsApi.Api/MovieDetailsApi.Api.csproj -c Release -o /app/publish -r linux-x64

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "MovieDetailsApi.Api.dll"]
