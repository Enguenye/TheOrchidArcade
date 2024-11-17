using System.ComponentModel.DataAnnotations;

namespace TheOrchidArchade.Models
{
    public class Verify2FAViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Authentication Code")]
        public string Code { get; set; }
    }

}
