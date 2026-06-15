# Implementation Plan

## Overview

هذا المشروع يهدف إلى إكمال تطبيق نظام Service و ServiceOrder ليتطابق مع نظام Job و JobOrder الموجود في المشروع من حيث البنية والوظائف والـ API endpoints.

## Tasks

- [x] 1. Create Service Response Contracts - إنشاء نماذج الاستجابة للخدمات (ServiceSummaryResponse, ServiceFilterRequest)

- [x] 2. Create Service Request Contracts - إنشاء نماذج الطلبات للخدمات (UpdateServiceRequest مع Validator)
  - **depends on**: 1

- [x] 3. Create ServiceOrder Response Contracts - إنشاء نماذج الاستجابة لطلبات الخدمات (ServiceOrderResponse, ServiceOrderSummaryResponse, ServiceOrderFilterRequest)
  - **depends on**: 1

- [x] 4. Create IServiceService Interface - إنشاء interface للخدمات منفصل عن ServiceOrder
  - **depends on**: 1, 2

- [x] 5. Refactor IServiceOrderService Interface - إعادة هيكلة IServiceOrderService لإزالة عمليات CRUD للخدمات
  - **depends on**: 3, 4

- [x] 6. Implement ServiceService Class - تطبيق ServiceService class مع جميع methods (Add, Get, Update, Delete)
  - **depends on**: 4

- [~] 7. Refactor ServiceOrderService Implementation - إعادة هيكلة ServiceOrderService implementation لتتبع نمط JobOrderService
  - **depends on**: 5, 6

- [x] 8. Create ServicesController - إنشاء ServicesController منفصل مع جميع CRUD endpoints
  - **depends on**: 6

- [~] 9. Refactor ServiceOrderController - إعادة هيكلة ServiceOrderController لتتبع نمط JobOrderController
  - **depends on**: 7, 8

- [~] 10. Update Dependency Injection - تحديث DependencyInjections.cs لتسجيل IServiceService و ServiceService
  - **depends on**: 6, 7

- [~] 11. Add Validators for New Contracts - إضافة FluentValidation validators للـ contracts الجديدة
  - **depends on**: 2, 3

- [x] 12. Verify Data Models - التحقق من Data Models (Service, ServiceOrder entities) وتحديثها إذا لزم الأمر

- [~] 13. Test Service CRUD Operations - اختبار عمليات CRUD للخدمات بشكل كامل
  - **depends on**: 8, 10

- [~] 14. Test ServiceOrder Workflow - اختبار workflow الكامل للطلبات من البداية للنهاية
  - **depends on**: 9, 10

- [~] 15. Integration Testing - اختبار التكامل مع الأنظمة الخارجية (Payment, Notifications, Files)
  - **depends on**: 13, 14

- [~] 16. Documentation and Code Review - التوثيق والمراجعة النهائية للكود
  - **depends on**: 15

## Notes

- يجب اتباع نفس النمط والمعايير المستخدمة في Job/JobOrder system
- الحفاظ على backward compatibility مع الكود الموجود
- استخدام async/await و CancellationToken بشكل صحيح
- تطبيق Authorization بشكل صحيح في جميع endpoints

## Task Dependency Graph

```
1 (Service Response Contracts)
├─> 2 (Service Request Contracts)
│   └─> 4 (IServiceService Interface)
│       ├─> 5 (Refactor IServiceOrderService)
│       │   └─> 7 (Refactor ServiceOrderService)
│       │       ├─> 9 (Refactor ServiceOrderController)
│       │       │   └─> 10 (Update DI)
│       │       │       ├─> 14 (Test ServiceOrder Workflow)
│       │       │       │   └─> 15 (Integration Testing)
│       │       │       │       └─> 16 (Documentation)
│       │       │       └─> 13 (Test Service CRUD)
│       │       │           └─> 15
│       │       └─> 10
│       └─> 6 (Implement ServiceService)
│           ├─> 7
│           ├─> 8 (Create ServicesController)
│           │   ├─> 9
│           │   └─> 13
│           └─> 10
└─> 3 (ServiceOrder Response Contracts)
    ├─> 5
    └─> 11 (Add Validators)

12 (Verify Data Models) - Independent
```
