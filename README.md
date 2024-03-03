# Squares API

## About
Squares API finds squares in 2D plane from user inported points list. It finds sets of squares and how many of them can be drawn in a plane. Point is made of X and Y coordinates and Square is made out of 4 such connected points


- Example of a list of points that make up a square: [(-1;1), (1;1), (1;-1), (-1;-1)]

## Technologies used
- [.NET Core](https://www.microsoft.com/net/core/platform)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Docker](https://hub.docker.com/r/microsoft/dotnet/)
- [Prometheus](https://prometheus.io/)


## Use cases
- I as a user can import a list of points
- I as a user can add a point to an existing list
- I as a user can delete a point from an existing list
- I as a user can retrieve the squares identified

## Getting Started

### Prerequisities
- Accessible SQL database

### For testing
1. Clone repository to your machine
2. Set database connection string
3. Run projet on development environment

### For production
1. Clone repository to your machine
2. Set database connection string
3. Build docker image
4. Run docker containers