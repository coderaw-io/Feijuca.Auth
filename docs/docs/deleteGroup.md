### ⚙️ Endpoint specification

##### Method: POST

##### Path: /group/{id}

##### Summary:

Deletes an existing group from the specified keycloak realm.

##### Responses

| Code | Description                                                                                           |
| ---- | ----------------------------------------------------------------------------------------------------- |
| 204  | The group was successfully deleted.                                                                   |
| 400  | The request was invalid or could not be processed.                                                    |
| 401  | The request lacks valid authentication credentials.                                                   |
| 403  | The request was understood, but the server is refusing to fulfill it due to insufficient permissions. |
| 500  | An internal server error occurred during the processing of the request.                               |

##### Header

| Name   | Located in | Description                                                                        | Required | Schema |
| ------ | ---------- | ---------------------------------------------------------------------------------- | -------- | ------ |
| id     | path       | The unique identifier of the group to be deleted.                                  | Yes      | Guid   |
| Tenant | header     | The tenant identifier used to filter the clients within a specific Keycloak realm. | Yes      | string |

##### Definition

![Endpoint definition](https://res.cloudinary.com/dd7cforjd/image/upload/uwvhk9p9vviypghr8wbd.jpg "Endpoint definition")

### 📝 How to Use the Endpoint

1. **Tenant Identification**:

   - The term _tenant_ in Feijuca represents the **realm name** within Keycloak where you’ll be performing actions.
   - You must specify the tenant name in the **HTTP header** to proceed.

2. **Id Unique Identifier**:
   - Add the unique identifier that represents the group to be deleted.
