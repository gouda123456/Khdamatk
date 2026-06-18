# تقرير الأخطاء المكتشفة في مشروع Khdamatk Backend

**تاريخ الفحص:** 13 مايو 2026  
**المراجع:** Kiro AI  
**نوع الفحص:** Logic Errors, Syntax Errors, EF Core Query Issues

---

## 🔴 أخطاء حرجة (Critical Errors)

### 1. خطأ منطقي في `VerificationsCodes` - Random Range

**الملف:** `Data/Entities/Identity/VerificationsCodes.cs`  
**السطر:** 28, 38  
**النوع:** Logic Error  
**الخطورة:** 🔴🔴🔴 عالية جداً

**الكود الخاطئ:**

```csharp
public VerificationsCodes(VerificationCodeType type)
{
    Type = type;
    Value = new Random().Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MinValue);
    //                                                              ^^^^^^^^                      ^^^^^^^^
    //                                                              نفس القيمة مرتين!
}

public void GenerateNewValue()
{
    Value = new Random().Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MinValue);
    //                                                                ^^^^^^^^                      ^^^^^^^^
}
```

**المشكلة:**

- استخدام `MinValue` مرتين بدلاً من `MinValue` و `MaxValue`
- هذا يعني أن الكود سيولد دائماً نفس القيمة (MinValue)
- الكود لن يعمل كما هو متوقع

**الحل:**

```csharp
public VerificationsCodes(VerificationCodeType type)
{
    Type = type;
    Value = new Random().Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue);
    //                                                              ^^^^^^^^                      ^^^^^^^^
}

public void GenerateNewValue()
{
    Value = new Random().Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue);
}
```

**التأثير:**

- جميع أكواد التحقق ستكون نفس القيمة
- خطر أمني كبير
- المستخدمون لن يتمكنوا من إعادة تعيين كلمة المرور

---

### 2. خطأ منطقي في `IsActive` Property

**الملف:** `Data/Entities/Identity/VerificationsCodes.cs`  
**السطر:** 15  
**النوع:** Logic Error  
**الخطورة:** 🔴🔴 عالية

**الكود الخاطئ:**

```csharp
public bool IsActive => DateTime.UtcNow < Createdat.AddDays(1) && IsUsed && !IsDelete;
//                                                                 ^^^^^^
//                                                                 خطأ منطقي!
```

**المشكلة:**

- الشرط `&& IsUsed` يعني أن الكود يكون Active فقط إذا كان **مُستخدم**
- المنطق الصحيح: الكود يكون Active إذا **لم يُستخدم بعد**

**الحل:**

```csharp
public bool IsActive => DateTime.UtcNow < Createdat.AddDays(1) && !IsUsed && !IsDelete;
//                                                                 ^^^^^^^
```

**التأثير:**

- جميع أكواد التحقق ستكون غير نشطة
- المستخدمون لن يتمكنوا من استخدام أكواد التحقق

---

### 3. خطأ في مقارنة التاريخ في `VerifyCodeAsync`

**الملف:** `Services/Implementations/AuthService.cs`  
**السطر:** 237-238  
**النوع:** Logic Error  
**الخطورة:** 🔴🔴 عالية

**الكود الخاطئ:**

```csharp
var expiryTime = DateTime.UtcNow.AddHours(1);

var validCode = await db.VerificationsCodes.FirstOrDefaultAsync
        (c => c.UserId == user.Id &&
            c.Type == request.CodeType &&
            c.Value == request.Value ,
            cancellationToken);

if (validCode?.Createdat >= expiryTime)
//              ^^^^^^^^^^    ^^^^^^^^^^
//              خطأ في المقارنة!
    return Failure(StatusCodes.Status409Conflict);
```

**المشكلة:**

- `expiryTime` هو وقت في المستقبل (الآن + ساعة)
- `Createdat` هو وقت في الماضي (وقت الإنشاء)
- المقارنة `Createdat >= expiryTime` ستكون دائماً `false`
- المنطق الصحيح: التحقق من أن الكود لم ينتهي بعد

**الحل:**

```csharp
var validCode = await db.VerificationsCodes.FirstOrDefaultAsync
        (c => c.UserId == user.Id &&
            c.Type == request.CodeType &&
            c.Value == request.Value &&
            !c.IsUsed &&
            c.Createdat.AddHours(1) > DateTime.UtcNow, // الكود صالح لمدة ساعة
            cancellationToken);

if (validCode is null)
{
    return Failure(StatusCodes.Status400BadRequest, "Invalid code", "The verification code is invalid or has expired");
}
```

**التأثير:**

- أكواد التحقق المنتهية ستظل تعمل
- خطر أمني

---

### 4. خطأ في استعلام EF Core في `OrderService`

