using AutoMapper;
using NTierLoginJWT.Core.DTOs;
using NTierLoginJWT.DAL;

namespace NTierLoginJWT.API.Helpers
{
    public class UserProfile:Profile
    { public UserProfile() { CreateMap<User, usersDTO>();
        }
        
    }
}
