import React, { useState } from "react";
import {
  Calendar,
  DollarSign,
  MapPin,
  Briefcase,
  Clock,
  User,
  Star,
  Send,
  Share2,
  Heart,
} from "lucide-react";

export default function JobDetails() {
  const [isSaved, setIsSaved] = useState(false);
  const [showProposal, setShowProposal] = useState(false);

  const job = {
    id: 1,
    title: "تصميم واجهة موقع إلكتروني احترافية",
    client: "أحمد محمود",
    clientRating: 4.8,
    clientReviews: 156,
    description:
      "نحتاج إلى تصميم واجهة موقع إلكتروني متكامل بتصميم عصري واحترافي. المشروع يشمل تصميم الصفحات الرئيسية والداخلية مع التركيز على تجربة المستخدم.",
    skills: ["UI Design", "Figma", "Adobe XD", "Web Design"],
    price: "2000-5000 EGP",
    duration: "3-5 أسابيع",
    level: "متوسط إلى عالي",
    proposals: 12,
    timeline: "2026-03-15",
    category: "تصميم",
    details: [
      "عدد الصفحات: 8 صفحات",
      "التصميم يجب أن يكون responsive",
      "تسليم ملفات PSD و Figma",
      "تصاميم بسيطة ونظيفة",
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
          <div className="flex items-center gap-4">
            <button className="text-purple-700 font-semibold">رجوع</button>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <div className="max-w-4xl mx-auto p-4 py-8">
        {/* Job Header */}
        <div className="bg-white rounded-xl shadow p-6 mb-6">
          <div className="flex justify-between items-start mb-4">
            <div>
              <h1 className="text-3xl font-bold mb-2">{job.title}</h1>
              <div className="flex items-center gap-4">
                <span className="flex items-center gap-1">
                  <User size={16} /> {job.category}
                </span>
                <span className="flex items-center gap-1">
                  <Briefcase size={16} /> {job.level}
                </span>
              </div>
            </div>
            <button
              onClick={() => setIsSaved(!isSaved)}
              className={`flex items-center gap-2 px-4 py-2 rounded-lg ${
                isSaved
                  ? "bg-red-100 text-red-700"
                  : "bg-gray-100 text-gray-700"
              }`}
            >
              <Heart size={18} fill={isSaved ? "currentColor" : "none"} />
              حفظ
            </button>
          </div>

          {/* Client Info */}
          <div className="bg-purple-50 p-4 rounded-lg mb-6">
            <div className="flex items-center gap-3 mb-2">
              <div className="w-12 h-12 rounded-full bg-gradient-to-r from-purple-400 to-pink-400" />
              <div>
                <p className="font-semibold">{job.client}</p>
                <p className="text-sm text-gray-600 flex items-center gap-1">
                  <Star size={14} className="text-yellow-500" fill="currentColor" />
                  {job.clientRating} ({job.clientReviews} تقييم)
                </p>
              </div>
            </div>
          </div>

          {/* Job Stats */}
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
            <div className="bg-gray-50 p-3 rounded-lg text-center">
              <p className="text-sm text-gray-600">الميزانية</p>
              <p className="font-bold flex items-center justify-center gap-1">
                <DollarSign size={16} /> {job.price}
              </p>
            </div>
            <div className="bg-gray-50 p-3 rounded-lg text-center">
              <p className="text-sm text-gray-600">المدة الزمنية</p>
              <p className="font-bold flex items-center justify-center gap-1">
                <Clock size={16} /> {job.duration}
              </p>
            </div>
            <div className="bg-gray-50 p-3 rounded-lg text-center">
              <p className="text-sm text-gray-600">التسليم</p>
              <p className="font-bold flex items-center justify-center gap-1">
                <Calendar size={16} /> 15-03-2026
              </p>
            </div>
            <div className="bg-gray-50 p-3 rounded-lg text-center">
              <p className="text-sm text-gray-600">العروض</p>
              <p className="font-bold">{job.proposals} عرض</p>
            </div>
          </div>
        </div>

        {/* Job Description */}
        <div className="bg-white rounded-xl shadow p-6 mb-6">
          <h2 className="text-2xl font-bold mb-3">وصف المشروع</h2>
          <p className="text-gray-700 mb-4">{job.description}</p>

          <h3 className="text-xl font-semibold mb-3">متطلبات المشروع:</h3>
          <ul className="list-disc list-inside space-y-2 mb-4">
            {job.details.map((detail, idx) => (
              <li key={idx} className="text-gray-700">
                {detail}
              </li>
            ))}
          </ul>

          <h3 className="text-xl font-semibold mb-3">المهارات المطلوبة:</h3>
          <div className="flex flex-wrap gap-2">
            {job.skills.map((skill, idx) => (
              <span
                key={idx}
                className="bg-purple-100 text-purple-700 px-3 py-1 rounded-full text-sm"
              >
                {skill}
              </span>
            ))}
          </div>
        </div>

        {/* Action Buttons */}
        <div className="bg-white rounded-xl shadow p-6">
          <button
            onClick={() => setShowProposal(!showProposal)}
            className="w-full bg-gradient-to-r from-purple-700 to-purple-600 text-white py-3 rounded-lg font-semibold hover:shadow-lg transition mb-3"
          >
            <Send size={18} className="inline mr-2" /> قدم عرضاً الآن
          </button>

          <button className="w-full bg-gray-200 text-gray-700 py-3 rounded-lg font-semibold hover:bg-gray-300 transition flex items-center justify-center gap-2">
            <Share2 size={18} /> شارك المشروع
          </button>
        </div>

        {/* Proposal Form */}
        {showProposal && (
          <div className="bg-white rounded-xl shadow p-6 mt-6">
            <h2 className="text-2xl font-bold mb-4">قدم عرضك</h2>
            <form className="space-y-4">
              <div>
                <label className="block font-semibold mb-2">سعر العرض (EGP)</label>
                <input
                  type="number"
                  placeholder="أدخل سعرك"
                  className="w-full border rounded-lg p-3"
                />
              </div>
              <div>
                <label className="block font-semibold mb-2">المدة الزمنية</label>
                <input
                  type="text"
                  placeholder="عدد الأيام أو الأسابيع"
                  className="w-full border rounded-lg p-3"
                />
              </div>
              <div>
                <label className="block font-semibold mb-2">رسالتك</label>
                <textarea
                  placeholder="اكتب رسالتك للعميل..."
                  rows="4"
                  className="w-full border rounded-lg p-3"
                />
              </div>
              <button
                type="submit"
                className="w-full bg-purple-700 text-white py-3 rounded-lg font-semibold hover:bg-purple-800"
              >
                إرسال العرض
              </button>
            </form>
          </div>
        )}
      </div>
    </div>
  );
}
