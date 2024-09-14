﻿using TokenManager.Application.Responses;
using TokenManager.Domain.Entities;

namespace TokenManager.Application.Mappers
{
    public static class GroupMapper
    {
        public static IEnumerable<GroupResponse> ToResponse(this IEnumerable<Group> group)
        {
            return group.Select(x => new GroupResponse(x.Id, x.Name, x.Path));
        }
        
        public static GroupResponse ToResponse(this Group group)
        {
            return new GroupResponse(group.Id, group.Name, group.Path);
        }
    }
}