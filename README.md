# HomeTG

Self-hosted Magic: the Gathering card collection tracker. 
Spawned from the desire to have an easy to manage collection (easily done by many wonderful online tools), that can also be easily updated in more non-conventional means.

The main use case is to scan cards with Delver Lens, then using its HTTP request feature, add the cards to HomeTG as well. 
Then be able to manage the collection outside of Delver Lens however needed. 

## Installation

### Docker 

Easiest is via Portainer (just create a new stack and point it to this repo, then change volumes as needed).

Of course, you also have access to the `docker-compose.yml` if you want to take advantage of that.

Finally, there's a `Dockercompose` for you if you want to go full manual. 

HomeTG stores all its user data in `/usr/share/hometg`, so make sure to map that volume so it does not get lost. 

### Other

You can run this directly by compiling the solution. 
It should run on any OS supported by .NET Core without issues.

On Windows, user data is stored in `ProgramData\hometg`. 

## Features

### Card database

The card database is downloaded on start-up from www.mtgjson.com if a newer version is available. 

You can trigger a manual update check via the `/mtg/update` endpoint. 

Coming soon: scheduled automatic update.

### Card search

You can currently search for cards by name and / or set. 
More filters incoming. 

### Collection management

You can add / remove cards to the collection. 

Searching through your collection by name and / or set is also possible.

## Endpoints

[TODO]