**الملف:** `Services/Implementations/OrderService.cs`  
**السطر:** 31-35  
**النوع:** Logic Error + Hardcoded Value  
**الخطورة:** 🔴🔴🔴 عالية جداً

**الكود الخاطئ:**

```csharp
var order = await db.ServiceOrders
    .Include(o => o.Customer)
    .Include(o => o.Service)
    .Include(o => o.ServiceProviderProfile)
        .ThenInclude(p => p.User)
    .FirstOrDefaultAsync(o => o.Id == 1);
    //                                ^^^
    //                                Hardcoded!
```

**المشكلة:**

- استخدام `o.Id == 1` بدلاً من `o.Id == orderId`
- سيتم دائماً جلب الطلب رقم 1 بغض النظر عن `orderId` المُمرر
- خطأ فادح في المنطق

**الحل:**

```csharp
var order = await db.ServiceOrders
    .Include(o => o.Customer)
    .Include(o => o.Service)
    .Include(o => o.ServiceProviderProfile)
        .ThenInclude(p => p.User)
    .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    //                                ^^^^^^^^
```

**التأثير:**

- جميع المستخدمين سيدفعون للطلب رقم 1
- خطأ كارثي في نظام الدفع
- فقدان البيانات

---

### 5. خطأ أمني في `SetPasswordAsync`

**الملف:** `Services/Implementations/AuthService.cs`  
**السطر:** 177-179  
**النوع:** Security Issue + Logic Error  
**الخطورة:** 🔴🔴🔴 عالية جداً

**الكود الخاطئ:**

```csharp
public async Task<resultBase> SetPasswordAsync(SetPasswordRequest request, CancellationToken cancellationToken = default)
{
    if(await userManager.FindByEmailAsync(request.Email) is not { } user)
        return Success(StatusCodes.Status200OK, "Password changed successfully", "Your password has been changed successfully");
        //     ^^^^^^^
        //     إرجاع Success حتى لو لم يُعثر على المستخدم!
```

**المشكلة:**

- إذا لم يُعثر على المستخدم، يتم إرجاع `Success`
- هذا خطأ أمني ومنطقي
- يجب إرجاع `Failure` أو `NotFound`

**الحل:**

```csharp
public async Task<resultBase> SetPasswordAsync(SetPasswordRequest request, CancellationToken cancellationToken = default)
{
    if(await userManager.FindByEmailAsync(request.Email) is not { } user)
        return Failure(StatusCodes.Status404NotFound, UserErrors.UserNotFound);
        //     ^^^^^^^
```

**التأثير:**

- يمكن للمهاجم معرفة ما إذا كان البريد الإلكتروني موجود أم لا
- خطر أمني

---

## 🟠 أخطاء متوسطة (Medium Errors)

### 6. مشكلة في `Conversation` Entity

**الملف:** `Data/Entities/Operations/JobOrder.cs`  
**السطر:** 48  
**النوع:** Potential Issue  
**الخطورة:** 🟠 متوسطة

**الكود:**

```csharp
[ForeignKey(nameof(Conversation))]
public int ConversationId { get; set; }
public virtual Conversation Conversation { get; set; } = new Conversation();
//                                                       ^^^^^^^^^^^^^^^^^^^
//                                                       تهيئة جديدة
```

**المشكلة:**

- تهيئة `Conversation` بقيمة جديدة قد يسبب مشاكل مع EF Core
- يفضل استخدام `null!` أو `= null!`

**الحل:**

```csharp
public virtual Conversation Conversation { get; set; } = null!;
```

**التأثير:**

- قد يسبب مشاكل في Tracking
- قد يتم إنشاء Conversations غير مرغوب فيها

---

### 7. خطأ في `JobOrder.BuildOrder`

**الملف:** `Data/Entities/Operations/JobOrder.cs`  
**السطر:** 70  
**النوع:** Logic Error  
**الخطورة:** 🟠 متوسطة

**الكود الخاطئ:**

```csharp
Conversation = new Conversation
{
    Category = ConversationCategory.Standard,
    CustomerId = job.CustomerId,
    ContextType = ConversationContextType.JobOffer,
    ProviderId = offer.ProviderProfileId,
    Title = job.Title,
    // ❌ لا يوجد ServiceOrderId
}
```

**المشكلة:**

- `Conversation` يحتاج إلى `ServiceOrderId` حسب تعريف الـ Entity
- لكن هنا نحن في `JobOrder` وليس `ServiceOrder`
- قد يكون هناك خطأ في تصميم الـ Schema

**الحل:**
يجب مراجعة تصميم `Conversation` Entity:

```csharp
// Option 1: جعل ServiceOrderId nullable
public int? ServiceOrderId { get; set; }

// Option 2: إضافة JobOrderId
public int? JobOrderId { get; set; }
```

