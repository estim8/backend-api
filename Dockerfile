FROM estim8-solution-base AS build-env

RUN dotnet publish ./src/Estim8.Backend.Api/Estim8.Backend.Api.csproj -o /app/out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS='http://+:5000'
ENTRYPOINT ["dotnet", "Estim8.Backend.Api.dll"]
