using System.Collections.Generic;
using System.Web.DomainServices;
using System.Web.Ria;
using System.Web.Ria.Data;
using System.Web.Security;

namespace Prism.Samples.Web
{
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        // NOTE: This is a sample code to get your application started. In the production code you would 
        // want to provide a mitigation against a denial of service attack by providing CAPTCHA 
        // control functionality or verifying user's email address.

        public void AddUser( UserInformation user )
        {
            MembershipCreateStatus createStatus;

            // NOTE: ASP.NET by default uses SQL Server Express to create the user database. 
            // CreateUser will fail if you do not have SQL Server Express installed.

            Membership.CreateUser( user.UserName, user.Password, user.Email, user.Question, user.Answer, true, null, out createStatus );
            if ( createStatus != MembershipCreateStatus.Success )
            {
                throw new DomainServiceException( ErrorCodeToString( createStatus ) );
            }
        }

        public IEnumerable<UserInformation> GetUsers()
        {
            return null;
        }

        private static string ErrorCodeToString( MembershipCreateStatus createStatus )
        {

            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes and add appropriate error handling.

            if ( createStatus == MembershipCreateStatus.DuplicateUserName )
            {
                return "Username already exists. Please enter a different user name and try again.";
            }

            return "Could not register the user, please verify the provided information and try again.";
        }
    }
}


