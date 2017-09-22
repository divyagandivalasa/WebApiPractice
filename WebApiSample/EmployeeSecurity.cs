using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeDataAccess;

namespace WebApiSample
{
    public class EmployeeSecurity
    {
        public static bool Login(string username, string password)
        {
            using (PracticeEntities entities = new PracticeEntities())
            {
                return
                    entities.Users.Any(
                        user =>
                            user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                            user.Password == password);
            }
        }
    }
}