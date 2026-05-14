# 📊 دليل ملء البيانات الشاملة (Data Seeding Guide)

## 🎯 نظرة عامة

تم إنشاء خدمة شاملة لملء قاعدة البيانات ببيانات تجريبية متكاملة تغطي **جميع الـ 35 جدول** في المشروع.

---

## 📋 ما سيتم إنشاؤه

### 1️⃣ **الملفات والصور (Media)** - 50 صورة
- ✅ 50 صورة PNG بحجم 400x400 بكسل
- ✅ ألوان متنوعة (أزرق، أحمر، أخضر، أصفر، بنفسجي، برتقالي، وردي، سماوي)
- ✅ مخزنة في: `wwwroot/uploads/image_1.png` إلى `image_50.png`

### 2️⃣ **المستخدمين (Users)** - 20 مستخدم
- ✅ 20 مستخدم بأدوار مختلفة (Admin, Client, Freelancer, ServiceProvider)
- ✅ كلمة السر لجميع المستخدمين: **`Giggo343@`**
- ✅ جميع المستخدمين: `EmailConfirmed = true`
- ✅ بيانات شخصية متكاملة (الاسم الكامل، تاريخ الميلاد، الصورة الشخصية)

### 3️⃣ **التصنيفات والمهارات (Categories & Skills)**
- ✅ 8 تصنيفات: البرمجة، التصميم، الكتابة، التسويق، الاستشارات، الترجمة، الفيديو، الموسيقى
- ✅ 30+ مهارة موزعة على التصنيفات

### 4️⃣ **مقدمو الخدمة (ServiceProviders)** - 10 مقدمين
- ✅ ملفات متكاملة مع bio وساعات عمل ورتبة
- ✅ مهارات محدودة لكل مقدم خدمة (5-7 مهارات)
- ✅ بيانات كاملة عن الأرباح والمشاريع المكتملة

### 5️⃣ **الخدمات (Services)** - 30 خدمة
- ✅ 30 خدمة مختلفة بأسعار متنوعة (500-5000)
- ✅ أسعار التسليم (3-17 يوم)
- ✅ ربط كل خدمة ب 3 صور

### 6️⃣ **الشهادات والأعمال السابقة**
- ✅ 30 شهادة (3 لكل مقدم خدمة)
- ✅ 50 عمل سابق (5 لكل مقدم خدمة)
- ✅ 100+ ملف وسائط مرتبط بالأعمال

### 7️⃣ **إعلانات الوظائف والعروض**
- ✅ 15 إعلان وظيفة مع متطلبات مهارات
- ✅ 45 عرض عمل (3 عروض لكل وظيفة)
- ✅ 45 مرحلة عمل (3 مراحل لكل وظيفة)

### 8️⃣ **طلبات الخدمات والوظائف**
- ✅ 20 طلب خدمة بحالات مختلفة
- ✅ 10 طلب وظيفة مع تواريخ البدء والانتهاء

### 9️⃣ **المعاملات المالية (Payments)**
- ✅ 30+ معاملة مالية
- ✅ حساب رسوم المنصة تلقائياً (10-15%)
- ✅ بيانات الدفع الكاملة (العملة، الحالة، بوابة الدفع)

### 🔟 **البطاقات الائتمانية (CreditCards)** - 20 بطاقة
- ✅ 20 بطاقة ائتمانية محاكية
- ✅ أرقام بطاقات صحيحة الصيغة
- ✅ تواريخ انتهاء صحيحة

### 1️⃣1️⃣ **المحادثات والرسائل**
- ✅ 30+ محادثة
- ✅ 100+ رسالة توزيعية
- ✅ محادثات للخدمات والوظائف والنزاعات

### 1️⃣2️⃣ **التقييمات والنزاعات**
- ✅ 15+ تقييم (تقييمات 3-5 نجوم)
- ✅ 3+ نزاعات مع محادثات الدعم

### 1️⃣3️⃣ **البيانات الإضافية**
- ✅ 100+ مفضل للمستخدمين
- ✅ 10+ مسلم وظيفة
- ✅ تقارير

---

## 🚀 كيفية الاستخدام

### الخطوة 1: التأكد من المتطلبات

```bash
# تأكد من أن Migrations مطبقة على قاعدة البيانات
dotnet ef database update
```

### الخطوة 2: تسجيل الخدمة في DependencyInjection

إذا لم تكن الخدمة مسجلة بالفعل، أضفها في ملف `DependancyInjections.cs`:

