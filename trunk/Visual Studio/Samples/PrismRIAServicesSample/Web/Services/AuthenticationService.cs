using System.Web.Ria;
using System.Web.Ria.ApplicationServices;


namespace Prism.Samples.Web
{
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User>
    {
    }

    public class User : UserBase
    {
        // NOTE: Profile properties can be added for use in Silverlight application.
        // To enable profiles, edit the appropriate section of web.config file.

        // public string MyProfileProperty { get; set; }
    }

}




