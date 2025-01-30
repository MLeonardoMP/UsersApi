
# Users API

## Features
- Minimal API built with .NET 9
- CRUD endpoints for User management
- Basic in-memory data storage
- Simple logging middleware
- Very basic authorization check
- OpenAPI support in development
- Comprehensive request.http test file

## Getting Started
1. Make sure you have .NET 9 installed.
2. Restore packages and build the solution.
3. Run the application; by default, it listens on localhost with HTTPS.
4. Use the request.http file to test all endpoints with sample requests.

## Endpoints
- POST /users  
- GET /users  
- GET /users/{id}  
- PUT /users/{id}  
- DELETE /users/{id}