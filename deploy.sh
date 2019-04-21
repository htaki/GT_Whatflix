#!/bin/bash

cd ./src/whatflix.presentation.api
rm -rf ./bin/output
dotnet publish -c Release -o ./bin/output
docker build -t whatflix .
docker tag whatflix registry.heroku.com/whatflix/web
heroku container:push web -a whatflix
heroku container:release web -a whatflix