using System;
using System.ComponentModel.DataAnnotations;

namespace GG.Models
{
    //Validation attribute for todays date or future date
    public class RestrictedDate : ValidationAttribute
    {
        public override bool IsValid(object date)
        {
            DateTime inputDate = (DateTime)date;
            return inputDate >= DateTime.Today;
        }
    }
}
