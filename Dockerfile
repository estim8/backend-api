FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY ./*.sln ./

# Move src projects
COPY ./src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./src/${file%.*}/ && mv $file ./src/${file%.*}/; done
 
#Move tst projects
COPY ./tst/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./tst/${file%.*}/ && mv $file ./tst/${file%.*}/; done
 
RUN dotnet restore
 
COPY . .

RUN dotnet publish ./src/Estim8.Backend.Api/Estim8.Backend.Api.csproj -o /app/out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Estim8.Backend.Api.dll"]
