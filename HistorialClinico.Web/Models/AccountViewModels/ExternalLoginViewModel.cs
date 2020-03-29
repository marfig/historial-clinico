#region Using

using System.ComponentModel.DataAnnotations;

#endregion

namespace HistorialClinico.Web.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
