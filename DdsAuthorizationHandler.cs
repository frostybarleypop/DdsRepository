using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DDSPatient
{

    public class DdsAuthorization : IAuthorizationRequirement
    {
    }

    public class DdsAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            context.Succeed(requirement);
        }
    }
}
