//using FluentValidation;
//using SmartHome.Application.DTOs.Category;

//namespace Gym.Application.Validators.Categories
//{
//    public class CreateCategoryValidator : AbstractValidator<CategoryCreateDto>
//    {
//        public CreateCategoryValidator()
//        {
//            RuleFor(x => x.Name)
//                .NotEmpty().WithMessage("Category name is required.")
//                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters.");

//            RuleFor(x => x.Description)
//                .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
//        }
//    }
//}
