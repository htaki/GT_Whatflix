FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY ./bin/output .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Whatflix.Api.dll