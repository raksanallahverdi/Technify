using System.ComponentModel.DataAnnotations;

namespace Technify.Models.Account
{
    public class AccountForgetPasswordVM
    {
        [Required(ErrorMessage ="Mail Must be Entered!")]
        public string Email { get; set; }
    }
}
