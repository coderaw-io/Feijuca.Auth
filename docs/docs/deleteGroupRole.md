### ⚙️ Endpoint specification

##### Method: DELETE

##### Path: /group/{groupid}/role/{roleid}

##### Summary:

Remove a role from a specific group in the specified Keycloak realm.

##### Responses

| Code | Description                                                                                           |
| ---- | ----------------------------------------------------------------------------------------------------- |
| 204  | The role was successfully removed from the group.                                                     |
| 400  | The request was invalid or could not be processed.                                                    |
| 401  | The request lacks valid authentication credentials.                                                   |
| 403  | The request was understood, but the server is refusing to fulfill it due to insufficient permissions. |
| 500  | An internal server error occurred during the processing of the request.                               |

##### Header

| Name    | Located in | Description                                                                        | Required | Schema |
| ------- | ---------- | ---------------------------------------------------------------------------------- | -------- | ------ |
| groupid | patch      | The unique identifier of the group from which the role will be removed.            | Yes      | Guid   |
| roleid  | patch      | The unique identifier of the role to be removed from the group.                    | Yes      | Guid   |
| Tenant  | header     | The tenant identifier used to filter the clients within a specific Keycloak realm. | Yes      | string |

##### Definition

![Endpoint definition](https://res.cloudinary.com/dd7cforjd/image/upload/mjzrwnuj9ljdcihfbvdj.jpg "Endpoint definition")

### 📝 How to Use the Endpoint

1. **Tenant Identification**:
   - The term _tenant_ in Feijuca represents the **realm name** within Keycloak where you’ll be performing actions.
   - You must specify the tenant name in the **HTTP header** to proceed.
2. **Id Unique group identification**:
   - The term _groupid_ represents the **unique identifier of the group** whose roles are being retrieved.
   - You must specify the Group Id in the **HTTP path** to proceed.
3. **Id Unique rule identification**:
   - The term _id_ represents the **unique identifier of the role** whose role is being deleted from the group.
   - You must specify the Group Id in the **HTTP path** to proceed.
