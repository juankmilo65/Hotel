using Hotel.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel.ExampleData
{
    public class AccountMock
    {
        private List<AccountDto> listAccounts = new List<AccountDto>();

        public AccountMock()
        {
            listAccounts.Add(new AccountDto { Password = "123", UserName = "test1" });
            listAccounts.Add(new AccountDto { Password = "123", UserName = "test2" });
            listAccounts.Add(new AccountDto { Password = "123", UserName = "test3" });
        }

        public bool Login(string username, string password)
        {
            return listAccounts.Count(acc => acc.UserName.Equals(username) && acc.Password.Equals(password)) > 0;
        }
    }
}