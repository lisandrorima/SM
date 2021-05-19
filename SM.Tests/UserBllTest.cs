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

namespace SM.Tests
{
	public class UserBllTest
	{

		private readonly BllUser _sut;

		private readonly Mock<IDaoUser> _DaoUser = new Mock<IDaoUser>();
		private readonly Mock<IMapper> _automapper = new Mock<IMapper>();

		public UserBllTest()
		{
			_sut = new BllUser(_DaoUser.Object, _automapper.Object);
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
	}

}