**التأثير:**

- قد يفشل حفظ الـ JobOrder
- خطأ في قاعدة البيانات

---

### 8. مشكلة في `PaymentTransaction` Navigation

**الملف:** `Data/Entities/Operations/JobOrder.cs`  
**السطر:** 34-35  
**النوع:** Potential Issue  
**الخطورة:** 🟠 متوسطة

**الكود:**

```csharp
[ForeignKey(nameof(PaymentTransaction))]
public int PaymentTransactionId { get; set; }
public virtual PaymentTransaction PaymentTransaction { get; set; } = new();
//                                                                   ^^^^^^
```

**المشكلة:**

- تهيئة `PaymentTransaction` بقيمة جديدة
- لكن `PaymentTransactionId` مطلوب
- قد يسبب مشاكل عند الحفظ

**الحل:**

```csharp
public virtual PaymentTransaction? PaymentTransaction { get; set; }
// أو
public virtual PaymentTransaction PaymentTransaction { get; set; } = null!;
```

**التأثير:**

- قد يتم إنشاء PaymentTransactions فارغة
- مشاكل في Tracking

---

## 🟡 تحذيرات (Warnings)

### 9. اسم ملف خاطئ

**الملف:** `Data/Entities/Operations/OrderBase .cs`  
**النوع:** File Naming Issue  
**الخطورة:** 🟡 منخفضة

**المشكلة:**

- اسم الملف يحتوي على مسافة في النهاية: `OrderBase .cs`
- يجب أن يكون: `OrderBase.cs`

**الحل:**
إعادة تسمية الملف:

```bash
# في PowerShell
Rename-Item "OrderBase .cs" "OrderBase.cs"
```

**التأثير:**

- قد يسبب مشاكل في بعض الأدوات
- صعوبة في البحث عن الملف

---

### 10. استخدام `private set` مفقود

**الملف:** `Data/Entities/Identity/VerificationsCodes.cs`  
**السطر:** 10  
**النوع:** Design Issue  
**الخطورة:** 🟡 منخفضة

**الكود الحالي:**

```csharp
public int Value { get; set; }
```

**المشكلة:**

- `Value` يمكن تعديله من الخارج
- يجب أن يكون `private set` لأن هناك method `GenerateNewValue()`

**الحل:**

```csharp
public int Value { get; private set; }
```

**التأثير:**

- يمكن تعديل القيمة من الخارج
- قد يتم تجاوز الـ Validation

---

### 11. استخدام `new Random()` في كل مرة

**الملف:** `Data/Entities/Identity/VerificationsCodes.cs`  
**السطر:** 28, 38  
**النوع:** Performance Issue  
**الخطورة:** 🟡 منخفضة

**الكود:**

```csharp
Value = new Random().Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue);
```

**المشكلة:**

- إنشاء `Random` جديد في كل مرة قد يعطي نفس القيم إذا تم استدعاؤه بسرعة
- يفضل استخدام `Random.Shared` (في .NET 6+)

**الحل:**

```csharp
Value = Random.Shared.Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue);
```

**التأثير:**

- قد يتم توليد نفس الأكواد في بعض الحالات
- مشكلة في الأداء

---

### 12. استخدام `IsUsed` بدون `private set`

**الملف:** `Data/Entities/Identity/VerificationsCodes.cs`  
**السطر:** 11  
**النوع:** Design Issue  
**الخطورة:** 🟡 منخفضة

**الكود:**

```csharp
public bool IsUsed { get; set; } = false;
```

**المشكلة:**

- `IsUsed` يمكن تعديله من الخارج
- يجب أن يكون `private set` أو يتم التحكم فيه عبر method

**الحل:**

```csharp
public bool IsUsed { get; private set; } = false;

public void MarkAsUsed()
{
    IsUsed = true;
    Updatedat = DateTime.UtcNow;
}
```

**التأثير:**

- يمكن تعديل الحالة من الخارج
- قد يتم تجاوز الـ Business Logic

---

## 📊 ملخص الأخطاء

| النوع               | العدد | الخطورة       |
| ------------------- | ----- | ------------- |
| **Logic Errors**    | 5     | 🔴 عالية جداً |
| **Security Issues** | 2     | 🔴 عالية جداً |
| **EF Core Issues**  | 2     | 🟠 متوسطة     |
| **Design Issues**   | 3     | 🟡 منخفضة     |
| **File Naming**     | 1     | 🟡 منخفضة     |

**الإجمالي:** 13 خطأ/تحذير

---

## 🚨 الأخطاء التي يجب إصلاحها فوراً

### أولوية قصوى (يجب إصلاحها قبل أي شيء):

