 import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import homePhoto from '../../assets/Images/Home.png'
import { faFileInvoiceDollar, faPhoneVolume, faShieldHalved } from '@fortawesome/free-solid-svg-icons'
import { faCircleCheck } from '@fortawesome/free-regular-svg-icons'
import TestimonialCard from '../../Components/TestimonialCard/TestimonialCard'
import WhyChooesUs from '../../Components/WhyChooesUs/WhyChooesUs'
import FreelancerCard from '../../Components/FreelancerCard/FreelancerCard'
import ServiceCategories from '../../Components/ServiceCategories/ServiceCategories'
import Feature from '../../Components/Feature/Feature'
import Freelancer from '../../Components/Freelancer/Freelancer'
import ClientReviews from '../../Components/ClientReviews/ClientReviews'
import { Link } from 'react-router-dom'
 

export default function Home() {
  return (
  <>
  <div className='pt-16 lg:pt-0'>
     <div>
   {/* ===== Hero Section: Hire The Best Talents to Grow Your Business ===== */}
  <div className="grid gap-36 lg:grid-cols-2 items-center  dark:bg-gray-900 
transition-colors duration-300">

  {/* Left-Side */}
  <div className="p-8 space-y-3">
    
    {/* Main Title */}
    <h1 className="font-bold text-2xl sm:text-3xl lg:text-4xl leading-snug 
    text-gray-900 dark:text-gray-100">
      Hire The Best Talents 
      <span className="block lg:inline">
            to Grow Your Business
      </span>
    </h1>
      
    <p className="text-base sm:text-lg lg:text-xl 
    text-gray-700 dark:text-gray-400 
    leading-relaxed">
      khadma hub is a freelance & remote work marketplace with thousands of top-rated freelancers & remote employees. 
      It is simple and quick to post your job for free and get quick proposals. Top companies and start-ups in Egypt hire elharefa freelancers.
    </p>

    {/* Buttons */}
    <div className="flex items-center gap-3">
      
      <Link 
        to={`/post-jop`} 
        className="btn w-fit 
        bg-purple-700 hover:bg-purple-800
        dark:bg-purple-600 dark:hover:bg-purple-500
        text-white transition">
        post jop
      </Link>

      <button className="btn w-fit 
      bg-amber-500 hover:bg-amber-600
      dark:bg-amber-400 dark:hover:bg-amber-300
      text-white transition">
        Get A jop
      </button>

    </div>
  </div>

  {/* Right-Side */}
  <div>
    <img 
      src={homePhoto} 
      className="dark:brightness-90 dark:contrast-110 transition"
    />
  </div>

</div>
   {/* ===== Why Choose Us: Why Hire Freelancers Through Khadma Hub ===== */}
 <div className="space-y-6 p-10 
bg-gray-50 dark:bg-gray-950 
transition-colors duration-300">

  <h2 className="font-bold text-3xl text-center 
  text-gray-900 dark:text-gray-100">
    Why Hire Freelancers Through khadma hub
  </h2>

  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">

    {/* Dedicated Support */}
    <div className="bg-white dark:bg-gray-900 
    p-6 rounded-xl 
    shadow-sm dark:shadow-lg 
    border border-gray-100 dark:border-gray-800
    hover:-translate-y-2 hover:shadow-xl
    transition-all duration-300
    text-center space-y-4">

      <div className="w-12 h-12 mx-auto flex items-center justify-center 
      rounded-full 
      bg-purple-100 dark:bg-purple-500/10 
      text-purple-700 dark:text-purple-400">
        <FontAwesomeIcon className="text-lg" icon={faPhoneVolume} />
      </div>

      <h3 className="font-semibold text-lg 
      text-gray-900 dark:text-gray-100">
        Dedicated Support
      </h3>

      <p className="text-sm 
      text-gray-600 dark:text-gray-400">
        Managers are always there to help you out with any kind of challenges
      </p>
    </div>

    {/* Pay Safe */}
    <div className="bg-white dark:bg-gray-900 
    p-6 rounded-xl 
    shadow-sm dark:shadow-lg 
    border border-gray-100 dark:border-gray-800
    hover:-translate-y-2 hover:shadow-xl
    transition-all duration-300
    text-center space-y-4">

      <div className="w-12 h-12 mx-auto flex items-center justify-center 
      rounded-full 
      bg-purple-100 dark:bg-purple-500/10 
      text-purple-700 dark:text-purple-400">
        <FontAwesomeIcon className="text-lg" icon={faShieldHalved} />
      </div>

      <h3 className="font-semibold text-lg 
      text-gray-900 dark:text-gray-100">
        Pay Safe
      </h3>

      <p className="text-sm 
      text-gray-600 dark:text-gray-400">
        Holds the money you pay to freelancers in safe deposit until the work is completed.
      </p>
    </div>

    {/* Get Taxable Invoices */}
    <div className="bg-white dark:bg-gray-900 
    p-6 rounded-xl 
    shadow-sm dark:shadow-lg 
    border border-gray-100 dark:border-gray-800
    hover:-translate-y-2 hover:shadow-xl
    transition-all duration-300
    text-center space-y-4">

      <div className="w-12 h-12 mx-auto flex items-center justify-center 
      rounded-full 
      bg-purple-100 dark:bg-purple-500/10 
      text-purple-700 dark:text-purple-400">
        <FontAwesomeIcon className="text-lg" icon={faFileInvoiceDollar} />
      </div>

      <h3 className="font-semibold text-lg 
      text-gray-900 dark:text-gray-100">
        Get Taxable Invoices
      </h3>

      <p className="text-sm 
      text-gray-600 dark:text-gray-400">
        Receive taxable invoices through elharefa for the job done by the freelancer.
      </p>
    </div>

    {/* Guaranteed Satisfaction */}
    <div className="bg-white dark:bg-gray-900 
    p-6 rounded-xl 
    shadow-sm dark:shadow-lg 
    border border-gray-100 dark:border-gray-800
    hover:-translate-y-2 hover:shadow-xl
    transition-all duration-300
    text-center space-y-4">

      <div className="w-12 h-12 mx-auto flex items-center justify-center 
      rounded-full 
      bg-purple-100 dark:bg-purple-500/10 
      text-purple-700 dark:text-purple-400">
        <FontAwesomeIcon className="text-lg" icon={faCircleCheck} />
      </div>

      <h3 className="font-semibold text-lg 
      text-gray-900 dark:text-gray-100">
        Guaranteed Satisfaction
      </h3>

      <p className="text-sm 
      text-gray-600 dark:text-gray-400">
        We're incredibly proud of our freelancers with so many happy clients.
      </p>
    </div>

  </div>
</div>
   
{/* =====Talent Search: Find Quality Freelancers & Remote Employees ===== */}
<div >
  <ServiceCategories/>
</div>
{/* ===== Platform Features ===== */}
<div>
  <Feature/>
</div>
{/* ===== Top Freelancers: Our Top-Rated Freelancers ===== */}
<div>
  <Freelancer/>
  
</div>
{/* ===== Testimonials: Hear from Our Clients ===== */}
<div>
  <div>
     
    <ClientReviews/>
  </div>

</div>


  </div>
  
  
  
  
  
  
  
  
  
  
  
  <WhyChooesUs/>
  </div>
  
  </>
  )
}
