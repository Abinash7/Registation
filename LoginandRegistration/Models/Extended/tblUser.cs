using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginandRegistration.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class tblUser
    {
        public string ConfirmPassword { get; set; }
    }
    public class UserMetaData
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="First Name Required")]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name ="Date Of Birth")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DOB { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage ="Minimum 8 characters required")]       
        public string Password { get; set; }

        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }

}