namespace Khdamatk.Server.Contracts.Home;


public record AddPortfolioRequest(
    string Title,
    string Description,
    string ImageUrl
);



public record AddEducationRequest(
    string SchoolName,      // اسم الجامعة أو المدرسة
    string Degree,          // الدرجة العلمية (مثلاً Bachelor's)
    string FieldOfStudy,    // التخصص
    string Description,     // وصف بسيط
    DateTime StartDate,     // تاريخ البدء
    DateTime? EndDate       // تاريخ التخرج (ممكن يكون null لو لسه بيدرس)
);


public record AddExperienceRequest(
    string Title,           // المسمى الوظيفي (مثلاً Senior Developer)
    string CompanyName,     // اسم الشركة
    string Description,     // المهام اللي كنت بتعملها
    DateTime StartDate,     // تاريخ البداية
    DateTime? EndDate       // تاريخ النهاية (null لو لسه شغال هناك)
);
public record UpdateProfileRequest(
    string JobTitle,
    string? Bio,
    double HourlyRate,
    int ExperienceYears
);
public record UpdateSkillsRequest(
    List<int> SkillIds // لستة بـ IDs المهارات اللي اليوزر اختارها من الـ Dropdown
);