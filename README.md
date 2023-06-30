[![.NET](https://github.com/morosanmihail/hometg/actions/workflows/dotnet.yml/badge.svg)](https://github.com/morosanmihail/hometg/actions/workflows/dotnet.yml)
# HomeTG

Self-hosted Magic: the Gathering card collection tracker.
Spawned from the desire to have an easy to manage collection (easily done by many wonderful online tools), that can also be easily updated in more non-conventional means.

![Example of the UI](https://github.com/morosanmihail/hometg/blob/main/images/ui20230628.jpg?raw=true)

## Installation

Easiest is via Portainer with Docker Compose (just create a new stack and point it to this repo, then change volumes as needed).

For more info, [check the wiki](https://github.com/morosanmihail/hometg/wiki/Installation).

## Features

### Card database

The card database is downloaded on start-up from www.mtgjson.com if a newer version is available.

You can trigger a manual update check via the `/mtg/update` endpoint.

Coming soon:
- scheduled automatic update.
- live migration of mtgjson.com Sqlite database into Postgres, for higher performance.

### Card search

You can search for cards by various filters (more to be added) in both the Magic database and in your existing collections.

![Example of the Search UI](https://github.com/morosanmihail/hometg/blob/main/images/search20230628.png?raw=true)

### Collection management

You can add / remove cards to the collection, both one by one and in bulk, through importing.

Moving cards between collections also possible.

Deleting cards from a collection, as well as outright deleting collections, also possible.

For a more thorough description of features, [please check the wiki](https://github.com/morosanmihail/hometg/wiki/Features).
