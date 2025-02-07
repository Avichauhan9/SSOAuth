using SSO_Backend.Context;

namespace SSO_Backend.Services;

public class BaseService(AppDBContext context, UserInfo user) : AuthService(user)
{
    protected readonly AppDBContext _context = context;
}
