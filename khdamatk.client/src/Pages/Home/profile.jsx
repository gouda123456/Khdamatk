
import React from 'react';

export function ProfilePage() {
  return (
    <div className="min-h-screen bg-[#FAFAFA] text-gray-900 font-sans">
      {/* Navbar */}
      <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" integrity="sha512-z3gLpd7yknf1YoNbCzqRKc4qyor8gaKU1qmn+CShxbuBusANI9QpRohGBreCFkKxLhei6S9CQXFEbbKuqLg0DA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
      <nav className="sticky top-0 z-[100] flex justify-between items-center px-[5%] py-4 bg-white shadow-sm">
        <div className="text-2xl font-bold text-[#7B1FA2]">
          KHADMA <span className="text-[#F59E0B] font-normal border border-[#F59E0B] px-1 rounded">HUB</span>
        </div>
        
        <ul className="hidden md:flex gap-8 font-semibold list-none">
          <li><a href="#services" className="hover:text-[#7B1FA2] transition-colors">Services</a></li>
          <li><a href="#about" className="hover:text-[#7B1FA2] transition-colors">About</a></li>
          <li><a href="#jobs" className="hover:text-[#7B1FA2] transition-colors">Job</a></li>
        </ul>

        <div className="flex items-center gap-6 text-xl">
          <i className="fa-regular fa-comment cursor-pointer"></i>
          <i className="fa-regular fa-bell cursor-pointer"></i>
          <span className="text-base font-medium cursor-pointer">AR</span>
          <div className="w-10 h-10 rounded-full border-2 border-[#7B1FA2] flex items-center justify-center text-[#7B1FA2]">
            <i className="fa-regular fa-user text-lg"></i>
          </div>
        </div>
      </nav>

      {/* Cover Photo */}
      <div className="h-[250px] bg-black relative">
        <div className="absolute top-5 right-[5%] flex gap-4 text-white text-xl">
          <i className="fa-solid fa-trash text-red-500 cursor-pointer"></i>
          <i className="fa-solid fa-pen cursor-pointer"></i>
        </div>
      </div>

      {/* Main Container */}
      <main className="max-w-[900px] mx-auto -mt-[100px] mb-10 bg-white rounded-xl shadow-sm px-8 pb-8 relative">
        
        {/* Profile Header */}
        <div className="flex flex-col items-center relative pb-8 border-b border-gray-200">
          <div className="absolute top-5 right-0 flex gap-4 text-xl cursor-pointer text-gray-500">
            <i className="fa-regular fa-share-from-square"></i>
          </div>

          <div className="relative -mt-[60px] mb-4">
            <div className="w-[150px] h-[150px] bg-gray-300 rounded-full border-4 border-white overflow-hidden">
              {/* Image would go here */}
            </div>
            <div className="absolute bottom-2.5 w-full flex justify-between px-2.5 text-xl">
              <i className="fa-solid fa-trash text-red-500 cursor-pointer"></i>
              <i className="fa-solid fa-pen text-gray-900 cursor-pointer"></i>
            </div>
          </div>

          <h1 className="text-3xl font-semibold mb-6">Omnia Salah</h1>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 w-full text-center mb-8">
            <div className="flex flex-col gap-2">
              <span className="text-gray-500">Member since 2025 Nov</span>
              <span className="font-medium">Cairo, Egypt <i className="fa-solid fa-location-dot ml-1"></i></span>
            </div>
            <div className="flex flex-col gap-2">
              <div className="text-gray-900">
                {[...Array(5)].map((_, i) => (
                  <i key={i} className="fa-regular fa-star"></i>
                ))} (0)
              </div>
              <span className="font-medium">Software engineer</span>
            </div>
            <div className="flex flex-col gap-2">
              <span className="font-medium">2 years experience</span>
              <span className="text-sm text-gray-500 italic">Working 3 hours a week<br />as a freelancer</span>
            </div>
          </div>

          <button className="bg-[#7B1FA2] text-white px-8 py-3 rounded-lg text-lg font-medium hover:opacity-90 transition-opacity">
            Contact me
          </button>
        </div>

        {/* Pricing */}
        <div className="flex justify-between py-4 border-b border-gray-200 font-medium">
          <span>Average per hour</span>
          <span>50 EG/HR</span>
        </div>

        {/* Bio */}
        <p className="py-6 text-gray-800 text-[0.95rem] leading-relaxed">
          Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et
          dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex
          ea commodo consequat.
        </p>

        {/* Skills Section */}
        <section className="mb-8">
          <div className="flex justify-between items-center mt-6 mb-4">
            <div className="text-xl font-semibold flex items-center gap-2">
              <i className="fa-solid fa-chevron-right text-sm"></i> Skills
            </div>
            <i className="fa-regular fa-pen-to-square text-xl text-gray-400 cursor-pointer"></i>
          </div>
          <div className="flex flex-wrap gap-2">
            <span className="bg-gray-200 px-4 py-1.5 rounded-full text-[0.85rem]">Node.js</span>
            <span className="bg-gray-200 px-4 py-1.5 rounded-full text-[0.85rem]">React.js</span>
            <span className="bg-gray-200 px-4 py-1.5 rounded-full text-[0.85rem]">DDD Architecture</span>
          </div>
        </section>

        {/* Portfolio Section */}
        <section className="mb-8">
          <div className="flex justify-between items-center mt-6 mb-4">
            <div className="text-xl font-semibold flex items-center gap-2">
              <i className="fa-solid fa-chevron-right text-sm"></i> Previous work
            </div>
            <div className="bg-gray-100 w-10 h-8 flex items-center justify-center rounded-md text-[#7B1FA2] cursor-pointer">
              <i className="fa-solid fa-plus"></i>
            </div>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {[1, 2].map((i) => (
              <div key={i} className="border border-gray-200 rounded-lg overflow-hidden bg-white shadow-sm">
                <div className="h-[140px] bg-black bg-[url('https://via.placeholder.com/400x200/222/555?text=Project')] bg-cover"></div>
                <div className="p-4">
                  <div className="flex justify-between items-center mb-2 font-semibold text-gray-900">
                    <span>Project Name</span>
                    <i className="fa-regular fa-pen-to-square text-gray-400 cursor-pointer"></i>
                  </div>
                  <div className="flex gap-2">
                    <span className="bg-gray-200 px-3 py-1 rounded-full text-[0.75rem]">UI</span>
                    <span className="bg-gray-200 px-3 py-1 rounded-full text-[0.75rem]">UX</span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </section>

        {/* Timeline Sections (Education, Certification, Experience) */}
        {['Educational', 'certification', 'Experience'].map((title, idx) => (
          <section key={idx} className="mb-8">
            <div className="flex justify-between items-center mt-6 mb-4">
              <div className="text-xl font-semibold flex items-center gap-2">
                <i className="fa-solid fa-chevron-right text-sm"></i> {title}
              </div>
              <div className="bg-gray-100 w-10 h-8 flex items-center justify-center rounded-md text-[#7B1FA2] cursor-pointer">
                <i className="fa-solid fa-plus"></i>
              </div>
            </div>
            <div className="flex flex-col md:flex-row gap-6 pl-4 border-l-2 border-gray-100 ml-2">
              <div className="text-3xl text-gray-900 mt-2 shrink-0">
                <i className={`fa-solid ${title === 'Educational' ? 'fa-graduation-cap' : title === 'certification' ? 'fa-certificate' : 'fa-user-tie'}`}></i>
              </div>
              <div className="flex-1">
                <div className="flex justify-between items-start mb-2">
                  <span className="text-lg font-semibold italic">"{title} Name"</span>
                  <i className="fa-regular fa-pen-to-square text-xl text-gray-400 cursor-pointer"></i>
                </div>
                {title === 'Educational' && <div className="font-medium mb-2">"specialty"</div>}
                <p className="text-gray-800 text-[0.95rem] mb-4 leading-relaxed">
                  Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                </p>
                <div className="font-medium text-gray-700">2022 /2/1 - 2026/5/1</div>
              </div>
            </div>
          </section>
        ))}
      </main>

      {/* Footer */}
      <footer className="bg-[#4A148C] text-white py-12 px-[5%] mt-16">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-12">
          <div>
            <div className="text-2xl font-bold mb-4">
              KHADMA <span className="text-[#F59E0B] border border-[#F59E0B] px-1 rounded">HUB</span>
            </div>
            <p className="text-sm mb-2 text-gray-300">Social media</p>
            <div className="flex gap-4 text-2xl mt-4">
              <i className="fa-brands fa-facebook text-[#1877F2] cursor-pointer"></i>
              <i className="fa-brands fa-google text-[#DB4437] cursor-pointer"></i>
              <i className="fa-brands fa-apple text-white cursor-pointer"></i>
            </div>
          </div>
          
          <div>
            <h4 className="font-semibold mb-4 text-lg">About</h4>
            <ul className="space-y-2 text-gray-300 text-sm">
              <li className="hover:text-white cursor-pointer transition-colors">About Us</li>
              <li className="hover:text-white cursor-pointer transition-colors">Why KHADMA HUB</li>
              <li className="hover:text-white cursor-pointer transition-colors">Reviews & Testimonials</li>
              <li className="hover:text-white cursor-pointer transition-colors">How KHADMA HUB work</li>
            </ul>
          </div>

          <div>
            <h4 className="font-semibold mb-4 text-lg">Find jobs</h4>
            <ul className="space-y-2 text-gray-300 text-sm">
              <li className="hover:text-white cursor-pointer transition-colors">Development jobs</li>
              <li className="hover:text-white cursor-pointer transition-colors">Writing jobs</li>
              <li className="hover:text-white cursor-pointer transition-colors">Designers jobs</li>
              <li className="hover:text-white cursor-pointer transition-colors">Sales jobs</li>
            </ul>
          </div>

          <div>
            <h4 className="font-semibold mb-4 text-lg">Sub-Heading</h4>
            <div className="border border-white rounded p-3 flex justify-between items-center mt-4 cursor-pointer hover:bg-white hover:text-[#4A148C] transition-all">
              <span className="font-medium">Button Text</span>
              <i className="fa-solid fa-globe"></i>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
}

export default ProfilePage;