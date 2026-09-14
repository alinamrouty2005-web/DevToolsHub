# DevToolsHub API Documentation

## 1. Overview
DevToolsHub is a .NET 10 Web API for backend/API developers. The API provides developer tools, projects, collections, saved API requests, favorites, history, subscriptions, plans and notifications.

## 2. Base URLs
- HTTPS: `https://localhost:7135`
- HTTP: `http://localhost:5026`

The included Postman collection uses HTTPS by default. If the ASP.NET Core development certificate is not trusted, trust the local development certificate or temporarily use the HTTP variable value.

## 3. Authentication
Authentication uses JWT Bearer tokens.

1. Call `POST /api/Auth/register`.
2. Call `POST /api/Auth/login`.
3. Copy the returned `token` and send it in `Authorization: Bearer <token>` for protected endpoints.

The Postman collection automatically saves the login token into the `token` collection variable.

### Register
`POST /api/Auth/register`
```json
{
  "fullName": "Test User",
  "email": "test@example.com",
  "password": "Test@123456"
}
```

### Login
`POST /api/Auth/login`
```json
{
  "email": "test@example.com",
  "password": "Test@123456"
}
```

Response contains `token` and `expiration`.

## 4. Authorization
- Normal authenticated endpoints require a valid JWT.
- Admin endpoints require the `Admin` role.
- Tools and Plans read endpoints are public.
- Registering a normal user assigns the `User` role in the application.
- No admin user is seeded because passwords are hashed during registration. For testing Admin endpoints, an existing user must be assigned the Admin role through the database or an authorized administrative workflow.

## 5. Common Validation Rules
DTO validation is performed automatically by ASP.NET Core `[ApiController]`. Common rules include required fields, email validation, minimum string lengths, maximum string lengths, numeric ranges, and API request status code range.

Validation response follows the project format:
```json
{
  "success": false,
  "message": "Validation failed.",
  "errors": { }
}
```

## 6. Endpoints

### System
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/health` | Public | Health check |

### Authentication
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/Auth/register` | Public | Register user |
| POST | `/api/Auth/login` | Public | Login and receive JWT |

### Users
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/User` | JWT | Get users |
| GET | `/api/User/{id}` | JWT | Get user by ID |
| GET | `/api/User/email?email=...` | JWT | Get user by email |
| POST | `/api/User` | JWT | Create user |
| PUT | `/api/User/{id}` | JWT | Update user |
| DELETE | `/api/User/{id}` | JWT | Delete user |

### Roles
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/Role` | JWT | Get all roles |
| GET | `/api/Role/{id}` | JWT | Get role by ID |
| GET | `/api/Role/name?name=User` | JWT | Get role by name |
| POST | `/api/Role` | Admin | Create role |
| PUT | `/api/Role/{id}` | Admin | Update role |
| DELETE | `/api/Role/{id}` | Admin | Delete role |

### User Roles
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/UserRole` | Admin | Get all user-role records |
| GET | `/api/UserRole/{userId}/{roleId}` | Admin | Get relationship |
| POST | `/api/UserRole` | Admin | Assign role to user |
| DELETE | `/api/UserRole/{userId}/{roleId}` | Admin | Remove role from user |

### Projects
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/Project` | JWT | Get projects |
| GET | `/api/Project/{id}` | JWT | Get project by ID |
| GET | `/api/Project/my` | JWT | Get current user's projects |
| GET | `/api/Project/search?name=API` | JWT | Search projects |
| GET | `/api/Project/paged?pageNumber=1&pageSize=10` | JWT | Pagination |
| POST | `/api/Project` | JWT | Create project |
| PUT | `/api/Project/{id}` | JWT | Update project |
| DELETE | `/api/Project/{id}` | JWT | Delete project |

### Tools
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/Tool` | Public | Get tools |
| GET | `/api/Tool/{id}` | Public | Get tool by ID |
| GET | `/api/Tool/name?name=JSON%20Formatter` | Public | Get by name |
| GET | `/api/Tool/search?name=JSON` | Public | Search tools |
| GET | `/api/Tool/category?category=JSON` | Public | Filter by category |
| GET | `/api/Tool/active` | Public | Get active tools |
| GET | `/api/Tool/paged?pageNumber=1&pageSize=10&search=JSON&category=JSON&sortBy=name&sortDescending=false` | Public | Pagination + search + filtering + sorting |
| POST | `/api/Tool` | Admin | Create tool |
| PUT | `/api/Tool/{id}` | Admin | Update tool |
| DELETE | `/api/Tool/{id}` | Admin | Delete tool |

### Tool History
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/ToolHistory` | Admin | Get all history |
| GET | `/api/ToolHistory/{id}` | JWT | Get history by ID |
| GET | `/api/ToolHistory/my` | JWT | Current user's history |
| GET | `/api/ToolHistory/tool/{toolId}` | JWT | History for a tool |
| GET | `/api/ToolHistory/project/{projectId}` | JWT | History for a project |
| GET | `/api/ToolHistory/paged?pageNumber=1&pageSize=10` | Admin | Paged history |
| POST | `/api/ToolHistory` | JWT | Create history record |
| PUT | `/api/ToolHistory/{id}` | JWT | Update history record |
| DELETE | `/api/ToolHistory/{id}` | JWT | Delete history record |

