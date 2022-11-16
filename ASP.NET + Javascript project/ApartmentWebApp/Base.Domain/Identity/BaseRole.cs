using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace Base.Domain.Identity;

public class BaseRole : BaseRole<Guid>, IDomainEntityId
{
    public BaseRole() : base()
    {
        
    }
    
    public BaseRole(string roleName) : base(roleName)
    {
        
    }
}

public class BaseRole<Tkey> : IdentityRole<Tkey>, IDomainEntityId<Tkey>
    where Tkey : IEquatable<Tkey>
{
    public BaseRole() : base()
    {
        
    }
    
    public BaseRole(string roleName) : base(roleName)
    {
        
    }

    
}