FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /src
COPY ./src /src
RUN ls
RUN dotnet test
RUN dotnet publish HackerNewsCli --output /src/out
RUN ls /src/out

FROM microsoft/dotnet:2.2-sdk-alpine
WORKDIR /app
COPY --from=build-env /src/out ./
ENTRYPOINT ["dotnet", "HackerNewsCli.dll"]