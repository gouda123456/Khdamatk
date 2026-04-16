import React, { useState } from "react";
import {
  Server,
  Database,
  Cloud,
  Settings,
  Code,
  Shield,
  Upload,
  Download,
  BarChart3,
  AlertCircle,
  CheckCircle,
} from "lucide-react";

export default function ServerPage() {
  const [selectedServer, setSelectedServer] = useState(null);

  const servers = [
    {
      id: 1,
      name: "خادم التطوير",
      type: "Development",
      status: "active",
      uptime: "99.8%",
      storage: "500 GB",
      bandwidth: "1 TB/month",
      cpu: "4 cores",
      ram: "16 GB",
      price: "500 EGP/month",
      features: [
        "قاعدة بيانات مجانية",
        "شهادة SSL",
        "دعم 24/7",
        "نسخ احتياطية يومية",
      ],
    },
    {
      id: 2,
      name: "خادم الإنتاج",
      type: "Production",
      status: "active",
      uptime: "99.99%",
      storage: "2 TB",
      bandwidth: "10 TB/month",
      cpu: "8 cores",
      ram: "32 GB",
      price: "2000 EGP/month",
      features: [
        "أداء عالي",
        "شهادة SSL",
        "دعم أولوي",
        "نسخ احتياطية كل ساعة",
      ],
    },
    {
      id: 3,
      name: "خادم الاختبار",
      type: "Testing",
      status: "inactive",
      uptime: "98.5%",
      storage: "750 GB",
      bandwidth: "5 TB/month",
      cpu: "6 cores",
      ram: "24 GB",
      price: "1200 EGP/month",
      features: [
        "بيئة معزولة",
        "شهادة SSL",
        "دعم قياسي",
        "نسخ احتياطية أسبوعية",
      ],
    },
  ];

  const stats = [
    { label: "الخوادم النشطة", value: "2", icon: Server },
    { label: "إجمالي التخزين", value: "3.25 TB", icon: Database },
    { label: "النطاق الترددي", value: "16 TB/month", icon: Cloud },
    { label: "وقت التشغيل", value: "99.89%", icon: BarChart3 },
  ];

  return (
    <div dir="rtl" className="bg-gray-100 min-h-screen font-sans">
      {/* Header */}
      <header className="bg-white shadow sticky top-0 z-50">
        <div className="max-w-7xl mx-auto px-4 py-3 flex items-center justify-between">
          <div className="text-xl font-bold">
            <span className="text-purple-700">Khadma</span>{" "}
            <span className="text-yellow-500">Hub</span>
          </div>
          <h2 className="text-2xl font-bold text-gray-800">إدارة الخوادم</h2>
          <nav className="flex gap-4">
            <button className="text-purple-700 hover:text-purple-900">
              إعدادات
            </button>
          </nav>
        </div>
      </header>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto p-4 py-8">
        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-8">
          {stats.map((stat, idx) => {
            const Icon = stat.icon;
            return (
              <div key={idx} className="bg-white rounded-xl shadow p-6">
                <div className="flex items-center justify-between">
                  <div>
                    <p className="text-gray-600 text-sm">{stat.label}</p>
                    <p className="text-3xl font-bold text-purple-700 mt-2">
                      {stat.value}
                    </p>
                  </div>
                  <Icon size={32} className="text-purple-400" />
                </div>
              </div>
            );
          })}
        </div>

        {/* Servers List */}
        <div className="space-y-6">
          <div className="flex justify-between items-center">
            <h2 className="text-2xl font-bold">الخوادم المتاحة</h2>
            <button className="bg-purple-700 text-white px-6 py-2 rounded-lg hover:bg-purple-800 flex items-center gap-2">
              <Server size={18} /> إضافة خادم جديد
            </button>
          </div>

          {servers.map((server) => (
            <div
              key={server.id}
              onClick={() => setSelectedServer(server.id)}
              className="bg-white rounded-xl shadow hover:shadow-lg transition cursor-pointer p-6"
            >
              <div className="flex items-start justify-between mb-4">
                <div>
                  <div className="flex items-center gap-3">
                    <Server size={24} className="text-purple-700" />
                    <div>
                      <h3 className="text-xl font-bold">{server.name}</h3>
                      <p className="text-gray-600">{server.type}</p>
                    </div>
                  </div>
                </div>
                <div className="flex items-center gap-2">
                  {server.status === "active" ? (
                    <>
                      <CheckCircle className="text-green-500" size={24} />
                      <span className="text-green-600 font-semibold">نشط</span>
                    </>
                  ) : (
                    <>
                      <AlertCircle className="text-gray-400" size={24} />
                      <span className="text-gray-600 font-semibold">غير نشط</span>
                    </>
                  )}
                </div>
              </div>

              {/* Server Specs */}
              <div className="grid grid-cols-2 md:grid-cols-5 gap-4 mb-4">
                <div className="bg-gray-50 p-3 rounded-lg">
                  <p className="text-xs text-gray-600">المعالج</p>
                  <p className="font-semibold">{server.cpu}</p>
                </div>
                <div className="bg-gray-50 p-3 rounded-lg">
                  <p className="text-xs text-gray-600">الذاكرة</p>
                  <p className="font-semibold">{server.ram}</p>
                </div>
                <div className="bg-gray-50 p-3 rounded-lg">
                  <p className="text-xs text-gray-600">التخزين</p>
                  <p className="font-semibold">{server.storage}</p>
                </div>
                <div className="bg-gray-50 p-3 rounded-lg">
                  <p className="text-xs text-gray-600">النطاق</p>
                  <p className="font-semibold">{server.bandwidth}</p>
                </div>
                <div className="bg-gray-50 p-3 rounded-lg">
                  <p className="text-xs text-gray-600">وقت التشغيل</p>
                  <p className="font-semibold">{server.uptime}</p>
                </div>
              </div>

              {/* Features and Price */}
              <div className="flex justify-between items-end">
                <div>
                  <p className="text-sm font-semibold mb-2">المميزات:</p>
                  <div className="flex flex-wrap gap-2">
                    {server.features.map((feature, idx) => (
                      <span
                        key={idx}
                        className="text-xs bg-purple-100 text-purple-700 px-2 py-1 rounded"
                      >
                        ✓ {feature}
                      </span>
                    ))}
                  </div>
                </div>
                <div className="text-right">
                  <p className="text-2xl font-bold text-purple-700">
                    {server.price}
                  </p>
                  <button className="bg-gradient-to-r from-purple-700 to-purple-600 text-white px-4 py-2 rounded-lg mt-2 hover:shadow-lg transition">
                    الاشتراك
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>

        {/* Server Details Panel */}
        {selectedServer && (
          <div className="bg-white rounded-xl shadow p-6 mt-8">
            <h2 className="text-2xl font-bold mb-6">تفاصيل الخادم</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-semibold mb-1">
                    عنوان IP
                  </label>
                  <input
                    type="text"
                    value="192.168.1.100"
                    disabled
                    className="w-full bg-gray-100 border rounded p-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-semibold mb-1">
                    تاريخ البدء
                  </label>
                  <input
                    type="text"
                    value="2026-01-15"
                    disabled
                    className="w-full bg-gray-100 border rounded p-2"
                  />
                </div>
              </div>
              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-semibold mb-1">
                    اسم المستخدم
                  </label>
                  <input
                    type="text"
                    value="admin"
                    disabled
                    className="w-full bg-gray-100 border rounded p-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-semibold mb-1">
                    تاريخ الانتهاء
                  </label>
                  <input
                    type="text"
                    value="2027-01-15"
                    disabled
                    className="w-full bg-gray-100 border rounded p-2"
                  />
                </div>
              </div>
            </div>

            <div className="flex gap-4 mt-6">
              <button className="flex-1 bg-purple-700 text-white py-2 rounded-lg font-semibold hover:bg-purple-800 flex items-center justify-center gap-2">
                <Settings size={18} /> الإعدادات
              </button>
              <button className="flex-1 bg-gray-200 text-gray-700 py-2 rounded-lg font-semibold hover:bg-gray-300 flex items-center justify-center gap-2">
                <Download size={18} /> تحميل البيانات
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
