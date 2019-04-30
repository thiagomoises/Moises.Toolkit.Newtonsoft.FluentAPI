using System;
using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.FluentAPI.Tests
{
    public class UserTest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public UserStatus Status { get; set; }
        public string City { get; set; }
    }

    public enum UserStatus
    {
        NotConfirmed,
        Active,
        Deleted
    }
}
