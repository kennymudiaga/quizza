using AutoMapper;
using JwtFactory;
using Quizza.Users.Application.Extensions;
using Quizza.Users.Application.Options;
using Quizza.Users.Domain.Models;
using Quizza.Users.Domain.Models.Entities;

namespace Quizza.Users.Application.Contracts;

public abstract class LoginManager
{
    protected readonly JwtProvider JwtProvider;
    protected readonly UserPolicyOptions UserPolicy;
    protected readonly IMapper Mapper;

    public LoginManager(JwtProvider jwtProvider, IMapper mapper, UserPolicyOptions userPolicy)
    {
        JwtProvider = jwtProvider;        
        Mapper = mapper;
        UserPolicy = userPolicy;
    }

    protected LoginResponse CreateLogin(UserProfile userProfile)
    {
        var loginResponse = Mapper.Map<LoginResponse>(userProfile);
        var expiryDate = DateTime.UtcNow.AddMinutes(UserPolicy.SessionTimeout);
        var token = JwtProvider.GetUserToken(userProfile.GetClaims(), expiryDate);

        return loginResponse with { Token = token, ExpiryDate = expiryDate };
    }
}