```csharp
services.AddScoped<DataSeederService>();
```

### الخطوة 3: تشغيل الـ API

استخدم أحد الأدوات التالية:

#### **باستخدام cURL:**
```bash
curl -X POST http://localhost:5000/api/v1/seeddata/seed-all \
  -H "Content-Type: application/json"
```

#### **باستخدام Postman:**
```
POST http://localhost:5000/api/v1/seeddata/seed-all
```

#### **باستخدام PowerShell:**
```powershell
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/v1/seeddata/seed-all" `
  -Method Post -ContentType "application/json"
$response | ConvertTo-Json
```

### الخطوة 4: الانتظار والتحقق

⏳ **المدة المتوقعة:** 30-60 ثانية

✅ **رد النجاح:**
```json
{
  "statusCode": 200,
  "message": "تم ملء البيانات بنجاح",
  "arabicMessage": "تم إضافة جميع البيانات التجريبية بنجاح لجميع الـ 35 جدول",
  "details": {
    "users": "20 مستخدم",
    "mediaFiles": "50 صورة PNG",
    "services": "30 خدمة",
    "jobPosts": "15 إعلان وظيفة",
    ...
  }
}
```

---

## 🧪 اختبار البيانات

### 1. التحقق من الصور

```bash
# في PowerShell
Get-ChildItem ".\Khdamatk.Server\wwwroot\uploads" | Measure-Object
# يجب أن يظهر 50 ملف
```

### 2. اختبار تسجيل الدخول

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user1@khdamatk.com",
    "password": "Giggo343@"
  }'
```

### 3. استعلامات SQL للتحقق

```sql
-- عدد المستخدمين
SELECT COUNT(*) as UsersCount FROM AspNetUsers;  -- 20

-- عدد الخدمات
SELECT COUNT(*) as ServicesCount FROM Services;  -- 30

-- عدد الصور
SELECT COUNT(*) as MediaCount FROM Medias;  -- 50

-- عدد الطلبات
SELECT COUNT(*) as ServiceOrdersCount FROM ServiceOrders;  -- 20
SELECT COUNT(*) as JobOrdersCount FROM JobOrders;  -- 10

-- عدد المحادثات والرسائل
SELECT COUNT(*) as ConversationsCount FROM Conversations;  -- 30+
SELECT COUNT(*) as MessagesCount FROM Messages;  -- 100+

-- عدد التقييمات
SELECT COUNT(*) as ReviewsCount FROM Reviews;  -- 15+

-- عرض بيانات مستخدم
SELECT Email, FullName, Role, EmailConfirmed FROM AspNetUsers WHERE Email = 'user1@khdamatk.com';
```

---

## ⚠️ ملاحظات مهمة

### ❌ لا تشغل الـ Endpoint مرتين

```
⚠️ تحذير: إذا قمت بتشغيل الـ Endpoint مرة ثانية، سيظهر خطأ 409 (Conflict)
```

**الحل:** حذف البيانات وإعادة التشغيل

```sql
-- حذف البيانات بالترتيب الصحيح
DELETE FROM JobDeliverables;
DELETE FROM Messages;
DELETE FROM Conversations;
DELETE FROM Disputes;
DELETE FROM Reviews;
DELETE FROM PaymentTransactions;
DELETE FROM CreditCards;
DELETE FROM JobOrders;
DELETE FROM ServiceOrders;
DELETE FROM JobOffers;
DELETE FROM JobSkillRequirements;
DELETE FROM JobPosts;
DELETE FROM MileStones;
DELETE FROM PortfolioMedia;
DELETE FROM PortfolioItems;
DELETE FROM Certificates;
DELETE FROM ProviderSkills;
DELETE FROM ServiceProviderProfiles;
DELETE FROM ServiceMedia;
DELETE FROM Services;
DELETE FROM UserFavorites;
DELETE FROM AspNetUsers;
DELETE FROM AspNetRoles;
DELETE FROM Medias;
DELETE FROM Skills;
DELETE FROM Categories;
```

ثم حذف الملفات:
```powershell
# في PowerShell
Remove-Item ".\Khdamatk.Server\wwwroot\uploads\*" -Force -Recurse
```

---

## 📊 إحصائيات البيانات

