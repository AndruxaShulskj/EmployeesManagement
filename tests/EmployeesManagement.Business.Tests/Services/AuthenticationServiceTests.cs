using AutoMapper;
using EmployeesManagement.Business.Exceptions;
using EmployeesManagement.Business.Interfaces;
using EmployeesManagement.Business.Models;
using EmployeesManagement.Business.Services;
using EmployeesManagement.Common.Interfaces;
using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.Business.Tests.Services;

public class AuthenticationServiceTests : TestBase<AuthenticationService>
{
    private const string TEST_HASH = "test-hash";

    public AuthenticationServiceTests()
    {
        AutoMocker
            .GetMock<IUserRepository>()
            .Setup(x => x.GetUserByLoginAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserEntity() { Hash = TEST_HASH });

        AutoMocker
            .GetMock<IEncryptor>()
            .Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(TEST_HASH);

        AutoMocker
            .GetMock<IMapper>()
            .Setup(x => x.Map<User>(It.IsAny<object>()))
            .Returns(new User());
    }

    [Fact]
    public async Task LoginAsyncShouldReturnUserSuccess()
    {
        //Arrange
        var sut = CreateTestSubject();

        //Act
        var user = await sut.LoginAsync(new UserCredential("login", "password"));

        //Assert
        Assert.NotNull(user);
    }

    [Fact]
    public async Task LoginAsyncShouldReturnExceptionWhenUserNotExist()
    {
        //Arrange
        AutoMocker
            .GetMock<IUserRepository>()
            .Setup(x => x.GetUserByLoginAsync(It.IsAny<string>()))
            .ReturnsAsync(null as UserEntity);

        var sut = CreateTestSubject();

        //Act
        var task = sut.LoginAsync(new UserCredential("login", "password"));

        //Assert
        await Assert.ThrowsAsync<UserNotExistOrWrongPasswordException>(() => task);
    }
    
    [Fact]
    public async Task LoginAsyncShouldReturnExceptionWhenPasswordIncorrect()
    {
        //Arrange
        AutoMocker
            .GetMock<IUserRepository>()
            .Setup(x => x.GetUserByLoginAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserEntity() {Hash = "1234"});

        var sut = CreateTestSubject();

        //Act
        var task = sut.LoginAsync(new UserCredential("login", "password"));

        //Assert
        await Assert.ThrowsAsync<UserNotExistOrWrongPasswordException>(() => task);
    }
}