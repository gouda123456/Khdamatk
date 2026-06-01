# Requirements: Complete Service & ServiceOrder Implementation

## Overview

إكمال تطبيق نظام Service و ServiceOrder ليتطابق مع نظام Job و JobOrder الموجود في المشروع من حيث البنية والوظائف والـ API endpoints.

## Business Context

المشروع عبارة عن منصة خدمات (Khdamatk) تربط بين مقدمي الخدمات والعملاء. يوجد نظامان رئيسيان:

1. **Job System**: العميل ينشر وظيفة ومقدمو الخدمات يقدمون عروضهم
2. **Service System**: مقدم الخدمة ينشر خدمة والعملاء يطلبونها مباشرة

حالياً نظام Job مكتمل بينما نظام Service يحتاج للإكمال.

## Requirements

### REQ-1: Service Contracts Completion

**Priority**: High  
**Status**: Not Started

يجب إكمال الـ Contracts الخاصة بـ Service لتشمل:

#### REQ-1.1: Service Response Models

- `ServiceSummaryResponse`: نموذج ملخص الخدمة للعرض في القوائم
- `ServiceDetailedResponse`: نموذج تفصيلي للخدمة (مشابه لـ JobDetailed)
- يجب أن تتضمن:
  - معلومات الخدمة الأساسية (Id, Title, Description, Price)
  - معلومات مقدم الخدمة (Provider Info)
  - الصور والمرفقات
  - التقييمات والمراجعات
  - عدد الطلبات
  - وقت التسليم
  - عدد المراجعات المسموحة

#### REQ-1.2: Service Request Models

- `UpdateServiceRequest`: لتحديث الخدمة
- `ServiceFilterRequest`: للبحث والفلترة (مشابه لـ JobsFilterRequest)

### REQ-2: ServiceOrder Contracts Completion

**Priority**: High  
**Status**: Not Started

يجب إكمال الـ Contracts الخاصة بـ ServiceOrder:

#### REQ-2.1: ServiceOrder Response Models

- `ServiceOrderResponse`: نموذج تفصيلي للطلب (مشابه لـ JobOrderResponse)
  - معلومات الطلب (OrderId, OrderType, FinalPrice, Status)
  - معلومات العميل (Customer)
  - معلومات مقدم الخدمة (Provider)
  - ملخص الخدمة (ServiceSummary)
  - المحادثات (Chat)
  - الملفات المسلمة (DeliverableFiles)
- `ServiceOrderSummaryResponse`: ملخص الطلب للعرض في القوائم

#### REQ-2.2: ServiceOrder Request Models

- `StartServiceOrderPaymentRequest`: لبدء عملية الدفع
- `ServiceOrderFilterRequest`: للبحث والفلترة في الطلبات

### REQ-3: Service Interface Refactoring

**Priority**: High  
**Status**: Not Started

يجب إعادة هيكلة `IServiceOrderService` لتكون أكثر وضوحاً:

#### REQ-3.1: Separate Service CRUD Operations

إنشاء interface منفصل `IServiceService` يحتوي على:

- `AddServiceAsync(AddServiceRequest request, CancellationToken ct)`
- `GetServiceAsync(int serviceId, CancellationToken ct)`
- `GetServicesAsync(ServiceFilterRequest request, CancellationToken ct)`
- `UpdateServiceAsync(int serviceId, UpdateServiceRequest request, CancellationToken ct)`
- `DeleteServiceAsync(int serviceId, CancellationToken ct)`

#### REQ-3.2: Clean ServiceOrder Operations

تنظيف `IServiceOrderService` ليحتوي فقط على عمليات الطلبات:

- `AddOrderAsync(int serviceId, string customerId, OrderServiceRequest request, CancellationToken ct)`
- `AcceptOrderAsync(int orderId, string providerId, CancellationToken ct)`
- `RejectOrderAsync(int orderId, string providerId, CancellationToken ct)`
- `StartServiceOrderPaymentAsync(int orderId, StartServiceOrderPaymentRequest request, CancellationToken ct)`
- `PaymentSuccessAsync(WebHookModel model, CancellationToken ct)`
- `PaymentFailureAsync(CancelTransactionModel model, CancellationToken ct)`
- `GetOrderAsync(int orderId, string userId, CancellationToken ct)`
- `GetOrdersAsync(string userId, ServiceOrderFilterRequest request, CancellationToken ct)`
- `GetOrderSummaryAsync(int orderId, string userId, CancellationToken ct)`
- `SubmitWorkAndMessageAsync(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken ct)`
- `GetConversationsAsync(string userId, CancellationToken ct)`
- `GetConversationMessagesAsync(int orderId, string userId, CancellationToken ct)`
- `CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken ct)`
- `CancelOrderAsync(int orderId, string userId, CancellationToken ct)`
- `OpenDisputeAsync(int orderId, string raiserId, OrderDisputeRequest request, CancellationToken ct)`

### REQ-4: Service Implementation

**Priority**: High  
**Status**: Not Started

إنشاء `ServiceService` class في `Services/Implementations`:

- تطبيق جميع methods من `IServiceService`
- استخدام نفس النمط المستخدم في `JobService`
- التعامل مع الصور والمرفقات
- التحقق من الصلاحيات

### REQ-5: ServiceOrder Implementation Refactoring

**Priority**: High  
**Status**: Not Started

