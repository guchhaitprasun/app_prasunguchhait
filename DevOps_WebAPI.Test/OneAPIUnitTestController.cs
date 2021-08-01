using DevOps_WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DevOps_WebAPI.Test
{
    public class OneAPIUnitTestController
    {
        public static IOptions<OneMemory> option;
        static OneAPIUnitTestController()
        {
            option = Options.Create(new OneMemory());

        }

        [Fact]
        public void Task_GetListOfUsers_Return_OkResult()
        {
            //Arrange  
            OneAPIController controller = new OneAPIController(option);

            //Act  
            IActionResult result = controller.Get();
            var statusResult = result as OkObjectResult;

            //Assert  
            Assert.NotNull(statusResult);
            Assert.Equal(200, statusResult.StatusCode);
        }

        [Fact]
        public void Task_GetUserById_Return_OkResult()
        {
            //Arrange  
            OneAPIController controller = new OneAPIController(option);
            int userId = 2;

            //Act  
            IActionResult result = controller.Get(userId);
            var statusResult = result as OkObjectResult;

            //Assert  
            Assert.NotNull(statusResult);
            Assert.Equal(200, statusResult.StatusCode);
        }

        [Fact]
        public void Task_PostUser_Return_OkResult()
        {
            //Arrange  
            OneAPIController controller = new OneAPIController(option);
            OneModel user = new OneModel();
            user.UserId = 3000;
            user.UserName = "NAGP User";
            user.EmailAddress = "NAGP2021@nagp.com";
            user.PhoneNo = "999999999";

            //Act  
            IActionResult result = controller.Post(user);
            var statusResult = result as OkObjectResult;

            //Assert  
            Assert.NotNull(statusResult);
            Assert.Equal(200, statusResult.StatusCode);
        }

        [Fact]
        public void Task_UpdateUser_Return_OkResult()
        {
            //Arrange  
            OneAPIController controller = new OneAPIController(option);
            OneModel user = new OneModel();
            user.UserId = 1;
            user.UserName = "NAGP User";
            user.EmailAddress = "2021Nagp@nagp.com";
            user.PhoneNo = "999999999";

            //Act  
            IActionResult result = controller.Put(user.UserId, user);
            var statusResult = result as OkObjectResult;

            //Assert  
            Assert.NotNull(statusResult);
            Assert.Equal(200, statusResult.StatusCode);
        }

        [Fact]
        public void Task_DeleteUser_Return_OkResult()
        {
            //Arrange  
            OneAPIController controller = new OneAPIController(option);
            var UserId = 3000;

            //Act  
            IActionResult result = controller.Delete(UserId);
            var statusResult = result as OkObjectResult;

            //Assert  
            Assert.NotNull(statusResult);
            Assert.Equal(200, statusResult.StatusCode);
        }
    }
}
