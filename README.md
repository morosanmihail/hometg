# HomeTG

Self-hosted Magic: the Gathering card collection tracker.
Spawned from the desire to have an easy to manage collection (easily done by many wonderful online tools), that can also be easily updated in more non-conventional means.

The main use case is to scan cards with Delver Lens, then using its HTTP request feature, add the cards to HomeTG as well.
Then be able to manage the collection outside of Delver Lens however needed.

![Example of the UI](https://github.com/morosanmihail/hometg/blob/main/images/ui20230628.jpg?raw=true)

## Installation

### Docker

Easiest is via Portainer (just create a new stack and point it to this repo, then change volumes as needed).

Of course, you also have access to the `docker-compose.yml` if you want to take advantage of that.

Finally, there's a `Dockercompose` for you if you want to go full manual.

If using Sqlite, HomeTG stores all its user data in `/usr/share/hometg`, so make sure to map that volume so it does not get lost.

### Other

You can run this directly by compiling the solution.
It should run on any OS supported by .NET Core without issues.

On Windows, with Sqlite as the database, user data is stored in `ProgramData\hometg`.

## Features

### Card database

The card database is downloaded on start-up from www.mtgjson.com if a newer version is available.

You can trigger a manual update check via the `/mtg/update` endpoint.

Coming soon:
- scheduled automatic update.
- live migration of mtgjson.com Sqlite database into Postgres, for higher performance.

### Card search

You can search for cards by various filters (more to be added) in both the Magic database and in your existing collections.

### Collection management

You can add / remove cards to the collection, both one by one and in bulk, through importing.

Moving cards between collections also possible.

Deleting cards from a collection, as well as outright deleting collections, also possible.
