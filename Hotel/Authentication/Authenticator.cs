﻿
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace Hotel.Authentication
{
    public class Authenticator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName != "peoplesite" && password != "password")
            {
                throw new FaultException("Invalid user and/or password");
            }
        }
    }
}