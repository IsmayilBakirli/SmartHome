//using FluentValidation;
//using SmartHome.Application.DTOs.User;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SmartHome.Application.Validators.User
//{
//    public class CreateUserValidator:AbstractValidator<CreateUserDto>
//    {
//        public CreateUserValidator()
//        {
//            RuleFor(x => x.FirstName)
//                .NotEmpty().WithMessage("First name is required")
//                .MaximumLength(20).WithMessage("First name cannot exceed 20 characters.");
//            RuleFor(x => x.LastName)
//                 .NotEmpty().WithMessage("Last name is required")
//                 .MaximumLength(20).WithMessage("Last name cannot exceed 20 characters.");
//            RuleFor(x => x.UserName)
//               .NotEmpty().WithMessage("Username  is required")
//                 .MaximumLength(20).WithMessage("Last name cannot exceed 20 characters.");
//            RuleFor(x => x.Email)
//               .NotEmpty().WithMessage("Email  is required")
//               .EmailAddress().WithMessage("Invalid email format");
//            RuleFor(x => x.Password)
//                 .NotEmpty().WithMessage("Password  is required");
//        }
//    }
//}