إعادة هيكلة `ServiceOrderService`:

- إزالة عمليات CRUD الخاصة بالخدمات
- التركيز على عمليات الطلبات فقط
- تطبيق نفس flow الموجود في `JobOrderService`:
  1. إنشاء الطلب
  2. قبول/رفض من مقدم الخدمة
  3. الدفع
  4. التنفيذ والمحادثات
  5. التسليم
  6. المراجعة والإكمال
  7. النزاعات

### REQ-6: Controllers Refactoring

**Priority**: High  
**Status**: Not Started

#### REQ-6.1: Create ServicesController

إنشاء `ServicesController` منفصل في `Controllers/V1`:

- `GET /api/Services` - Get all services with filtering
- `GET /api/Services/{id}` - Get service details
- `POST /api/Services` - Add new service
- `PUT /api/Services/{id}` - Update service
- `DELETE /api/Services/{id}` - Delete service

#### REQ-6.2: Refactor ServiceOrderController

تنظيف `ServiceOrderController` ليتطابق مع `JobOrderController`:

**Service Operations Section** (يجب نقلها لـ ServicesController):

- إزالة endpoints الخاصة بـ CRUD للخدمات

**Order Initialization Section**:

- `POST /api/ServiceOrder/Services/{serviceId}/Orders` - Create order
- `PUT /api/ServiceOrder/Services/{serviceId}/Orders/{orderId}/Accept` - Accept order
- `PUT /api/ServiceOrder/Services/{serviceId}/Orders/{orderId}/Reject` - Reject order
- `POST /api/ServiceOrder/ServiceOrders/{orderId}/Payment` - Start payment

**Order Middle Operations Section**:

- `PUT /api/ServiceOrder/ServiceOrders/{orderId}/Cancel` - Cancel order
- `GET /api/ServiceOrder/ServiceOrders/{orderId}/Summary` - Get order summary
- `GET /api/ServiceOrder/ServiceOrders/{orderId}` - Get order details
- `POST /api/ServiceOrder/ServiceOrders/{orderId}/SubmitWorkAndMessage` - Submit work
- `GET /api/ServiceOrder/ServiceOrders/{orderId}/ConversationMessages` - Get messages
- `GET /api/ServiceOrder/ServiceOrders/Conversations` - Get all conversations

**Order End Operations Section**:

- `PUT /api/ServiceOrder/ServiceOrders/{orderId}/Complete` - Complete order
- `POST /api/ServiceOrder/ServiceOrders/{orderId}/OpenDispute` - Open dispute

### REQ-7: Data Models Alignment

**Priority**: Medium  
**Status**: Not Started

التأكد من أن الـ Data Models في `Data/Entities` تدعم جميع الوظائف المطلوبة:

- `Service` entity
- `ServiceOrder` entity
- `ServiceOrderMessage` entity
- `ServiceOrderDeliverable` entity
- العلاقات بين الـ entities

### REQ-8: Validation

**Priority**: Medium  
**Status**: Not Started

إضافة FluentValidation validators لجميع الـ request models:

- `AddServiceRequestValidator`
- `UpdateServiceRequestValidator`
- `OrderServiceRequestValidator`
- `StartServiceOrderPaymentRequestValidator`

### REQ-9: Error Handling & Response Consistency

**Priority**: Medium  
**Status**: Not Started

التأكد من:

- استخدام `resultBase` بشكل متسق
- رسائل الأخطاء واضحة ومفيدة
- HTTP status codes صحيحة
- استخدام `.Respond()` extension method

### REQ-10: Authorization & Permissions

**Priority**: High  
**Status**: Not Started

تطبيق الصلاحيات:

- فقط مقدم الخدمة يمكنه إضافة/تعديل/حذف خدماته
- فقط العميل يمكنه إنشاء طلب
- فقط مقدم الخدمة يمكنه قبول/رفض الطلب
- كلاهما يمكنه إلغاء الطلب (بشروط)
- كلاهما يمكنه فتح نزاع

## Success Criteria

1. ✅ جميع الـ Contracts مكتملة ومتسقة مع نظام Job
2. ✅ الـ Services منفصلة ومنظمة (IServiceService و IServiceOrderService)
3. ✅ الـ Controllers منفصلة ومنظمة (ServicesController و ServiceOrderController)
4. ✅ جميع الـ API endpoints تعمل بشكل صحيح
5. ✅ الـ Validation موجودة لجميع الـ requests
6. ✅ الصلاحيات مطبقة بشكل صحيح
7. ✅ الكود يتبع نفس النمط والمعايير المستخدمة في Job system

## Out of Scope

- تعديل نظام Job الموجود
- إضافة features جديدة غير موجودة في Job system
- تعديل قاعدة البيانات (إلا إذا كان ضرورياً)
- إضافة Unit Tests (يمكن إضافتها لاحقاً)

## Dependencies

- نظام Job و JobOrder الموجود (كمرجع)
- FluentValidation library
- Mapster (للـ mapping)
- نظام Authentication الموجود
- نظام Payment (Fawaterak) الموجود

## Notes

- يجب الحفاظ على backward compatibility مع الكود الموجود
- يجب اتباع نفس naming conventions المستخدمة في المشروع
- يجب استخدام async/await بشكل صحيح
- يجب استخدام CancellationToken في جميع العمليات الطويلة
