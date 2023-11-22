using NetCoreTDD.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreTDD.UnitTests.Fixtures
{
    public static class UsersFixture
    {
        public static List<User> GetTestUsers()=> new List<User>()
        {
            new User()
            {
                Name = "Test 0",
                Email = "test.user@test.com",
                Address = new Address()
                {
                    Street="123",
                    City="Somewhere0",
                    ZipCode = "123"
                }
            },
            new User()
            {
                Name = "Test 1",
                Email = "test1.user@test.com",
                Address = new Address()
                {
                    Street="1234",
                    City="Somewhere1",
                    ZipCode = "1234"
                }
            },
            new User()
            {
                Name = "Test 2",
                Email = "test2.user@test.com",
                Address = new Address()
                {
                    Street="12345",
                    City="Somewhere2",
                    ZipCode = "12345"
                }
            },
            new User()
            {
                Name = "Test 3",
                Email = "test3.user@test.com",
                Address = new Address()
                {
                    Street="123456",
                    City="Somewhere3",
                    ZipCode = "123456"
                }
            },
        };
    }
}
