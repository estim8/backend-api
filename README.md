[![Build status](https://dev.azure.com/estim8/Estim8.io/_apis/build/status/Estim8.Backend-CI)](https://dev.azure.com/estim8/Estim8.io/_build/latest?definitionId=2)

---
This documentation (and the Wiki) is work in progress. Hopefully, it won't be that long until it's in a stable state :sweat_smile:

---

<img src="https://github.com/estim8/backend-api/raw/master/doc/img/logo-small.png" width="200">

# A free online estimation game for agile teams

Estim8 is a free online platform for agile delivery teams to estimate work in an easy and fun way.

![Sample UI](doc/img/sample-ui-cards-shown.png?s=400)

The above is, by the way, a mock-up at the moment ;-) You can find the full mock-up on our [Sketch cloud](https://sketch.cloud/s/eKvja).

Read more about our vision and see some more nice mockups in the [Wiki](https://github.com/estim8/backend-api/wiki/Vision)

## Helping distributed teams

Running estimation sessions with distributed colleagues is hard (or at least a lot harder than it has to be!). With Estim8 we try to create a modern, nice-looking and free tool to easy setup estimation sessions with colleagues across the globe.

# Navigating this repo

This repository contains the API component for the Estim8 product. The web front project is hosted in a [separate repo](https://github.com/estim8/web-frontend).

The repo is organized in these folders:
- `/src` is where the source code and solution files goes
- `/tst` is where tests are written
- `/doc` is for documentation and supporting files

You can read more about the overall architecture [here](https://github.com/estim8/backend-api/wiki/Architecture).

# How to run the code

Running the code is really easy. Just clone the repo and run `docker-compose -f ./docker-compose.yml up` from a terminal.
This will build and spin up the API in a container and Redis in another. If you have [Docker](https://www.docker.com/get-started) installed, that is.

The API is then accessible on `http://localhost:33000`

## Latest build

The latest successful build is automatically deployed in QA and is accessible at https://api-qa.estim8.io

# Get involved

We are looking for contributors! Please reach out to us if you're up for helping on a brand new open source project (with developers new to running open source projects as well)!

Have a look at the [Issues list](https://github.com/estim8/backend-api/issues) to see the work that is currently up for grabs. We really welcome any feedback!

## Project roadmap

See the [project roadmap](https://github.com/orgs/estim8/projects/1) on our main project board. 
