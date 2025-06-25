//using FluentValidation;
//using SmartHome.Application.DTOs.Location;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SmartHome.Application.Validators.Location
//{
//    public class CreateLocationValidator : AbstractValidator<LocationCreateDto>
//    {
//        public CreateLocationValidator()
//        {
//            RuleFor(x => x.Name)
//                .NotEmpty().WithMessage("Location name is required")
//                .MaximumLength(50).WithMessage("Location name cannot exceed 50 characters.");
//            RuleFor(x => x.Zone)
//                 .NotEmpty().WithMessage("Zone name is required")
//                 .MaximumLength(50).WithMessage("Location name cannot exceed 50 characters.");
//            RuleFor(x => x.Floor)
//               .NotEmpty().WithMessage("Floor  is required")
//               .LessThanOrEqualTo(100).WithMessage("Floor can not be greater than 100");
//            RuleFor(x => x.Description)
//                .MaximumLength(255).WithMessage("Location description cannot exceed 255 characters. ");
//        }

//    }
//}
