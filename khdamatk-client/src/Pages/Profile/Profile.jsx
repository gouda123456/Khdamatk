import React from 'react'

export default function Profile() {
  return (
     <>
  
    <div className="min-h-screen bg-gray-50 font-sans text-gray-800 pt-15">
      <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css"></link>
      
     

      {/* --- Main Content --- */}
      <main className="max-w-4xl mx-auto my-12 bg-white shadow-2xl rounded-3xl overflow-hidden relative">
        
        {/* Banner Section */}
        <div className="h-48 bg-black relative">
           <div className="absolute top-6 right-6 flex gap-5 text-white text-xl">
             <i className="fa-solid fa-trash cursor-pointer hover:text-red-500 transition"></i>
             <i className="fa-solid fa-pen cursor-pointer hover:text-blue-500 transition"></i>
           </div>
        </div>

        {/* Profile Header Info */}
        <div className="px-12 pb-10 relative">
          {/* Avatar */}
          <div className="flex justify-center -mt-24 mb-6 relative">
             <div className="w-44 h-44 rounded-full bg-gray-300 border-8 border-white shadow-xl flex items-center justify-center overflow-hidden">
               <i className="fa-solid fa-user text-8xl text-gray-400 mt-6"></i>
             </div>
             <div className="absolute top-24 left-[calc(50%-6.5rem)] text-red-500 cursor-pointer bg-white p-2 rounded-full shadow-md border border-gray-100">
                <i className="fa-solid fa-trash text-sm"></i>
             </div>
             <div className="absolute top-24 right-[calc(50%-6.5rem)] text-gray-600 cursor-pointer bg-white p-2 rounded-full shadow-md border border-gray-100">
                <i className="fa-solid fa-pen text-sm"></i>
             </div>
             <div className="absolute top-4 right-4 text-gray-500 cursor-pointer hover:text-purple-700">
                <i className="fa-regular fa-pen-to-square text-2xl"></i>
             </div>
          </div>

          <div className="text-center mb-10">
            <h1 className="text-4xl font-black text-gray-900 mb-6">Omnia Salah</h1>
            
            <div className="grid grid-cols-3 gap-6 text-sm text-gray-600 border-b pb-10 border-gray-100">
                <div className="flex flex-col items-center gap-2 border-r border-gray-200 px-4">
                    <span className="text-xs uppercase tracking-widest text-gray-400">Member since 2025 Nov</span>
                    <div className="flex items-center gap-2 font-bold text-black text-xl">
                        <span>Cairo, Egypt</span>
                        <i className="fa-solid fa-location-dot text-red-500 text-2xl"></i>
                    </div>
                </div>
                <div className="flex flex-col items-center gap-2 border-r border-gray-200 px-4">
                     <div className="flex text-yellow-400 text-lg gap-1">
                        <i className="fa-solid fa-star"></i>
                        <i className="fa-solid fa-star"></i>
                        <i className="fa-solid fa-star"></i>
                        <i className="fa-solid fa-star"></i>
                        <i className="fa-solid fa-star"></i>
                     </div>
                     <span className="font-bold text-black text-xl">Software engineer</span>
                </div>
                <div className="flex flex-col items-center gap-2 px-4">
                    <span className="font-bold text-black text-xl">2 years experience</span>
                    <span className="text-xs font-medium">Working 3 hours a week as a freelancer</span>
                </div>
            </div>
            
            <div className="mt-8">
                <button className="bg-[#7B1FA2] text-white px-14 py-4 rounded-xl text-xl font-bold hover:bg-purple-800 shadow-lg transition-transform active:scale-95">
                    Contact me
                </button>
            </div>
          </div>

          {/* Average per hour & Bio */}
          <div className="border-b border-gray-200 pb-10 mb-10">
              <div className="flex justify-between items-center mb-4">
                  <span className="text-gray-700 font-bold text-xl">Average per hour</span>
                  <span className="text-black font-black text-2xl">50 EG/HR</span>
              </div>
              <p className="text-lg text-gray-500 leading-relaxed text-justify font-medium">
                  Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
              </p>
          </div>

          {/* Skills Section */}
          <div className="mb-10">
            <div className="flex justify-between items-center mb-6">
                <h3 className="font-black text-2xl text-gray-800 flex items-center gap-3">
                    <i className="fa-solid fa-chevron-right text-base text-purple-600"></i> Skills
                </h3>
                <i className="fa-regular fa-pen-to-square text-gray-500 cursor-pointer text-2xl"></i>
            </div>
            <div className="flex flex-wrap gap-4">
                <span className="bg-gray-100 border border-gray-200 text-gray-800 px-8 py-2.5 rounded-full text-lg font-bold">React.js</span>
                <span className="bg-gray-100 border border-gray-200 text-gray-800 px-8 py-2.5 rounded-full text-lg font-bold">Tailwind CSS</span>
            </div>
          </div>

          {/* Portfolio Section */}
          <div className="mb-12 border-b border-gray-200 pb-12">
            <div className="flex justify-between items-center mb-8">
                <h3 className="font-black text-2xl text-gray-800 flex items-center gap-3">
                    <i className="fa-solid fa-chevron-right text-base text-purple-600"></i> Portfolio
                </h3>
                <div className="bg-purple-600 p-3 rounded-xl hover:bg-purple-700 cursor-pointer shadow-md">
                    <i className="fa-solid fa-plus text-white text-xl"></i>
                </div>
            </div>
            
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-10">
                <div className="border border-gray-200 rounded-2xl p-6 shadow-sm hover:shadow-md transition">
                    <div className="h-40 bg-gray-900 rounded-xl mb-4 flex items-center justify-center overflow-hidden">
                        <div className="w-full h-full relative p-4 opacity-50">
                             <div className="grid grid-cols-2 gap-2 h-full">
                                <div className="bg-gray-700 h-full rounded"></div>
                                <div className="bg-gray-700 h-full rounded"></div>
                             </div>
                        </div>
                    </div>
                    <div className="flex justify-between items-start mb-4">
                        <span className="font-black text-xl">Name: Work Project</span>
                        <i className="fa-regular fa-pen-to-square text-gray-400 text-xl cursor-pointer"></i>
                    </div>
                    <div className="flex gap-2">
                        <span className="bg-gray-200 w-12 h-3 rounded-full"></span>
                        <span className="bg-gray-200 w-12 h-3 rounded-full"></span>
                    </div>
                </div>
                <div className="border border-gray-200 rounded-2xl p-6 shadow-sm hover:shadow-md transition">
                    <div className="h-40 bg-gray-900 rounded-xl mb-4 flex items-center justify-center overflow-hidden">
                        <div className="w-full h-full relative p-4 opacity-50">
                             <div className="grid grid-cols-2 gap-2 h-full">
                                <div className="bg-gray-700 h-full rounded"></div>
                                <div className="bg-gray-700 h-full rounded"></div>
                             </div>
                        </div>
                    </div>
                    <div className="flex justify-between items-start mb-4">
                        <span className="font-black text-xl">Name: Work Project</span>
                        <i className="fa-regular fa-pen-to-square text-gray-400 text-xl cursor-pointer"></i>
                    </div>
                    <div className="flex gap-2">
                        <span className="bg-gray-200 w-12 h-3 rounded-full"></span>
                        <span className="bg-gray-200 w-12 h-3 rounded-full"></span>
                    </div>
                </div>
            </div>
          </div>

          {/* Educational Section */}
          <div className="mb-12 border-b border-gray-200 pb-12">
            <div className="flex justify-between items-center mb-8">
                <h3 className="font-black text-2xl text-gray-800 flex items-center gap-3">
                    <i className="fa-solid fa-chevron-right text-base text-purple-600"></i> Educational
                </h3>
                <div className="bg-purple-600 p-3 rounded-xl cursor-pointer shadow-md">
                    <i className="fa-solid fa-plus text-white text-xl"></i>
                </div>
            </div>

            <div className="flex gap-8 items-start bg-gray-50/50 p-6 rounded-2xl border border-gray-100">
                <div className="p-4 bg-white rounded-2xl shadow-sm">
                     <i className="fa-solid fa-graduation-cap text-6xl text-purple-900"></i>
                </div>
                <div className="flex-1">
                    <div className="flex justify-between items-start">
                        <h4 className="font-black text-2xl text-gray-900">Faculty of Engineering</h4>
                        <i className="fa-regular fa-pen-to-square text-gray-400 cursor-pointer text-2xl"></i>
                    </div>
                    <p className="text-xl text-purple-800 font-bold mt-1 mb-3">Computer Science specialty</p>
                    <p className="text-lg text-gray-500 text-justify mb-5 leading-relaxed">
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
                    </p>
                    <span className="text-lg font-black text-gray-800 bg-white border border-gray-200 px-6 py-2 rounded-xl">2022 /2/1 - 2026/5/1</span>
                </div>
            </div>
          </div>

          {/* Certification Section */}
          <div className="mb-12 border-b border-gray-200 pb-12">
            <div className="flex justify-between items-center mb-8">
                <h3 className="font-black text-2xl text-gray-800 flex items-center gap-3">
                    <i className="fa-solid fa-chevron-right text-base text-purple-600"></i> Certification
                </h3>
                <div className="bg-purple-600 p-3 rounded-xl cursor-pointer shadow-md">
                    <i className="fa-solid fa-plus text-white text-xl"></i>
                </div>
            </div>

            <div className="flex gap-8 items-start bg-gray-50/50 p-6 rounded-2xl border border-gray-100">
                <div className="p-4 bg-white rounded-2xl shadow-sm">
                     <i className="fa-solid fa-certificate text-6xl text-purple-900"></i>
                </div>
                <div className="flex-1">
                    <div className="flex justify-between items-start">
                        <h4 className="font-black text-2xl text-gray-900">AWS Certified Developer</h4>
                        <i className="fa-regular fa-pen-to-square text-gray-400 cursor-pointer text-2xl"></i>
                    </div>
                    <p className="text-lg text-gray-500 text-justify mb-5 mt-4 leading-relaxed">
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                    </p>
                    <span className="text-lg font-black text-gray-800 bg-white border border-gray-200 px-6 py-2 rounded-xl">2022 /2/1 - 2026/5/1</span>
                </div>
            </div>
          </div>

          {/* Experience Section */}
          <div className="mb-6">
            <div className="flex justify-between items-center mb-8">
                <h3 className="font-black text-2xl text-gray-800 flex items-center gap-3">
                    <i className="fa-solid fa-chevron-right text-base text-purple-600"></i> Experience
                </h3>
                <div className="bg-purple-600 p-3 rounded-xl cursor-pointer shadow-md">
                    <i className="fa-solid fa-plus text-white text-xl"></i>
                </div>
            </div>

            <div className="flex gap-8 items-start bg-gray-50/50 p-6 rounded-2xl border border-gray-100">
                <div className="p-4 bg-white rounded-2xl shadow-sm">
                     <i className="fa-solid fa-users-viewfinder text-6xl text-purple-900"></i>
                </div>
                <div className="flex-1">
                    <div className="flex justify-between items-start">
                        <h4 className="font-black text-2xl text-gray-900">Senior Software Engineer</h4>
                        <i className="fa-regular fa-pen-to-square text-gray-400 cursor-pointer text-2xl"></i>
                    </div>
                    <p className="text-lg text-gray-500 text-justify mb-2 mt-4 leading-relaxed">
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                    </p>
                </div>
            </div>
          </div>

        </div>
      </main>
    </div>
  
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     </>
  )
}
