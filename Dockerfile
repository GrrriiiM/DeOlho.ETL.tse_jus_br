FROM microsoft/dotnet:2.2-sdk-alpine AS build-env
WORKDIR /src

COPY /src .


WORKDIR /src/4-API
RUN dotnet publish -c Release -o ../../publish

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine

WORKDIR /publish
COPY --from=build-env /publish .

ENTRYPOINT ["dotnet", "DeOlho.ETL.tse_jus_br.API.dll"]
