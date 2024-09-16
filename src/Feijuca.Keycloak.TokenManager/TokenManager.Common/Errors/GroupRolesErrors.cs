﻿using TokenManager.Common.Models;

namespace TokenManager.Common.Errors
{
    public static class GroupRolesErrors
    {
        public static string TechnicalMessage { get; private set; } = "";

        public static Error ErrorAddRoleToGroup => new(
            "GroupRoles.ErrorAddRoleToGroup",
            $"An error occurred while trying adding a new role to the group: {TechnicalMessage}"
        );
        
        public static Error ErrorGetGroupRoles => new(
            "GroupRoles.ErrorGetGroupRoles",
            $"An error occurred while trying adding get group roles: {TechnicalMessage}"
        );
    }
}