# Lobby Manager API
A REST API template for managing game lobbies and users.
The backend is built with C# (.NET 9.0) and ASP.NET core.
A lightweight demo frontend is included using Node.js, TypeScript, React, and Vite.

## Features
- RESTful API for managing users and game lobbies
- Token-based authentication
- Lobby creation and membership management
- Expandable backend architecture
- Demo frontend for interacting with the API

## Project Scope
The main focus of this project is building a clean, extensible backend architecture.
Because of this, some parts of the project are intentionally simplified:
- The frontend is a demo and should be treated as such, code in the ``./client`` folder may be suboptimal, but functional to interact with the backend in its current state.
- There is no SQL database, everything is stored in-memory. This way any long-term storage solutions can be chosen down the line.
- User authentication is functional, but simplified. There are no e-mails or passwords, since for some simpler projects you may not need these.

## Project Structure
- ``./.github`` -> CI/CD workflows
- ``./client`` -> React demo frontend
- ``./server`` -> ASP.NET Core backend

# Running the project
For starters, clone the repo.
```
git clone https://github.com/DaFrankort/game-backend.git
```

Copy the ``example.env`` file and rename it to ``.env``. You may need to change the URL later depending on where the backend server runs.

## Backend
The backend server can be launched by using the following command.
```
cd server
dotnet run
```

## Frontend
Start the backend before starting the frontend, and ensure the URL in ``.env`` is the same URL the backend is running on.
The frontend can be run by using the following commands:
```
cd client
npm install
npm run dev
```

# Endpoints
The backend has the following endpoints:
## Users
- ``POST /api/user`` - Create a new user, requires a JSON body with the name of the user using the following format: ``{ "name": "Username" }``. Important to note that doing this will return JSON for your user *with* a unique **bearer token**, this token is required in all your other requests or they will be denied.
- ``GET /api/user/{userId}`` - Get specific information from a user.
- ``GET /api/user/me`` - Using your bearer token as context, will return your associated user-profile.
- ``GET /api/user`` - Get an overview of all users.
This endpoint is paginated, by default it returns the first 25 results of the first page. You can specify the page and page size by using query parameters, for example: ``/api/user?page=2&limit=25``.

## Lobbies
All lobby endpoints require you to have a valid **bearer token**. You can get one by creating a user.
- ``GET /api/lobby`` - Get an overview of all lobbies, with limited information about users.
This endpoint is paginated, by default it returns the first 25 results of the first page. You can specify the page and page size by using query parameters, for example: ``/api/lobby?page=2&limit=30``.
- ``POST /api/lobby`` - Create a new lobby, requires a body with the name for the lobby, using the following format: ``{ "name": "MyLobby" }``.
- ``GET /api/lobby/{lobbyId}`` - Get information about a specific lobby. Returns more detailed information about users in the lobby if the requester is a member of the lobby, otherwise it returns limited information about users in the lobby.
- ``POST /api/lobby/{lobbyId}/members`` - Join a lobby, this will use your bearer token to determine which user to add. A user can only be added to a lobby by themselves.
- ``DELETE /api/lobby/{lobbyId}/members`` - Leave a lobby, this will use your bearer token to determine which user to remove. A user can only leave a lobby they are a member of.
- ``DELETE /api/lobby/{lobbyId}/members/{userId}`` - Remove a user from a lobby, the lobby owner can kick anyone, and users can kick themselves. This will use your bearer token to determine which user is trying to perform the action.
