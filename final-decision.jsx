import React, { useState } from "react";
import {
  CheckCircle,
  AlertCircle,
  DollarSign,
  Calendar,
  MessageCircle,
  FileText,
  Clock,
  User,
  Star,
  ArrowRight,
  Download,
  Send,
  Phone,
} from "lucide-react";

export default function FinalDecision() {
  const [decision, setDecision] = useState(null);
  const [contractAccepted, setContractAccepted] = useState(false);
  const [paymentMethod, setPaymentMethod] = useState("credit-card");

  const job = {
    id: 1,
    title: "تصميم واجهة موقع إلكتروني احترافية",
    budget: "2000-5000 EGP",
    duration: "3-5 أسابيع",
    deadline: "2026-04-15",
    category: "تصميم",
    description: "نحتاج إلى تصميم واجهة موقع إلكتروني متكامل",
  };

  const selectedFreelancer = {
    id: 1,
    name: "أحمد علي محمود",
    title: "مصمم UI/UX احترافي",
    rating: 4.9,
    reviews: 245,
    completedProjects: 87,
    proposedPrice: 3500,
    proposedDuration: "3 أسابيع",
    deliveryDate: "2026-04-10",
    message:
      "أنا مصمم UI/UX محترف مع خبرة 5 سنوات. لدي خبرة واسعة في Figma و Adobe XD. سأقوم بتسليم التصاميم بجودة عالية على الموعد المحدد.",
  };

  const contract = {
    terms: [
      "تسليم كامل التصاميم بصيغة Figma و PSD",
      "عدد التعديلات غير محدود",
      "ضمان الجودة والاحترافية",
      "توافر الفريلانسر للأسئلة والاستفسارات",
    ],
    paymentSchedule: [
      {
        id: 1,
        milestone: "عند بدء المشروع",
        percentage: 50,
        amount: 1750,
        status: "pending",
      },
      {
        id: 2,
        milestone: "عند تسليم النسخة الأولية",
        percentage: 25,
        amount: 875,
        status: "pending",
      },
      {
        id: 3,
        milestone: "عند الانتهاء النهائي",
        percentage: 25,
        amount: 875,
        status: "pending",
      },
    ],
  };

  return (
    <div dir="rtl" className="bg-gray-100 min-h-screen font-sans">
      {/* Header */}
      <header className="bg-white shadow sticky top-0 z-50">
        <div className="max-w-7xl mx-auto px-4 py-3 flex items-center justify-between">
          <div className="text-xl font-bold">
            <span className="text-purple-700">Khadma</span>{" "}
            <span className="text-yellow-500">Hub</span>
          </div>
          <h2 className="text-2xl font-bold text-gray-800">القرار النهائي</h2>
          <button className="text-purple-700 font-semibold">رجوع</button>
        </div>
      </header>

      {/* Progress Steps */}
      <div className="bg-white border-b">
        <div className="max-w-7xl mx-auto px-4 py-6">
          <div className="flex items-center justify-between">
            {[
              { step: 1, label: "اختيار الفريلانسر", status: "completed" },
              { step: 2, label: "مراجعة العرض", status: "completed" },
              { step: 3, label: "المعاينة النهائية", status: "active" },
              { step: 4, label: "الدفع", status: "pending" },
            ].map((item, idx) => (
              <div key={idx} className="flex items-center">
                <div
                  className={`w-10 h-10 rounded-full flex items-center justify-center font-bold ${
                    item.status === "completed"
                      ? "bg-green-500 text-white"
                      : item.status === "active"
                      ? "bg-purple-700 text-white"
                      : "bg-gray-300 text-gray-600"
                  }`}
                >
                  {item.status === "completed" ? (
                    <CheckCircle size={20} />
                  ) : (
                    item.step
                  )}
                </div>
                <p className="ml-2 text-sm font-semibold">{item.label}</p>
                {idx < 3 && <ArrowRight className="ml-4 text-gray-400" />}
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto p-4 py-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Left Column - Project & Freelancer */}
          <div className="lg:col-span-2 space-y-6">
            {/* Project Summary */}
            <div className="bg-white rounded-xl shadow p-6">
              <h2 className="text-2xl font-bold mb-4">ملخص المشروع</h2>
              <div className="space-y-3">
                <div className="flex justify-between items-start">
                  <span className="text-gray-600">اسم المشروع:</span>
                  <span className="font-semibold text-right">{job.title}</span>
                </div>
                <div className="flex justify-between items-start">
                  <span className="text-gray-600">الفئة:</span>
                  <span className="font-semibold">{job.category}</span>
                </div>
                <div className="flex justify-between items-start">
                  <span className="text-gray-600">الميزانية المتاحة:</span>
                  <span className="font-semibold text-purple-700">
                    {job.budget}
                  </span>
                </div>
                <div className="flex justify-between items-start">
                  <span className="text-gray-600">الموعد المطلوب:</span>
                  <span className="font-semibold">{job.deadline}</span>
                </div>
              </div>

              <hr className="my-4" />

              <div className="text-sm text-gray-700 leading-relaxed">
                <p className="font-semibold mb-2">وصف المشروع:</p>
                <p>{job.description}</p>
              </div>
            </div>

            {/* Freelancer Selection */}
            <div className="bg-white rounded-xl shadow p-6">
              <h2 className="text-2xl font-bold mb-4">الفريلانسر المختار</h2>

              <div className="flex items-start gap-4 mb-6 pb-6 border-b">
                <div className="w-24 h-24 rounded-full bg-gradient-to-r from-purple-400 to-pink-400 flex-shrink-0" />
                <div className="flex-1">
                  <h3 className="text-xl font-bold mb-1">
                    {selectedFreelancer.name}
                  </h3>
                  <p className="text-gray-600 mb-2">
                    {selectedFreelancer.title}
                  </p>
                  <div className="flex gap-4 text-sm">
                    <span className="flex items-center gap-1">
                      <Star size={14} className="text-yellow-500" fill="currentColor" />
                      {selectedFreelancer.rating} ({selectedFreelancer.reviews} تقييم)
                    </span>
                    <span className="flex items-center gap-1">
                      <CheckCircle size={14} className="text-green-500" />
                      {selectedFreelancer.completedProjects} مشروع
                    </span>
                  </div>
                </div>
              </div>

              {/* Proposal Details */}
              <div className="space-y-4">
                <div>
                  <p className="text-sm font-semibold text-gray-600 mb-2">
                    عرض الفريلانسر:
                  </p>
                  <div className="bg-gray-50 p-4 rounded-lg text-sm text-gray-700">
                    {selectedFreelancer.message}
                  </div>
                </div>

                <div className="grid grid-cols-3 gap-4">
                  <div className="bg-purple-50 p-3 rounded-lg text-center">
                    <p className="text-xs text-gray-600">السعر المقترح</p>
                    <p className="text-lg font-bold text-purple-700">
                      {selectedFreelancer.proposedPrice} EGP
                    </p>
                  </div>
                  <div className="bg-purple-50 p-3 rounded-lg text-center">
                    <p className="text-xs text-gray-600">المدة المقترحة</p>
                    <p className="text-lg font-bold text-purple-700">
                      {selectedFreelancer.proposedDuration}
                    </p>
                  </div>
                  <div className="bg-purple-50 p-3 rounded-lg text-center">
                    <p className="text-xs text-gray-600">تاريخ التسليم</p>
                    <p className="text-lg font-bold text-purple-700">
                      {selectedFreelancer.deliveryDate}
                    </p>
                  </div>
                </div>
              </div>
            </div>

            {/* Contract Terms */}
            <div className="bg-white rounded-xl shadow p-6">
              <h2 className="text-2xl font-bold mb-4">شروط العقد</h2>

              <div className="space-y-4 mb-6">
                <h3 className="font-semibold">متطلبات المشروع:</h3>
                <ul className="space-y-2">
                  {contract.terms.map((term, idx) => (
                    <li
                      key={idx}
                      className="flex gap-3 text-gray-700 text-sm items-start"
                    >
                      <CheckCircle
                        size={16}
                        className="text-green-500 flex-shrink-0 mt-0.5"
                      />
                      <span>{term}</span>
                    </li>
                  ))}
                </ul>
              </div>

              {/* Payment Schedule */}
              <div className="mb-6">
                <h3 className="font-semibold mb-3">جدول السداد:</h3>
                <div className="space-y-2">
                  {contract.paymentSchedule.map((payment) => (
                    <div
                      key={payment.id}
                      className="flex items-center justify-between p-3 bg-gray-50 rounded-lg border"
                    >
                      <div>
                        <p className="font-semibold text-sm">
                          {payment.milestone}
                        </p>
                        <p className="text-xs text-gray-600">
                          {payment.percentage}% من الإجمالي
                        </p>
                      </div>
                      <div className="text-right">
                        <p className="font-bold text-purple-700">
                          {payment.amount} EGP
                        </p>
                      </div>
                    </div>
                  ))}
                </div>
              </div>

              {/* Accept Terms */}
              <div className="boundary-t pt-4">
                <label className="flex items-center gap-3 cursor-pointer">
                  <input
                    type="checkbox"
                    checked={contractAccepted}
                    onChange={(e) => setContractAccepted(e.target.checked)}
                    className="w-5 h-5 cursor-pointer"
                  />
                  <span className="text-sm">
                    أوافق على شروط العقد والسياسات
                  </span>
                </label>
              </div>
            </div>
          </div>

          {/* Right Column - CTA */}
          <div className="space-y-6">
            {/* Price Summary */}
            <div className="bg-white rounded-xl shadow p-6 sticky top-24">
              <h3 className="text-xl font-bold mb-4">ملخص التكلفة</h3>

              <div className="space-y-3 mb-4 pb-4 border-b">
                <div className="flex justify-between items-center">
                  <span className="text-gray-600">سعر المشروع:</span>
                  <span className="font-semibold text-lg">
                    {selectedFreelancer.proposedPrice} EGP
                  </span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-gray-600">رسوم النظام (10%):</span>
                  <span className="font-semibold">
                    {Math.round(selectedFreelancer.proposedPrice * 0.1)} EGP
                  </span>
                </div>
              </div>

              <div className="flex justify-between items-center mb-6">
                <span className="font-bold text-lg">الإجمالي:</span>
                <span className="text-2xl font-bold text-purple-700">
                  {Math.round(selectedFreelancer.proposedPrice * 1.1)} EGP
                </span>
              </div>

              {/* Action Buttons */}
              <div className="space-y-3">
                <button
                  disabled={!contractAccepted}
                  className={`w-full py-3 rounded-lg font-semibold text-white flex items-center justify-center gap-2 transition ${
                    contractAccepted
                      ? "bg-gradient-to-r from-purple-700 to-purple-600 hover:shadow-lg"
                      : "bg-gray-400 cursor-not-allowed"
                  }`}
                >
                  <CheckCircle size={18} /> تأكيد واستمرار للدفع
                </button>

                <button className="w-full py-3 rounded-lg font-semibold text-purple-700 bg-purple-50 hover:bg-purple-100 transition flex items-center justify-center gap-2">
                  <MessageCircle size={18} /> تواصل مع الفريلانسر
                </button>

                <button className="w-full py-3 rounded-lg font-semibold text-gray-700 border border-gray-300 hover:bg-gray-50 transition flex items-center justify-center gap-2">
                  <AlertCircle size={18} /> طلب تعديل العروض
                </button>
              </div>

              {/* Trust Badge */}
              <div className="mt-6 p-4 bg-green-50 border border-green-200 rounded-lg text-center">
                <p className="text-xs text-green-700 font-semibold mb-1">
                  ✓ محمي بضمان Khadma Hub
                </p>
                <p className="text-xs text-green-600">
                  أموالك آمنة وسيتم الدفع عند قبول العمل
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
