# Gaming API

Welcome to the Gaming API! This API provides access to a collection of games, allowing users to retrieve game details, paginate results, and filter by various criteria.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [API Endpoints](#api-endpoints)
  - [Get Games with Offset and Limit](#get-games-with-offset-and-limit)
  - [Get Games with Page and Page Size](#get-games-with-page-and-page-size)
- [Running Tests](#running-tests)
- [Data](#data)

## Features

- Retrieve a list of games with pagination options.
- Filter results based on various criteria.
- Simple and intuitive RESTful API design.

## Getting Started

### Prerequisites

To run this API, you'll need the following:

- [.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- A code editor (like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/))
- [Postman](https://www.postman.com/) or a similar tool for testing API endpoints (optional)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/pttg24/api-games-basic-pagination.git
   cd api-games-basic-pagination
   
2. Restore dependencies:

    ```bash
    dotnet restore
    
3. Run the API:

    ```bash
    dotnet run
    
The API will be available at http://localhost:5000 (or a different port - see launchSettings.json).

### API Endpoints

The API has two endpoints corresponding to two different pagination options and implementation.

***

**OPTION A - Get Games with Offset and Limit**

    GET /api/a-games

**Query Parameters:**

    offset (int): The number of items to skip (default: 0).
    limit (int): The maximum number of items to return (default: 2).

**Example Request:**

    GET /api/games?offset=0&limit=2
    
***    

**OPTION B - Get Games with Page and Page Size**

    GET /api/b-games

**Query Parameters:**

    page (int): The page number to retrieve (default: 1).
    pageSize (int): The number of items per page (default: 10).
    orderBy (string): Order by release date (asc or desc, default: desc).

**Example Request:**

    GET /api/games?page=1&pageSize=2&orderBy=desc
    
***     

### Running Tests

To run unit tests for the API, execute the following command in the test project directory:

    dotnet test

### Data

API data is provided under /docs folder.
A GitHub deployment makes the data accessible via the following url:
https://pttg24.github.io/api-games-basic-pagination/steam_games_feed.json