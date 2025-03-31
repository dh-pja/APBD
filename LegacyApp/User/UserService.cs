using System;
using LegacyApp.Interfaces;
using LegacyApp.Utils;

namespace LegacyApp
{
    public class UserService : IUserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!NameValidator.Validate(firstName) || !NameValidator.Validate(lastName)
                || !EmailValidator.Validate(email) || !AgeValidator.Validate(dateOfBirth))
            {
                return false;
            }

            var client = new ClientRepository().GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            switch (client.Type)
            {
                case ClientType.VeryImportantClient:
                    user.HasCreditLimit = false;
                    break;
                case ClientType.ImportantClient:
                    using (var userCreditService = new UserCreditService())
                    {
                        int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                        creditLimit = creditLimit * 2;
                        user.CreditLimit = creditLimit;
                    }
                    break;
                case ClientType.NormalClient:
                    user.HasCreditLimit = true;
                    using (var userCreditService = new UserCreditService())
                    {
                        int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                        user.CreditLimit = creditLimit;
                    }
                    break;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
    }
}
