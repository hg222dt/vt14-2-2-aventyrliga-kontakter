using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace contacts.Model
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage="Ange ett förnamn")]
        [StringLength(50, ErrorMessage="Förnamnet får vara max 50 tecken långt.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ange ett efternamn")]
        [StringLength(50, ErrorMessage="Efternamnet får vara max 50 tecken långt.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ange en e-postadress")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage="Ange en tillåten e-postadress")]
        [StringLength(50, ErrorMessage="E-postadressen får vara max 50 tecken lång.")]
        public string Email { get; set; }
    }
}