### Favorites
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/Favorite` | Admin | Get all favorites |
| GET | `/api/Favorite/{id}` | JWT | Get favorite by ID |
| GET | `/api/Favorite/my` | JWT | Current user's favorites |
| GET | `/api/Favorite/tool/{toolId}` | JWT | Get favorite for a tool |
| POST | `/api/Favorite` | JWT | Add favorite |
| DELETE | `/api/Favorite/{id}` | JWT | Delete favorite |

### Collections
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/Collection` | Admin | Get all collections |
| GET | `/api/Collection/{id}` | JWT | Get collection by ID |
| GET | `/api/Collection/my` | JWT | Current user's collections |
| GET | `/api/Collection/search?name=My` | JWT | Search collections |
| GET | `/api/Collection/paged?pageNumber=1&pageSize=10` | JWT | Pagination |
| POST | `/api/Collection` | JWT | Create collection |
| PUT | `/api/Collection/{id}` | JWT | Update collection |
| DELETE | `/api/Collection/{id}` | JWT | Delete collection |

### Collection Items
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/CollectionItem` | Admin | Get all collection items |
| GET | `/api/CollectionItem/{collectionId}/{toolId}` | JWT | Get item by composite key |
| GET | `/api/CollectionItem/collection/{collectionId}` | JWT | Get items by collection |
| GET | `/api/CollectionItem/tool/{toolId}` | JWT | Get items by tool |
| POST | `/api/CollectionItem` | JWT | Add tool to collection |
| DELETE | `/api/CollectionItem/{collectionId}/{toolId}` | JWT | Remove tool from collection |

### API Requests
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/ApiRequest` | JWT | Get current user's saved requests |
| GET | `/api/ApiRequest/{id}` | JWT | Get request by ID |
| GET | `/api/ApiRequest/method/{method}` | JWT | Filter by HTTP method |
| GET | `/api/ApiRequest/project/{projectId}` | JWT | Get requests by project |
| GET | `/api/ApiRequest/page?pageNumber=1&pageSize=10` | JWT | Pagination |
| POST | `/api/ApiRequest` | JWT | Save API request |
| PUT | `/api/ApiRequest/{id}` | JWT | Update API request |
| DELETE | `/api/ApiRequest/{id}` | JWT | Delete API request |

### Plans
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/Plan` | Public | Get plans |
| GET | `/api/Plan/active` | Public | Get active plans |
| GET | `/api/Plan/{id}` | Public | Get plan by ID |
| GET | `/api/Plan/search?name=Pro` | Public | Search plans |
| POST | `/api/Plan` | Admin | Create plan |
| PUT | `/api/Plan/{id}` | Admin | Update plan |
| DELETE | `/api/Plan/{id}` | Admin | Delete plan |

### Subscriptions
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/Subscription` | Admin | Get all subscriptions |
| GET | `/api/Subscription/my` | JWT | Get current user's subscription |
| GET | `/api/Subscription/{id}` | Admin | Get subscription by ID |
| POST | `/api/Subscription` | JWT | Create subscription for current user |
| PUT | `/api/Subscription/{id}` | Admin | Update subscription |
| DELETE | `/api/Subscription/{id}` | Admin | Delete subscription |

### Notifications
| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/Notification` | JWT | Get notifications |
| GET | `/api/Notification/{id}` | JWT | Get notification by ID |
| GET | `/api/Notification/my` | JWT | Current user's notifications |
| GET | `/api/Notification/unread` | JWT | Current user's unread notifications |
| POST | `/api/Notification` | Admin | Create notification |
| PUT | `/api/Notification/{id}` | JWT | Update notification |
| DELETE | `/api/Notification/{id}` | Admin | Delete notification |

## 7. Example Request Bodies

### Project
```json
{
  "name": "My API Project",
  "description": "Backend API project"
}
```

### Tool
```json
{
  "name": "XML Formatter",
  "description": "Formats XML",
  "category": "Formatting",
  "isActive": true
}
```

### Collection
```json
{
  "name": "My API Tools",
  "description": "Useful developer tools"
}
```

### Collection Item
```json
{
  "collectionId": 1,
  "toolId": 1
}
```

### Favorite
```json
{
  "toolId": 1
}
```

### API Request
```json
{
  "projectId": null,
  "name": "Get Users",
  "method": "GET",
  "url": "https://example.com/api/users",
  "headers": "{}",
  "body": null,
  "statusCode": 200,
  "responseBody": "{}"
}
```

### Subscription
```json
{
  "planId": 1
}
```

### Notification
```json
{
  "userId": 1,
  "title": "Welcome",
  "message": "Welcome to DevToolsHub."
}
```

## 8. Suggested Testing Order
1. Run the API.
2. Verify `GET /health`.
3. Register a user.
4. Login and store the JWT.
5. Verify public Tools and Plans endpoints.
6. Create a Project.
7. Create a Collection.
8. Add a Tool to the Collection.
9. Add a Favorite.
10. Create Tool History.
11. Create an API Request.
12. Create a Subscription using an existing Plan.
13. Test search, filtering, sorting and pagination.
14. Test Admin-only endpoints with an Admin JWT.
15. Test invalid DTO data to verify validation handling.

## 9. Postman Collection
The project includes `Postman/DevToolsHub.postman_collection.json`. Import it into Postman. Set the `baseUrl` collection variable if your local HTTPS/HTTP port differs.

## 10. Notes
- The current project uses DTOs and manual DTO construction in controllers; no AutoMapper/Mapping layer is required.
- JWT expiration is controlled by the `Jwt:DurationInMinutes` configuration.
- The API uses global exception middleware and logging.
- Database migrations are already applied in the development environment according to the current project state.
