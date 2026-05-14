# دليل استخدام Seeding API

**تاريخ الإنشاء:** 13 مايو 2026  
**الإصدار:** 1.0

---

## 📋 نظرة عامة

تم إنشاء API شامل لعمل Fake Seeding للبيانات في قاعدة البيانات مع صور حقيقية.

---

## 🚀 كيفية الاستخدام

### 1. تشغيل المشروع

```bash
cd Khdamatk.Server
dotnet run
```

### 2. استدعاء API

**Endpoint:**

```
POST https://localhost:7210/api/Test/SeedData
```

**Method:** POST  
**Authentication:** لا يتطلب مصادقة

**مثال باستخدام cURL:**

```bash
curl -X POST https://localhost:7210/api/Test/SeedData
```

**مثال باستخدام PowerShell:**

```powershell
Invoke-RestMethod -Uri "https://localhost:7210/api/Test/SeedData" -Method Post
```

**مثال باستخدام Postman:**

1. افتح Postman
2. اختر POST
3. أدخل URL: `https://localhost:7210/api/Test/SeedData`
4. اضغط Send

---

## 📊 البيانات التي سيتم إنشاؤها

### 1. **Media (الصور)** - 20 صورة

- صور PNG بألوان مختلفة (400x400 بكسل)
- يتم حفظها في: `wwwRoot/Uploads/`
- أسماء الملفات: `image_1.png` إلى `image_20.png`

### 2. **Categories (التصنيفات)** - 6 تصنيفات

- برمجة وتطوير
- تصميم جرافيك
- كتابة وترجمة
- تسويق رقمي
- فيديو وصوت
- أعمال

### 3. **Skills (المهارات)** - 15 مهارة

- C#, ASP.NET Core, React, Angular, Vue.js
- Python, Django, Node.js
- Photoshop, Illustrator, Figma, UI/UX Design
- Content Writing, SEO, Social Media Marketing

### 4. **Users (المستخدمين)** - 10 مستخدمين

- **5 Freelancers** (مقدمي خدمات)
- **5 Clients** (عملاء)

**بيانات تسجيل الدخول:**

- Email: `user1@khdamatk.com` إلى `user10@khdamatk.com`
- Password: `Giggo343@` (لجميع المستخدمين)

**أمثلة:**

```
Email: user1@khdamatk.com
Password: Giggo343@

Email: user2@khdamatk.com
Password: Giggo343@
```

### 5. **Service Provider Profiles** - 5 ملفات

- مطور Full Stack
- مصمم جرافيك
- كاتب محتوى
- مسوق رقمي
- مطور تطبيقات

**كل ملف يحتوي على:**

- مهارتين (Skills)
- شهادة واحدة (Certificate)
- عمل سابق واحد (Portfolio Item)

### 6. **Services (الخدمات)** - 10 خدمات

- تطوير موقع ويب متكامل
- تصميم شعار احترافي
- كتابة مقال SEO
- إدارة حسابات السوشيال ميديا
- تطوير تطبيق موبايل
- تصميم بنر إعلاني
- ترجمة محتوى
- مونتاج فيديو
- استشارة تسويقية
- تصميم واجهة مستخدم

**كل خدمة تحتوي على:**

- صورة رئيسية
- 2 صور في المعرض
- سعر عشوائي (50-1000)
- وقت تسليم عشوائي (1-30 يوم)

### 7. **Job Posts (إعلانات الوظائف)** - 5 إعلانات

- مطلوب مطور ويب
- مطلوب مصمم جرافيك
- مطلوب كاتب محتوى
- مطلوب مسوق رقمي
- مطلوب مطور تطبيقات

### 8. **Job Offers (عروض العمل)** - 5 عروض

- عرض واحد لكل إعلان وظيفة

### 9. **Service Orders (طلبات الخدمات)** - 5 طلبات

- حالات مختلفة (Pending, Active, Completed, etc.)

### 10. **Reviews (التقييمات)** - 5 تقييمات

- تقييمات للطلبات المكتملة
- تقييمات من 3 إلى 5 نجوم

---

## ✅ الاستجابة المتوقعة

عند نجاح العملية، ستحصل على:

```json
{
  "message": "Data seeded successfully!",
  "stats": {
    "users": 10,
    "categories": 6,
    "skills": 15,
    "providers": 5,
    "services": 10,
    "jobPosts": 5,
    "jobSkillRequirements": 5,
    "jobOffers": 5,
    "serviceOrders": 5,
    "reviews": 5,
    "media": 20
  }
}
```

---

## 🔍 التحقق من البيانات

### 1. التحقق من الصور

```bash
# في PowerShell
Get-ChildItem "Khdamatk.Server\wwwRoot\Uploads"
```

يجب أن ترى 20 ملف PNG:

```
image_1.png
image_2.png
...
image_20.png
```

### 2. التحقق من قاعدة البيانات

```sql
-- عدد المستخدمين
SELECT COUNT(*) FROM AspNetUsers;  -- يجب أن يكون 10

-- عدد الخدمات
SELECT COUNT(*) FROM Services;  -- يجب أن يكون 10

-- عدد الصور
SELECT COUNT(*) FROM Medias;  -- يجب أن يكون 20

-- عرض المستخدمين
SELECT Email, FullName, Role FROM AspNetUsers;

-- عرض الخدمات
SELECT Title, Price, DeliveryTimeInDays FROM Services;
```

### 3. اختبار تسجيل الدخول