| الكيان | العدد | الملاحظات |
|--------|-------|----------|
| **Users** | 20 | كل الأدوار (Admin, Client, Freelancer, ServiceProvider) |
| **Media** | 50 | صور PNG بألوان مختلفة |
| **Categories** | 8 | تصنيفات رئيسية |
| **Skills** | 30+ | مهارات متنوعة |
| **ServiceProviders** | 10 | ملفات متكاملة |
| **Services** | 30 | خدمات بأسعار مختلفة |
| **Certificates** | 30 | 3 لكل مقدم خدمة |
| **PortfolioItems** | 50 | 5 لكل مقدم خدمة |
| **JobPosts** | 15 | إعلانات وظائف |
| **JobOffers** | 45 | 3 عروض لكل وظيفة |
| **MileStones** | 45 | 3 مراحل لكل وظيفة |
| **ServiceOrders** | 20 | طلبات خدمات متنوعة |
| **JobOrders** | 10 | طلبات وظائف |
| **PaymentTransactions** | 30+ | معاملات مالية |
| **CreditCards** | 20 | بطاقات ائتمانية |
| **Conversations** | 30+ | محادثات |
| **Messages** | 100+ | رسائل |
| **Reviews** | 15+ | تقييمات |
| **Disputes** | 3+ | نزاعات |
| **UserFavorites** | 100+ | المفضلة |
| **JobDeliverables** | 10+ | المسلمات |

**المجموع: 600+ سجل عبر 35 جدول**

---

## 🔐 أيانات الاختبار

### حسابات المستخدمين:

| البريد | كلمة السر | الدور |
|--------|----------|--------|
| user1@khdamatk.com | Giggo343@ | متنوع |
| user2@khdamatk.com | Giggo343@ | متنوع |
| user3@khdamatk.com | Giggo343@ | متنوع |
| ... | Giggo343@ | متنوع |
| user20@khdamatk.com | Giggo343@ | متنوع |

**جميع المستخدمين:** `EmailConfirmed = true`

---

## 🐛 استكشاف الأخطاء

### ❌ خطأ: "قاعدة البيانات موجودة بالفعل"

**السبب:** البيانات موجودة بالفعل

**الحل:**
1. حذف البيانات من SQL
2. حذف الملفات من wwwroot/uploads
3. تشغيل الـ Endpoint مرة أخرى

### ❌ خطأ: "الملفات لم تُنشأ"

**السبب:** مشكلة في أذونات المجلد

**الحل:**
1. تأكد أن مجلد `wwwroot/uploads` موجود
2. امنح أذونات الكتابة على المجلد
3. شغّل Visual Studio كمسؤول

### ❌ خطأ: "Timeout"

**السبب:** المشروع يستغرق وقت طويل

**الحل:**
1. انتظر 60 ثانية
2. تحقق من الـ Application Output
3. تأكد من أن قاعدة البيانات متاحة

---

## ✅ قائمة التحقق

- [ ] Migrations مطبقة على قاعدة البيانات
- [ ] مجلد `wwwroot/uploads` موجود
- [ ] DataSeederService مسجلة في DI
- [ ] المشروع يعمل بدون أخطاء (dotnet build)
- [ ] قاعدة البيانات فارغة من البيانات
- [ ] تشغيل الـ Endpoint POST مرة واحدة فقط
- [ ] الانتظار 30-60 ثانية للانتهاء
- [ ] التحقق من الملفات في wwwroot/uploads
- [ ] اختبار تسجيل الدخول

---

## 📚 المراجع

- **Endpoint:** `/api/v1/seeddata/seed-all`
- **Method:** `POST`
- **Content-Type:** `application/json`
- **Required Auth:** ❌ لا
- **Response Time:** 30-60 ثانية

---

## 💡 نصائح إضافية

### للتطوير المستقبلي:

1. **إضافة بيانات جديدة:** عدّل `DataSeederService.cs`
2. **تغيير الأسعار:** غيّر الثوابت في `CreateServicesAsync()`
3. **زيادة عدد البيانات:** زد حلقات `for`
4. **تغيير الألوان:** عدّل قائمة الألوان في `CreateMediaFilesAsync()`

---

## 📞 دعم وتواصل

إذا واجهت أي مشاكل:
1. تحقق من السجلات في Application Output
2. تأكد من صحة Connection String
3. تحقق من أن Migrations تم تطبيقها
4. احذف البيانات وحاول مرة أخرى

---

**تم الإنشاء:** 2026-05-14  
**الإصدار:** 1.0  
**الحالة:** ✅ جاهز للاستخدام
