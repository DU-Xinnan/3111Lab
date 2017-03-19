using SinExWebApp20256461.Models;

namespace SinExWebApp20256461.ViewModels
{
    public class RegisterCustomerViewModel
    {
        public PersonalShippingAccount PersonalInformation { get; set; }
        public BusinessShippingAccount BusinessInformation { get; set; }
        public RegisterViewModel LoginInformation { get; set; }
    }
}