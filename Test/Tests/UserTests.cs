using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Sat.Recruitment.DataAccess.Contracts;
using Sat.Recruitment.DTO.Responses;
using Sat.Recruitment.Services;
using Sat.Recruitment.Services.Test.Fake;
using System.Threading.Tasks;


namespace Sat.Recruitment.Services.Test
{
    public class UserTests
    {

        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
       
        public UserTests()
        {
            _userService = new UserService(_userRepositoryMock.Object, _configurationMock.Object);
        }

        [Test]
        public void TestInsertUserUnique()
        {
            //Arrenge
            var usersFake = FakeUserDTO.GetSome();
            var userFake = FakeUserDTO.GetOneUnique();
          

            _userRepositoryMock.Setup(s => s.GetAllUsers()).Returns(Task.FromResult(usersFake));
            _userRepositoryMock.Setup(s => s.CreateUser(userFake)).Returns(Task.FromResult(true));
            mockSection.Setup(x => x.Value).Returns("0,12");
            _configurationMock.Setup(x => x.GetSection(It.Is<string>(k => k == "NormalUser>100"))).Returns(mockSection.Object);
           
            //Act
            UserResponse userResponse = _userService.CreateUser(userFake).Result;

            //Assert
            Assert.AreEqual(true, userResponse.IsSuccess);
        }

        [Test]
        public void TestInsertUserRepeat()
        {
            //Arrenge
            var usersFake = FakeUserDTO.GetSome();
            var userFake = FakeUserDTO.GetOneRepeat();


            _userRepositoryMock.Setup(s => s.GetAllUsers()).Returns(Task.FromResult(usersFake));
            _userRepositoryMock.Setup(s => s.CreateUser(userFake)).Returns(Task.FromResult(false));
            mockSection.Setup(x => x.Value).Returns("0,12");
            _configurationMock.Setup(x => x.GetSection(It.Is<string>(k => k == "NormalUser>100"))).Returns(mockSection.Object);

            //Act
            UserResponse userResponse = _userService.CreateUser(userFake).Result;

            //Assert
            Assert.AreEqual(false, userResponse.IsSuccess);
        }

    }
}