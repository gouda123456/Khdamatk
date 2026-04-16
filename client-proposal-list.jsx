import React, { useState } from "react";
import {
  Search,
  Filter,
  MessageCircle,
  Star,
  MapPin,
  DollarSign,
  Clock,
  CheckCircle,
  AlertCircle,
  Eye,
  Trash2,
} from "lucide-react";

export default function ClientProposalList() {
  const [filter, setFilter] = useState("all");
  const [search, setSearch] = useState("");
  const [expandedId, setExpandedId] = useState(null);

  const proposals = [
    {
      id: 1,
      freelancer: "أحمد علي",
      freelancerRating: 4.9,
      freelancerImage: "",
      title: "تصميم واجهة الموقع",
      proposalText:
        "أنا مصمم UI/UX محترف مع خبرة 5 سنوات. لدي خبرة واسعة في Figma و Adobe XD. سأقوم بتسليم التصاميم بجودة عالية على الموعد المحدد.",
      price: 3500,
      duration: "3 أسابيع",
      rating: 5,
      status: "pending",
      date: "2026-03-01",
      skills: ["UI Design", "Figma", "Web Design"],
    },
    {
      id: 2,
      freelancer: "منى حسن",
      freelancerRating: 4.7,
      freelancerImage: "",
      title: "تصميم واجهة الموقع",
      proposalText:
        "مصممة جرافيكس متخصصة في تصميم الواجهات الحديثة والاحترافية. لدي مشاريع سابقة رائعة يمكنك رؤيتها في محفظتي.",
      price: 2800,
      duration: "2.5 أسابيع",
      rating: 4,
      status: "accepted",
      date: "2026-02-28",
      skills: ["UI Design", "UX Design", "Adobe XD"],
    },
    {
      id: 3,
      freelancer: "علي محمود",
      freelancerRating: 4.8,
      freelancerImage: "",
      title: "تصميم واجهة الموقع",
      proposalText:
        "خبرة 7 سنوات في تصميم الواجهات. أستطيع تسليم تصاميم احترافية مع عدة نسخ للأختيار من بينها.",
      price: 4200,
      duration: "4 أسابيع",
      rating: 5,
      status: "rejected",
      date: "2026-02-27",
      skills: ["Web Design", "UI Design", "Prototyping"],
    },
    {
      id: 4,
      freelancer: "سارة محمد",
      freelancerRating: 4.6,
      freelancerImage: "",
      title: "تصميم واجهة الموقع",
      proposalText:
        "مصممة ويب ومتخصصة في التصاميم المسؤولة. أعمل على أحدث الأدوات والتقنيات لضمان أفضل النتائج.",
      price: 3000,
      duration: "3 أسابيع",
      rating: 4,
      status: "pending",
      date: "2026-03-01",
      skills: ["Responsive Design", "UI Design", "Figma"],
    },
  ];

  const filtered = proposals.filter((proposal) => {
    const matchesSearch =
      proposal.freelancer.toLowerCase().includes(search.toLowerCase()) ||
      proposal.proposalText.toLowerCase().includes(search.toLowerCase());
    const matchesFilter = filter === "all" || proposal.status === filter;
    return matchesSearch && matchesFilter;
  });

  const getStatusColor = (status) => {
    switch (status) {
      case "pending":
        return "bg-yellow-100 text-yellow-700";
      case "accepted":
        return "bg-green-100 text-green-700";
      case "rejected":
        return "bg-red-100 text-red-700";
      default:
        return "bg-gray-100 text-gray-700";
    }
  };

  const getStatusLabel = (status) => {
    switch (status) {
      case "pending":
        return "قيد الانتظار";
      case "accepted":
        return "مقبول";
      case "rejected":
        return "مرفوض";
      default:
        return status;
    }
  };

  const getStatusIcon = (status) => {
    switch (status) {
      case "pending":
        return <AlertCircle size={16} />;
      case "accepted":
        return <CheckCircle size={16} />;
      case "rejected":
        return <AlertCircle size={16} />;
      default:
        return null;
    }
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
          <h2 className="text-2xl font-bold text-gray-800">قائمة العروض</h2>
          <div className="flex items-center gap-4">
            <MessageCircle className="cursor-pointer hover:text-purple-700" />
          </div>
        </div>
      </header>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto p-4 py-8">
        {/* Search and Filter */}
        <div className="mb-6 space-y-4">
          <div className="flex gap-4">
            <div className="flex-1 relative">
              <Search className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400" />
              <input
                type="text"
                placeholder="ابحث عن الفريلانسر أو الخدمة..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                className="w-full pr-10 pl-4 py-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
              />
            </div>
            <button className="bg-white border rounded-lg px-4 py-3 hover:bg-gray-50 flex items-center gap-2">
              <Filter size={18} /> تصفية
            </button>
          </div>

          {/* Filter Buttons */}
          <div className="flex gap-2 flex-wrap">
            <button
              onClick={() => setFilter("all")}
              className={`px-4 py-2 rounded-lg font-semibold transition ${
                filter === "all"
                  ? "bg-purple-700 text-white"
                  : "bg-white text-gray-600 hover:bg-gray-50"
              }`}
            >
              الكل ({proposals.length})
            </button>
            <button
              onClick={() => setFilter("pending")}
              className={`px-4 py-2 rounded-lg font-semibold transition ${
                filter === "pending"
                  ? "bg-yellow-500 text-white"
                  : "bg-white text-gray-600 hover:bg-gray-50"
              }`}
            >
              قيد الانتظار (
              {proposals.filter((p) => p.status === "pending").length})
            </button>
            <button
              onClick={() => setFilter("accepted")}
              className={`px-4 py-2 rounded-lg font-semibold transition ${
                filter === "accepted"
                  ? "bg-green-600 text-white"
                  : "bg-white text-gray-600 hover:bg-gray-50"
              }`}
            >
              مقبول ({proposals.filter((p) => p.status === "accepted").length})
            </button>
            <button
              onClick={() => setFilter("rejected")}
              className={`px-4 py-2 rounded-lg font-semibold transition ${
                filter === "rejected"
                  ? "bg-red-600 text-white"
                  : "bg-white text-gray-600 hover:bg-gray-50"
              }`}
            >
              مرفوض ({proposals.filter((p) => p.status === "rejected").length})
            </button>
          </div>
        </div>

        {/* Proposals List */}
        <div className="space-y-4">
          {filtered.length === 0 ? (
            <div className="bg-white rounded-xl shadow p-8 text-center">
              <p className="text-gray-500 text-lg">لا توجد عروض مطابقة</p>
            </div>
          ) : (
            filtered.map((proposal) => (
              <div
                key={proposal.id}
                className="bg-white rounded-xl shadow hover:shadow-md transition"
              >
                {/* Header */}
                <div className="p-6 border-b">
                  <div className="flex items-start justify-between mb-4">
                    <div className="flex items-start gap-4 flex-1">
                      <div className="w-16 h-16 rounded-full bg-gradient-to-r from-purple-400 to-pink-400 flex-shrink-0" />
                      <div className="flex-1">
                        <div className="flex items-center gap-2 mb-1">
                          <h3 className="text-xl font-bold">
                            {proposal.freelancer}
                          </h3>
                          <div className="flex items-center gap-1 text-yellow-500">
                            <Star size={14} fill="currentColor" />
                            <span className="text-sm">
                              {proposal.freelancerRating}
                            </span>
                          </div>
                        </div>
                        <p className="text-gray-600 text-sm mb-2">
                          {proposal.title}
                        </p>
                        <div className="flex gap-4 text-sm text-gray-600">
                          <span className="flex items-center gap-1">
                            <DollarSign size={14} /> {proposal.price} EGP
                          </span>
                          <span className="flex items-center gap-1">
                            <Clock size={14} /> {proposal.duration}
                          </span>
                          <span className="flex items-center gap-1">
                            <Star size={14} /> {proposal.rating} نجم
                          </span>
                        </div>
                      </div>
                    </div>
                    <div className={`px-3 py-1 rounded-full text-sm font-semibold flex items-center gap-1 ${getStatusColor(proposal.status)}`}>
                      {getStatusIcon(proposal.status)}
                      {getStatusLabel(proposal.status)}
                    </div>
                  </div>

                  {/* Skills */}
                  <div className="flex gap-2">
                    {proposal.skills.map((skill, idx) => (
                      <span
                        key={idx}
                        className="bg-gray-100 text-gray-700 text-xs px-2 py-1 rounded"
                      >
                        {skill}
                      </span>
                    ))}
                  </div>
                </div>

                {/* Expandable Section */}
                <div
                  onClick={() =>
                    setExpandedId(expandedId === proposal.id ? null : proposal.id)
                  }
                  className="p-6 cursor-pointer hover:bg-gray-50"
                >
                  <p className="text-gray-700 mb-3 text-sm">
                    {proposal.proposalText}
                  </p>
                  <p className="text-xs text-gray-500 mb-3">
                    تاريخ العرض: {proposal.date}
                  </p>

                  {expandedId === proposal.id && (
                    <div className="space-y-4 mt-4 pt-4 border-t">
                      <div className="bg-gray-50 p-4 rounded-lg">
                        <p className="text-sm font-semibold mb-2 text-gray-700">
                          ملفات المشروع:
                        </p>
                        <div className="flex gap-2">
                          <button className="text-xs bg-purple-100 text-purple-700 px-3 py-1 rounded hover:bg-purple-200">
                            📎 portfolio.pdf
                          </button>
                          <button className="text-xs bg-purple-100 text-purple-700 px-3 py-1 rounded hover:bg-purple-200">
                            🔗 رابط المحفظة
                          </button>
                        </div>
                      </div>
                    </div>
                  )}
                </div>

                {/* Actions */}
                <div className="p-6 bg-gray-50 flex gap-3 border-t">
                  {proposal.status === "pending" && (
                    <>
                      <button className="flex-1 bg-green-600 text-white py-2 rounded-lg font-semibold hover:bg-green-700 flex items-center justify-center gap-2">
                        <CheckCircle size={18} /> قبول العرض
                      </button>
                      <button className="flex-1 bg-red-600 text-white py-2 rounded-lg font-semibold hover:bg-red-700 flex items-center justify-center gap-2">
                        <AlertCircle size={18} /> رفض
                      </button>
                    </>
                  )}
                  <button className="flex-1 bg-white border border-purple-700 text-purple-700 py-2 rounded-lg font-semibold hover:bg-purple-50 flex items-center justify-center gap-2">
                    <MessageCircle size={18} /> رسالة
                  </button>
                  <button className="px-4 py-2 bg-white border rounded-lg hover:bg-gray-200">
                    <Eye size={18} />
                  </button>
                  {proposal.status === "pending" && (
                    <button className="px-4 py-2 bg-white border border-red-300 text-red-600 rounded-lg hover:bg-red-50">
                      <Trash2 size={18} />
                    </button>
                  )}
                </div>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
}
