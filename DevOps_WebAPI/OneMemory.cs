using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOps_WebAPI
{
    public class OneMemory
    {
        public OneMemory()
        {
            this.UsersList = _userList;
        }

        public List<OneModel> UsersList { get; set; }

        private static readonly List<OneModel> _userList = new List<OneModel>()
        {
            new OneModel()
            {
               UserId = 1,
               UserName = "User One",
               EmailAddress = "userone@nagp.com",
               PhoneNo = "123456712"
            },new OneModel()
            {
               UserId = 2,
               UserName = "User Two",
               EmailAddress = "usertwo@nagp.com",
               PhoneNo = "123456734"
            },new OneModel()
            {
               UserId = 3,
               UserName = "User Three",
               EmailAddress = "userthree@nagp.com",
               PhoneNo = "123456756"
            },new OneModel()
            {
               UserId = 4,
               UserName = "User Four",
               EmailAddress = "userfour@nagp.com",
               PhoneNo = "123456778"
            },new OneModel()
            {
               UserId = 5,
               UserName = "User Five",
               EmailAddress = "userfive@nagp.com",
               PhoneNo = "123456790"
            },
        };
    }

}