```bash
# باستخدام cURL
curl -X POST https://localhost:7210/Auth \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user1@khdamatk.com",
    "password": "Giggo343@"
  }'
```

---

## ⚠️ ملاحظات مهمة

### 1. تشغيل API مرة واحدة فقط

- ⚠️ **لا تقم بتشغيل API أكثر من مرة** على نفس قاعدة البيانات
- سيؤدي ذلك إلى تكرار البيانات
- إذا أردت إعادة التشغيل، احذف البيانات أولاً

### 2. حذف البيانات

إذا أردت حذف البيانات وإعادة التشغيل:

```sql
-- حذف جميع البيانات (بالترتيب)
DELETE FROM Reviews;
DELETE FROM ServiceOrders;
DELETE FROM JobOffers;
DELETE FROM JobSkillRequirements;
DELETE FROM JobPosts;
DELETE FROM ServiceMedia;
DELETE FROM Services;
DELETE FROM PortfolioMedia;
DELETE FROM PortfolioItems;
DELETE FROM Certificates;
DELETE FROM ProviderSkills;
DELETE FROM ServiceProviderProfiles;
DELETE FROM AspNetUsers;
DELETE FROM Medias;
DELETE FROM Skills;
DELETE FROM Categories;

-- حذف الصور من المجلد
-- في PowerShell:
Remove-Item "Khdamatk.Server\wwwRoot\Uploads\*" -Force
```

### 3. المتطلبات

- ✅ قاعدة البيانات يجب أن تكون موجودة
- ✅ Migrations يجب أن تكون مطبقة
- ✅ المجلد `wwwRoot/Uploads` سيتم إنشاؤه تلقائياً

---

## 🐛 استكشاف الأخطاء

### خطأ: "Cannot insert duplicate key"

**السبب:** البيانات موجودة بالفعل  
**الحل:** احذف البيانات القديمة أولاً

### خطأ: "Foreign key constraint"

**السبب:** ترتيب الحذف خاطئ  
**الحل:** احذف البيانات بالترتيب الصحيح (من الأسفل للأعلى)

### خطأ: "Directory not found"

**السبب:** مجلد Uploads غير موجود  
**الحل:** سيتم إنشاؤه تلقائياً، تأكد من الصلاحيات

### خطأ: "User creation failed"

**السبب:** Password Policy  
**الحل:** كلمة المرور `Giggo343@` تلبي جميع المتطلبات

---

## 📸 أمثلة على البيانات المُنشأة

### مثال: مستخدم Freelancer

```json
{
  "fullName": "أحمد محمد",
  "email": "user1@khdamatk.com",
  "role": "Freelancer",
  "profile": {
    "jobTitle": "مطور Full Stack",
    "bio": "مطور محترف مع خبرة 5 سنوات في تطوير تطبيقات الويب",
    "hourlyRate": 250,
    "experienceYears": 5,
    "skills": ["C#", "ASP.NET Core"],
    "certificates": ["شهادة احترافية"],
    "portfolio": ["مشروع 1"]
  }
}
```

### مثال: خدمة

```json
{
  "title": "تطوير موقع ويب متكامل",
  "shortDescription": "وصف مختصر للخدمة 1",
  "price": 750,
  "deliveryTimeInDays": 15,
  "averageRating": 4.5,
  "totalReviews": 25,
  "category": "برمجة وتطوير",
  "provider": "أحمد محمد"
}
```

### مثال: طلب خدمة

```json
{
  "orderId": 1,
  "service": "تطوير موقع ويب متكامل",
  "customer": "عمر خالد",
  "provider": "أحمد محمد",
  "amount": 750,
  "status": "Active",
  "additionalDetails": "تفاصيل إضافية للطلب 1"
}
```

---

## 🎯 الاستخدامات المقترحة

### 1. التطوير والاختبار

- اختبار الواجهات (Frontend)
- اختبار APIs
- اختبار الأداء

### 2. العروض التقديمية (Demos)

- عرض المشروع للعملاء
- عرض الميزات
- التدريب

### 3. الاختبارات الآلية

- Integration Tests
- End-to-End Tests
- Load Testing

---

## 📝 التخصيص

إذا أردت تخصيص البيانات، يمكنك تعديل:

### 1. عدد المستخدمين

```csharp
// في CreateUsersAsync
for (int i = 0; i < 20; i++)  // بدلاً من 10
```

### 2. عدد الخدمات

```csharp
// في CreateServices
for (int i = 0; i < 20; i++)  // بدلاً من 10
```

### 3. كلمة المرور

```csharp
// في CreateUsersAsync
var password = "YourPassword123!";  // بدلاً من Giggo343@
```

### 4. ألوان الصور

```csharp
// في CreateFakeMediaAsync
var colors = new[] { "FF0000", "00FF00", "0000FF" };  // ألوان مخصصة
```

---

## ✅ Checklist قبل الاستخدام

- [ ] قاعدة البيانات موجودة
- [ ] Migrations مطبقة
- [ ] المشروع يعمل بدون أخطاء
- [ ] لا توجد بيانات قديمة (أو تم حذفها)
- [ ] الصلاحيات صحيحة لإنشاء الملفات

---

## 🔗 روابط مفيدة

- **Swagger UI:** https://localhost:7210/swagger
- **Scalar API:** https://localhost:7210/scalar/v1
- **Test Endpoint:** https://localhost:7210/api/Test

---

**تم إنشاء هذا الدليل بواسطة:** Kiro AI  
**التاريخ:** 13 مايو 2026  
**الإصدار:** 1.0
