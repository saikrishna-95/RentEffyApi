using FluentValidation;
using Renteffy.Application.DTOs.Registration.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Validations.Registration
{
    public class UserRegistrationValidator: AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        }
    }
}
