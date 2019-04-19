#!/bin/bash

cd ./src/whatflix.Api
rm -rf ./bin/output
dotnet publish -c Release -o ./bin/output
docker build -t whatflix .
docker tag whatflix registry.heroku.com/whatflix/web
docker push registry.heroku.com/whatflix/web
heroku container:release web -a whatflix