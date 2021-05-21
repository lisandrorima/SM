using AutoMapper;
using Moq;
using SM.Bll;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
using SM.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Linq;
using SM.WebAPI;

namespace SM.Tests
{
	public class UserBllTest
	{

		private readonly BllUser _sut;

		private readonly Mock<IDaoUser> _DaoUser = new Mock<IDaoUser>();



		public UserBllTest()
		{
			var config = new MapperConfiguration(cfg => {
				cfg.AddProfile<MappingProfile>();
			});

			var _automapper = config.CreateMapper();
			_sut = new BllUser(_DaoUser.Object, _automapper);
		}

		[Fact]
		public async Task GetAll_ShouldReturnListUsers()
		{
			//arrange
			List<User> UserList = new List<User>
			{
				new User { ID = 1,
				 Name = "Name1",
				 Surname = "Surname2",
				 PersonalID = 12345678,
				 WalletAddress = "ax00123fjsHS_",
				 Email = "asd@asd.com",
				 Password = "password1"
			},
				new User { ID = 2,
				 Name = "Name2",
				 Surname = "Surname2",
				 PersonalID = 23456789,
				 WalletAddress = "Abnis908X_2",
				 Email = "mail@hotmail.com",
				 Password = "password3"
			},

		};


			_DaoUser.Setup(x => x.GetAll()).ReturnsAsync(UserList);


			//act



			IEnumerable<DTOUser> DTOuserlist = await _sut.GetAll();

			//assert
			DTOuserlist.Select(c => c.Name).Should().Equal(UserList.Select(c => c.Name));
			DTOuserlist.Select(c => c.Surname).Should().Equal(UserList.Select(c => c.Surname));
			DTOuserlist.Select(c => c.PersonalID).Should().Equal(UserList.Select(c => c.PersonalID));
			DTOuserlist.Select(c => c.WalletAddress).Should().Equal(UserList.Select(c => c.WalletAddress));
			DTOuserlist.Select(c => c.Password).Should().Equal(UserList.Select(c => c.Password));

		}

		[Fact]
		public async Task Login_ShouldReturnExistingUserDTO()
		{
			//arrange
			DTOLogin DtoLoginUser = new DTOLogin
			{
				Email = "asd@asd.com",
				Password = "password1"

			};

			User user = new User
			{
				ID = 1,
				Name = "Name1",
				Surname = "Surname2",
				PersonalID = 12345678,
				WalletAddress = "ax00123fjsHS_",
				Email = "asd@asd.com",
				Password = BCrypt.Net.BCrypt.HashPassword("password1")

			};

			var dto = new DTOUser();
			_DaoUser.Setup(m => m.GetByEmail(DtoLoginUser.Email)).ReturnsAsync(user);

			//act

			dto = await _sut.Login(DtoLoginUser);

			//assert
			dto.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());


		}

		[Fact]
		public async Task Login_ShouldReturnEmptyWhenUserNotExist()
		{
			//arrange
			DTOLogin DtoLoginUser = new DTOLogin
			{
				Email = "lisandro@asd.com",
				Password = "password1"

			};

			User user = new User
			{
				ID = 1,
				Name = "Name1",
				Surname = "Surname2",
				PersonalID = 12345678,
				WalletAddress = "ax00123fjsHS_",
				Email = "asd@asd.com",
				Password = BCrypt.Net.BCrypt.HashPassword("password1")

			};

			var dto = new DTOUser();
			_DaoUser.Setup(m => m.GetByEmail(DtoLoginUser.Email)).ReturnsAsync(user);

			//act

			dto = await _sut.Login(DtoLoginUser);

			//assert
			dto.Should().BeOfType<DTOUser>();
			dto.Surname.Should().BeNull();
			dto.Name.Should().BeNull();
			dto.WalletAddress.Should().BeNull();
			dto.PersonalID.Should().Be(0);
			dto.Password.Should().BeNull();
			dto.Email.Should().BeNull();
		}

		[Fact]
		public async Task Login_ShouldReturnEmptyWhenUserExistsButIncorrectPassword()
		{
			//arrange
			DTOLogin DtoLoginUser = new DTOLogin
			{
				Email = "asd@asd.com",
				Password = "password2"

			};

			User user = new User
			{
				ID = 1,
				Name = "Name1",
				Surname = "Surname2",
				PersonalID = 12345678,
				WalletAddress = "ax00123fjsHS_",
				Email = "asd@asd.com",
				Password = BCrypt.Net.BCrypt.HashPassword("password1")

			};

			var dto = new DTOUser();
			_DaoUser.Setup(m => m.GetByEmail(DtoLoginUser.Email)).ReturnsAsync(user);

			//act

			dto = await _sut.Login(DtoLoginUser);

			//assert
			dto.Should().BeOfType<DTOUser>();
			dto.Surname.Should().BeNull();
			dto.Name.Should().BeNull();
			dto.WalletAddress.Should().BeNull();
			dto.PersonalID.Should().Be(0);
			dto.Password.Should().BeNull();
			dto.Email.Should().BeNull();
		}

		[Fact]
		public async Task Register_ShouldReturnUserWhenSucces()
		{
			//arrange
			DTOUser dtoUser = new DTOUser
			{
				
				Name = "Name1",
				Surname = "Surname2",
				PersonalID = 12345678,
				WalletAddress = "ax00123fjsHS_",
				Email = "asd@asd.com",
				Password = "password1"

			};

		

			User userRegistrado = new User
			{
				ID = 1,
				Name = "Name1",
				Surname = "Surname2",
				PersonalID = 12345678,
				WalletAddress = "ax00123fjsHS_",
				Email = "asd@asd.com",
				Password = BCrypt.Net.BCrypt.HashPassword("password1")

			};

			_DaoUser.Setup(m => m.Register(It.IsAny<User>())).ReturnsAsync(userRegistrado);


			//Act
			var userFromDDBB = new User();
			userFromDDBB = await _sut.Register(dtoUser);

			//assert

			userFromDDBB.Should().BeOfType<User>();
			userFromDDBB.Should().BeEquivalentTo(userRegistrado);
		}
	}

}
