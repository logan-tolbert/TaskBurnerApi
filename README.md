# 🔥 TaskBurner API 🔥

TaskBurner API is a minimal .NET 9 Web API for managing issues. 
This API provides basic CRUD operations for an issue-tracking system 
and includes logging and validation features. 

## ✨ Features

- Minimal API structure using .NET 9
- CRUD operations for managing issues
- Console, Debug and Event Source logging
- xUnit test coverage
- In-memory database simulation

## 💡 Planned Features & Enhancements

- External logging aggregation service connection

- API Documentation (Swagger, OpenAPI)

- Enhance validation

- Pagination

## 💻 Endpoints

- Root Endpoint
  
  ```plantext
  GET /
  ```
  
  - Returns a simple version message.

- Get All Issues
  
  ```plaintext
  GET /issues  
  ```
  
  - Fetches all issues.

- Get Issue by ID
  
  ```plaintext
  GET /issues/{id}
  ```
  
  - Fetches a single issue by its ID.

- Create a New Issue
  
  ```plaintext
  POST /issues
  ```
  
  - Creates a new issue. Requires a JSON payload:
 
    ```json
    {
        "title": "Issue Title",
        "body": "Issue Body",
        "status": 0
    }
    ```
    
- Update Issue Status

  ```plaintext
  PUT /issues/{id}/status
  ```
  
  - Updates the status of an issue. Requires a JSON payload:
    ```json
    {
       "title": "Updated Title",
        "body": "Updated Body",
        "status": 1
    }
    ```

- Delete an Issue

  ```plaintext
  DELETE /issues/{id}
  ```
  
  - Deletes an issue by its ID.

## Running the API

- To run the API locally:

`dotnet run`

## Requirements

`.NET 8 SDK`

## License

This project is open-source and available under the MIT License.
