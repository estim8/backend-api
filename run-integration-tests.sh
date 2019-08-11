#!/bin/sh

docker-compose -f docker-compose.yml -f docker-compose-all-tests.yml up --build integration-tests