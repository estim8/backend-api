FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
 
RUN dotnet restore
 
COPY . .

RUN dotnet publish ./Estim8.Backend.Api/Estim8.Backend.Api.csproj -o /app/out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Estim8.Backend.Api.dll"]
