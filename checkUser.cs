using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReload
{
    public class checkUser
    {

        public string Login { get; set; } /*свойство Login*/

        public bool IsAdmin { get; } /*логическое свойство IsAdmin*/
        public string Status => IsAdmin ? "Admin" : "User"; /*Передаем значение тернарной операции, если заходит админ то выводим надпись Admin*/

        public checkUser(string login, bool isAdmin)
        {
            Login = login.Trim(); /*присваиваем свойство Login саму строку которую мы передаем в методе checkUser*/
            IsAdmin = isAdmin;
        }

    }
}
