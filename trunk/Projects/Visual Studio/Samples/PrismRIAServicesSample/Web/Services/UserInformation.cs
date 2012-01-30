using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prism.Samples.Web
{
    public sealed class UserInformation
    {
        [Key]
        [Required]
        [Display( Name = "username" )]
        [RegularExpression( "^[a-zA-Z0-9_]*$", ErrorMessage = "Username must contain only alphanumeric characters." )]
        public string UserName { get; set; }

        [Required]
        [Display( Name = "password", Description = "Password should be at least 7 characters long and should contain at least one non-alphanumeric character." )]
        [RegularExpression( "^.*[^a-zA-Z0-9].*$", ErrorMessage = "Password should contain at least one non-alphanumeric character." )]
        [StringLength( 50, MinimumLength = 7, ErrorMessage = "Password should be at least 7 characters long." )]
        public string Password { get; set; }

        [Key]
        [Required]
        [Display( Name = "email" )]
        [RegularExpression( @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "E-mail address is invalid." )]
        public string Email { get; set; }

        [Required]
        [Display( Name = "security question" )]
        public string Question { get; set; }

        [Required]
        [Display( Name = "security answer" )]
        public string Answer { get; set; }
    }

}
