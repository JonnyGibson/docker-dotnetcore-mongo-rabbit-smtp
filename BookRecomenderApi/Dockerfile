FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app
# Copy everything and build
COPY . ./
RUN dotnet restore "./BookRecomenderApi.csproj"
RUN dotnet publish "BookRecomenderApi.csproj" -c Release -o out
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BookRecomenderApi.dll"]