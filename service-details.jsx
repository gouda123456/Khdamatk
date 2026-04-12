import React, { useState } from "react";
import {
  Star,
  MessageCircle,
  ShoppingCart,
  Heart,
  Share2,
  ChevronLeft,
  ChevronRight,
  CheckCircle,
  Clock,
  RotateCcw,
  AlertCircle,
} from "lucide-react";

export default function ServiceDetails() {
  const [currentImageIdx, setCurrentImageIdx] = useState(0);
  const [isWishlisted, setIsWishlisted] = useState(false);
  const [selectedPackage, setSelectedPackage] = useState("basic");

  const service = {
    id: 1,
    name: "تصميم شعار احترافي وفريد",
    seller: "محمد أحمد",
    sellerId: 1,
    rating: 4.8,
    reviews: 342,
    completedOrders: 1250,
    category: "تصميم جرافيك",
    badge: "Top Seller",
    profileImage:
      "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=100&h=100&fit=crop",
    description:
      "مرحبا، أنا مصمم جرافيك احترافي متخصص في تصميم الشعارات والهويات البصرية. لدي خبرة 8 سنوات في تصميم شعارات فريدة وغير مكررة.\n\nما أقدمه:\n✓ تصميم شعار احترافي وفريد\n✓ ملفات بصيغ متعددة (PNG, PDF, AI, PSD)\n✓ تعديلات غير محدودة حتى تصل للنتيجة المطلوبة\n✓ استشارة مجانية حول الألوان والخطوط\n✓ حقوق ملكية كاملة للعمل\n\nعملية العمل:\n1. استقبل فكرتك ومتطلباتك\n2. أقدم 3 مفاهيم مختلفة\n3. نختار الأفضل ونطورها\n4. تعديلات حتى تصل لرضاك الكامل",
    images: [
      "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=500&h=500&fit=crop",
      "https://images.unsplash.com/photo-1561415261-38a76be7ce87?w=500&h=500&fit=crop",
      "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=500&h=500&fit=crop",
    ],
    packages: [
      {
        id: "basic",
        name: "الباقة الأساسية",
        price: 500,
        deliveryDays: 3,
        revisions: 2,
        description: "مفهوم واحد مع 2 تعديلات",
        features: ["مفهوم واحد", "2 تعديل", "ملف PNG و PDF", "استشارة مجانية"],
      },
      {
        id: "standard",
        name: "الباقة المتوسطة",
        price: 1000,
        deliveryDays: 5,
        revisions: 5,
        description: "3 مفاهيم مع 5 تعديلات",
        features: [
          "3 مفاهيم",
          "5 تعديلات",
          "ملفات متعددة (PNG, PDF, AI, PSD)",
          "أيقونة مصغرة",
          "استشارة مجانية",
        ],
      },
      {
        id: "premium",
        name: "الباقة المميزة",
        price: 2000,
        deliveryDays: 7,
        revisions: 999,
        description: "مفاهيم غير محدودة مع تعديلات غير محدودة",
        features: [
          "مفاهيم غير محدودة",
          "تعديلات غير محدودة",
          "جميع الملفات",
          "Brand Guidelines كامل",
          "استشارة موسعة",
          "دعم ما بعد التسليم",
        ],
      },
    ],
  };

  const currentPackage = service.packages.find((p) => p.id === selectedPackage);

  const nextImage = () => {
    setCurrentImageIdx((prev) => (prev + 1) % service.images.length);
  };

  const prevImage = () => {
    setCurrentImageIdx(
      (prev) => (prev - 1 + service.images.length) % service.images.length
    );
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
          <h2 className="text-2xl font-bold text-gray-800">تفاصيل الخدمة</h2>
          <button className="text-purple-700 font-semibold">رجوع</button>
        </div>
      </header>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto p-4 py-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Left Column - Images & Details */}
          <div className="lg:col-span-2 space-y-6">
            {/* Image Gallery */}
            <div className="bg-white rounded-xl shadow overflow-hidden">
              <div className="relative bg-gray-200 aspect-square">
                <img
                  src={service.images[currentImageIdx]}
                  alt="Service"
                  className="w-full h-full object-cover"
                />

                {/* Navigation Buttons */}
                <button
                  onClick={prevImage}
                  className="absolute left-4 top-1/2 transform -translate-y-1/2 bg-white/90 hover:bg-white p-2 rounded-full shadow transition"
                >
                  <ChevronLeft size={24} />
                </button>
                <button
                  onClick={nextImage}
                  className="absolute right-4 top-1/2 transform -translate-y-1/2 bg-white/90 hover:bg-white p-2 rounded-full shadow transition"
                >
                  <ChevronRight size={24} />
                </button>

                {/* Image Counter */}
                <div className="absolute bottom-4 left-4 bg-black/50 text-white px-3 py-1 rounded-full text-sm">
                  {currentImageIdx + 1} / {service.images.length}
                </div>
              </div>

              {/* Thumbnails */}
              <div className="flex gap-2 p-4 bg-gray-50">
                {service.images.map((img, idx) => (
                  <button
                    key={idx}
                    onClick={() => setCurrentImageIdx(idx)}
                    className={`w-16 h-16 rounded-lg overflow-hidden border-2 transition ${
                      idx === currentImageIdx
                        ? "border-purple-700"
                        : "border-gray-300"
                    }`}
                  >
                    <img src={img} alt={`Thumbnail ${idx + 1}`} className="w-full h-full object-cover" />
                  </button>
                ))}
              </div>
            </div>

            {/* Service Info */}
            <div className="bg-white rounded-xl shadow p-6">
              <div className="flex items-start justify-between mb-4">
                <div>
                  <h1 className="text-3xl font-bold text-gray-800 mb-2">
                    {service.name}
                  </h1>
                  <div className="flex items-center gap-4">
                    <span className="inline-block bg-yellow-100 text-yellow-800 px-3 py-1 rounded-full text-sm font-semibold">
                      {service.badge}
                    </span>
                    <span className="text-sm text-gray-600">
                      الفئة: {service.category}
                    </span>
                  </div>
                </div>
                <div className="flex gap-2">
                  <button
                    onClick={() => setIsWishlisted(!isWishlisted)}
                    className={`p-3 rounded-lg transition ${
                      isWishlisted
                        ? "bg-red-100 text-red-600"
                        : "bg-gray-100 text-gray-600 hover:bg-gray-200"
                    }`}
                  >
                    <Heart size={20} fill={isWishlisted ? "currentColor" : "none"} />
                  </button>
                  <button className="p-3 rounded-lg bg-gray-100 text-gray-600 hover:bg-gray-200 transition">
                    <Share2 size={20} />
                  </button>
                </div>
              </div>

              <hr className="my-4" />

              {/* Seller Info */}
              <div className="flex items-center gap-4 pb-6 border-b">
                <img
                  src={service.profileImage}
                  alt={service.seller}
                  className="w-16 h-16 rounded-full object-cover"
                />
                <div className="flex-1">
                  <h3 className="text-lg font-bold text-gray-800 mb-1">
                    {service.seller}
                  </h3>
                  <div className="flex gap-4 text-sm text-gray-600">
                    <span className="flex items-center gap-1">
                      <Star size={14} className="text-yellow-500" fill="currentColor" />
                      {service.rating} ({service.reviews} تقييم)
                    </span>
                    <span className="flex items-center gap-1">
                      <CheckCircle size={14} className="text-green-500" />
                      {service.completedOrders} طلب مكتمل
                    </span>
                  </div>
                </div>
                <button className="px-6 py-2 bg-purple-100 text-purple-700 rounded-lg hover:bg-purple-200 transition font-semibold">
                  متابعة
                </button>
              </div>

              {/* Description */}
              <div className="mt-6">
                <h3 className="text-xl font-bold text-gray-800 mb-3">
                  حول هذه الخدمة
                </h3>
                <p className="text-gray-700 leading-relaxed whitespace-pre-wrap">
                  {service.description}
                </p>
              </div>
            </div>

            {/* Reviews Preview */}
            <div className="bg-white rounded-xl shadow p-6">
              <h3 className="text-xl font-bold text-gray-800 mb-4">
                تقييمات العملاء
              </h3>

              <div className="space-y-4">
                {[1, 2, 3].map((review) => (
                  <div key={review} className="pb-4 border-b last:border-b-0">
                    <div className="flex items-start justify-between mb-2">
                      <div className="flex items-center gap-2">
                        <div className="w-10 h-10 rounded-full bg-gradient-to-r from-blue-400 to-purple-400" />
                        <div>
                          <p className="font-semibold text-gray-800">
                            عميل {review}
                          </p>
                          <p className="text-xs text-gray-500">منذ أسبوع</p>
                        </div>
                      </div>
                      <div className="flex gap-1">
                        {[1, 2, 3, 4, 5].map((star) => (
                          <Star
                            key={star}
                            size={14}
                            className="text-yellow-500"
                            fill="currentColor"
                          />
                        ))}
                      </div>
                    </div>
                    <p className="text-sm text-gray-700">
                      خدمة ممتازة جداً، العمل احترافي وسريع، سأتعامل معه مجددًا
                    </p>
                  </div>
                ))}
              </div>
            </div>
          </div>

          {/* Right Column - Packages & CTA */}
          <div className="space-y-6">
            {/* Package Selection */}
            <div className="bg-white rounded-xl shadow p-6">
              <h3 className="text-xl font-bold text-gray-800 mb-4">
                اختر الباقة المناسبة
              </h3>

              <div className="space-y-3">
                {service.packages.map((pkg) => (
                  <button
                    key={pkg.id}
                    onClick={() => setSelectedPackage(pkg.id)}
                    className={`w-full p-4 rounded-lg border-2 text-right transition ${
                      selectedPackage === pkg.id
                        ? "border-purple-700 bg-purple-50"
                        : "border-gray-200 bg-gray-50 hover:border-gray-300"
                    }`}
                  >
                    <div className="flex justify-between items-start mb-2">
                      <div>
                        <p className="font-bold text-gray-800">{pkg.name}</p>
                        <p className="text-xs text-gray-600">{pkg.description}</p>
                      </div>
                      <p className="text-lg font-bold text-purple-700">
                        {pkg.price} EGP
                      </p>
                    </div>
                  </button>
                ))}
              </div>
            </div>

            {/* Selected Package Details */}
            {currentPackage && (
              <div className="bg-white rounded-xl shadow p-6 sticky top-24">
                <h3 className="text-xl font-bold text-gray-800 mb-4">
                  {currentPackage.name}
                </h3>

                <div className="space-y-3 mb-6">
                  <div className="p-3 bg-purple-50 rounded-lg">
                    <p className="text-xs text-gray-600 mb-1">السعر</p>
                    <p className="text-2xl font-bold text-purple-700">
                      {currentPackage.price} EGP
                    </p>
                  </div>

                  <div className="grid grid-cols-2 gap-3">
                    <div className="p-3 bg-gray-50 rounded-lg text-center">
                      <Clock size={16} className="text-gray-600 mx-auto mb-1" />
                      <p className="text-xs text-gray-600">التسليم</p>
                      <p className="font-bold text-gray-800">
                        {currentPackage.deliveryDays} أيام
                      </p>
                    </div>
                    <div className="p-3 bg-gray-50 rounded-lg text-center">
                      <RotateCcw size={16} className="text-gray-600 mx-auto mb-1" />
                      <p className="text-xs text-gray-600">تعديلات</p>
                      <p className="font-bold text-gray-800">
                        {currentPackage.revisions === 999
                          ? "∞"
                          : currentPackage.revisions}
                      </p>
                    </div>
                  </div>
                </div>

                <div className="mb-6">
                  <p className="text-sm font-semibold text-gray-800 mb-3">
                    تتضمن الباقة:
                  </p>
                  <ul className="space-y-2">
                    {currentPackage.features.map((feature, idx) => (
                      <li
                        key={idx}
                        className="flex gap-2 text-sm text-gray-700 items-start"
                      >
                        <CheckCircle
                          size={14}
                          className="text-green-500 flex-shrink-0 mt-0.5"
                        />
                        <span>{feature}</span>
                      </li>
                    ))}
                  </ul>
                </div>

                <div className="space-y-3">
                  <button className="w-full py-3 bg-gradient-to-r from-purple-700 to-purple-600 text-white rounded-lg hover:shadow-lg transition font-semibold flex items-center justify-center gap-2">
                    <ShoppingCart size={18} /> اطلب الآن
                  </button>

                  <button className="w-full py-3 rounded-lg font-semibold text-purple-700 bg-purple-50 hover:bg-purple-100 transition flex items-center justify-center gap-2">
                    <MessageCircle size={18} /> تواصل قبل الطلب
                  </button>
                </div>
              </div>
            )}

            {/* Safety Badge */}
            <div className="bg-green-50 border border-green-200 rounded-lg p-4">
              <div className="flex gap-3">
                <AlertCircle size={20} className="text-green-600 flex-shrink-0" />
                <div className="text-sm text-green-700">
                  <p className="font-semibold mb-1">✓ محمي بضمان Khadma Hub</p>
                  <p>أموالك آمنة وسيتم الدفع عند استقبال العمل</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
