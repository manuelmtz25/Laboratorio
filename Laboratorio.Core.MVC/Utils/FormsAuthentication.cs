using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio.Core.MVC.Utils
{
    public class FormsAuthentication
    {
        //public void SignIn(Models.User user, bool createPersistentCookie)
        public void SignIn()
        {
            //var authTicket = new System.Web.Security.FormsAuthenticationTicket(1,
            //                                                                   user.GetFullName(), DateTime.Now,
            //                                                                   DateTime.Now.AddMinutes(60),
            //                                                                   createPersistentCookie,
            //                                                                   user.UserId.ToString());

            var authTicket = new System.Web.Security.FormsAuthenticationTicket(1,
                                                                               "Manuel Martínez", DateTime.Now,
                                                                               DateTime.Now.AddMinutes(60),
                                                                               true,
                                                                               "mamrtineza");

            var encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encryptedTicket);

            //if (createPersistentCookie)
            //{
            //    authCookie.Expires = authTicket.Expiration;
            //}
            authCookie.Expires = authTicket.Expiration;
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public void SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();

        }
    }
}