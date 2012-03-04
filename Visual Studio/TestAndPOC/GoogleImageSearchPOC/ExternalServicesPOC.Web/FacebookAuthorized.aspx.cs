using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExternalServicesPOC.Web
{
    public partial class FacebookAuthorized : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string queryString = this.ClientQueryString;
            if (queryString.Contains("code="))
            {
                string authCode = queryString.Split('=').Last();

                Page.ClientScript.RegisterStartupScript(typeof(FacebookAuthorized), "setAuthorizationToken", "<script>setAuthorizationToken('" + authCode + "')</script>");
            }
            else if (queryString.Contains("errorReason="))
            {
                string[] queryParameters = queryString.Split('&');
                
                string errorReason = queryParameters[0].Split('=').Last();
                string error = queryParameters[1].Split('=').Last();
                string errorDescription = queryParameters[2].Split('=').Last();

                ErrorMessage.Text = string.Format("Facebook not authorized: {0} - {1} ({2})", errorReason, error, errorDescription);
            }

            ErrorMessage.Text = ClientQueryString;
        }
    }
}