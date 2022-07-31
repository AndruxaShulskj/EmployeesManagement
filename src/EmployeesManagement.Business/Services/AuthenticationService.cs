using AutoMapper;
using EmployeesManagement.Business.Exceptions;
using EmployeesManagement.Business.Interfaces;
using EmployeesManagement.Business.Models;
using EmployeesManagement.Common.Interfaces;

namespace EmployeesManagement.Business.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptor _encryptor;
    private readonly IMapper _mapper;
    
    public AuthenticationService(
        IUserRepository userRepository,
        IEncryptor encryptor,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _encryptor = encryptor;
        _mapper = mapper;
    }

    public async Task<User> LoginAsync(UserCredential model)
    {
        var user = await _userRepository.GetUserByLoginAsync(model.login);
        if (user == null)
        {
            throw new UserNotExistOrWrongPasswordException();
        }

        var hash = _encryptor.GetHash(model.password, user.Salt);

        if (!user.Hash.Equals(hash))
        {
            throw new UserNotExistOrWrongPasswordException();
        }

        return _mapper.Map<User>(user);
    }
}