1. ✅ **خطأ Random Range في VerificationsCodes** (خطأ 1)
2. ✅ **خطأ IsActive Logic** (خطأ 2)
3. ✅ **خطأ Hardcoded orderId** (خطأ 4)
4. ✅ **خطأ SetPasswordAsync Security** (خطأ 5)
5. ✅ **خطأ VerifyCodeAsync Date Comparison** (خطأ 3)

### أولوية عالية:

6. ✅ **مشكلة Conversation في JobOrder** (خطأ 7)
7. ✅ **مشكلة PaymentTransaction Navigation** (خطأ 8)

### أولوية متوسطة:

8. ✅ **إعادة تسمية OrderBase .cs** (خطأ 9)
9. ✅ **استخدام Random.Shared** (خطأ 11)
10. ✅ **إضافة private set** (خطأ 10, 12)

---

## 🔧 خطة الإصلاح المقترحة

### المرحلة 1: إصلاح الأخطاء الحرجة (يوم واحد)

```csharp
// 1. إصلاح VerificationsCodes.cs
public VerificationsCodes(VerificationCodeType type)
{
    Type = type;
    Value = Random.Shared.Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue);
}

public void GenerateNewValue()
{
    Value = Random.Shared.Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue);
}

public bool IsActive => DateTime.UtcNow < Createdat.AddDays(1) && !IsUsed && !IsDelete;

public int Value { get; private set; }
public bool IsUsed { get; private set; } = false;

public void MarkAsUsed()
{
    IsUsed = true;
    Updatedat = DateTime.UtcNow;
}
```

```csharp
// 2. إصلاح AuthService.cs - VerifyCodeAsync
var validCode = await db.VerificationsCodes.FirstOrDefaultAsync
        (c => c.UserId == user.Id &&
            c.Type == request.CodeType &&
            c.Value == request.Value &&
            !c.IsUsed &&
            c.Createdat.AddHours(1) > DateTime.UtcNow,
            cancellationToken);

if (validCode is null)
{
    return Failure(StatusCodes.Status400BadRequest, "Invalid code", "The verification code is invalid or has expired");
}

validCode.MarkAsUsed();
```

```csharp
// 3. إصلاح AuthService.cs - SetPasswordAsync
if(await userManager.FindByEmailAsync(request.Email) is not { } user)
    return Failure(StatusCodes.Status404NotFound, UserErrors.UserNotFound);
```

```csharp
// 4. إصلاح OrderService.cs
var order = await db.ServiceOrders
    .Include(o => o.Customer)
    .Include(o => o.Service)
    .Include(o => o.ServiceProviderProfile)
        .ThenInclude(p => p.User)
    .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
```

### المرحلة 2: إصلاح المشاكل المتوسطة (نصف يوم)

```csharp
// 5. مراجعة Conversation Schema
// في Conversation.cs
public int? ServiceOrderId { get; set; }
public int? JobOrderId { get; set; }

// في JobOrder.cs
public virtual Conversation Conversation { get; set; } = null!;
public virtual PaymentTransaction? PaymentTransaction { get; set; }
```

### المرحلة 3: التحسينات (نصف يوم)

```bash
# 6. إعادة تسمية الملف
Rename-Item "OrderBase .cs" "OrderBase.cs"
```

---

## ✅ Checklist للإصلاح

- [ ] إصلاح Random Range في VerificationsCodes
- [ ] إصلاح IsActive Logic
- [ ] إصلاح VerifyCodeAsync Date Comparison
- [ ] إصلاح Hardcoded orderId في OrderService
- [ ] إصلاح SetPasswordAsync Security Issue
- [ ] مراجعة Conversation Schema
- [ ] إصلاح PaymentTransaction Navigation
- [ ] إعادة تسمية OrderBase .cs
- [ ] استخدام Random.Shared
- [ ] إضافة private set للـ Properties
- [ ] كتابة Unit Tests للتحقق من الإصلاحات
- [ ] اختبار جميع السيناريوهات

---

## 📝 ملاحظات إضافية

### نصائح لتجنب الأخطاء المستقبلية:

1. **استخدام Unit Tests** - كل الأخطاء المذكورة كان يمكن اكتشافها بالاختبارات
2. **Code Review** - مراجعة الكود قبل الـ Commit
3. **Static Analysis Tools** - استخدام أدوات مثل SonarQube
4. **Linting** - استخدام Roslyn Analyzers

### أدوات مفيدة:

```xml
<!-- إضافة إلى .csproj -->
<ItemGroup>
  <PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="8.0.0" />
  <PackageReference Include="SonarAnalyzer.CSharp" Version="9.0.0" />
</ItemGroup>
```

---

**تم إنشاء هذا التقرير بواسطة:** Kiro AI  
**التاريخ:** 13 مايو 2026  
**الإصدار:** 1.0
