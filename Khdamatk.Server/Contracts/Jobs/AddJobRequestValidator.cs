namespace Khdamatk.Server.Contracts.Jobs;

public class AddJobRequestValidator : AbstractValidator<AddJopRequest>
{
    public AddJobRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("معرف المستخدم (العميل) مطلوب.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الوظيفة مطلوب.")
            .MinimumLength(5).WithMessage("العنوان يجب أن يكون 5 أحرف على الأقل.")
            .MaximumLength(200).WithMessage("العنوان لا يجب أن يتجاوز 200 حرف.");

        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("يجب تحديد القسم.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("تفاصيل الوظيفة مطلوبة.")
            .MinimumLength(20).WithMessage("التفاصيل يجب أن تكون وافية (20 حرف على الأقل).");

        RuleFor(x => x.BudgetMin)
            .GreaterThan(0).WithMessage("الحد الأدنى للميزانية يجب أن يكون أكبر من الصفر.");

        RuleFor(x => x.BudgetMax)
            .GreaterThan(x => x.BudgetMin).WithMessage("الحد الأقصى للميزانية يجب أن يكون أكبر من الحد الأدنى.");

        RuleFor(x => x.TimeCommitment)
            .IsInEnum().WithMessage("قيمة وقت الالتزام غير صالحة.");

        RuleFor(x => x.ExperienceLevel)
            .IsInEnum().WithMessage("مستوى الخبرة غير صالح.");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.UtcNow).WithMessage("تاريخ التسليم يجب أن يكون في المستقبل.");

        RuleFor(x => x.Skills)
            .NotNull()
            .NotEmpty().WithMessage("يجب إضافة مهارة واحدة على الأقل.")
            .Must(skills => skills.Count <= 10).WithMessage("لا يمكنك إضافة أكثر من 10 مهارات."); // اختياري
    }
}