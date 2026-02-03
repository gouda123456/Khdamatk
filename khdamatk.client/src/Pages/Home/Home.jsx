 import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import homePhoto from '../../assets/Images/Home.png'
import { faFileInvoiceDollar, faPhoneVolume, faShieldHalved } from '@fortawesome/free-solid-svg-icons'
import { faCircleCheck } from '@fortawesome/free-regular-svg-icons'
import TestimonialCard from '../../Components/TestimonialCard/TestimonialCard'
import WhyChooesUs from '../../Components/WhyChooesUs/WhyChooesUs'
import FreelancerCard from '../../Components/FreelancerCard/FreelancerCard'
 

export default function Home() {
  return (
  <>
  <div className="container space-y-10 ">
   {/* ===== Hero Section: Hire The Best Talents to Grow Your Business ===== */}
   <div className='grid lg:grid-cols-2 items-center'>
      {/* Left-Side */}
    <div className=' p-8 space-y-3'>
      {/* Main Title */}
      <h1 className='font-bold text-3xl'>
        Hire The Best Talents to Grow<br/> Your Business
      </h1>
      {/* Main Title */}
      <p className='text-lg'>
        khadma hub is a freelance & remote work<br/> marketplace with thousands of top-rated<br/> freelancers & remote employees.<br/> It is simple and quick to Post your job for<br/> free and get quick proposals for your jobs Top<br/> companies and start-ups in Egypt hire<br/> elharefa freelancers
      </p>
      {/* Contact buttons */}
      <div className='flex items-center gap-3' >
        <button className='btn w-fit bg-Purple-800 text-white'>post jop </button>
        <button className='btn w-fit bg-amber-500 text-white'>Get A jop</button>

      </div>
    </div>
    {/* Right-Side */}
    <div>
      <img src={homePhoto}/>

    </div>
   </div>
   {/* ===== Why Choose Us: Why Hire Freelancers Through Khadma Hub ===== */}
   <div className='space-y-6 p-4'>
    <h2 className='font-bold text-3xl text-center'>Why Hire Freelancers Through khadma hub</h2>
    <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8'>
       {/* Dedicated Support */}
     <div className="bg-white p-6 rounded-xl shadow-sm text-center space-y-4">
            <div className="w-12 h-12 mx-auto flex items-center justify-center rounded-full bg-purple-100 text-purple-700">
             <FontAwesomeIcon className='text-lg' icon={faPhoneVolume} />
            </div>
            <h3 className="font-semibold text-lg">Dedicated Support</h3>
            <p className="text-sm text-gray-600">
             Managers are always there to help you out with any kind of challanges
            </p>
          </div>
      {/* Pay Safe */}
    <div className="bg-white p-6 rounded-xl shadow-sm text-center space-y-4">
            <div className="w-12 h-12 mx-auto flex items-center justify-center rounded-full bg-purple-100 text-purple-700">
             <FontAwesomeIcon icon={faShieldHalved} className='text-lg'/>
            </div>
            <h3 className="font-semibold text-lg">Pay Safe</h3>
            <p className="text-sm text-gray-600">
            holds the money you pay to the Freelancers in Safe Deposit until the work is completed
            </p>
          </div>
       {/* Get Taxable Invoices */}
    <div className="bg-white p-6 rounded-xl shadow-sm text-center space-y-4">
            <div className="w-12 h-12 mx-auto flex items-center justify-center rounded-full bg-purple-100 text-purple-700">
            <FontAwesomeIcon icon={faFileInvoiceDollar} className='text-lg'/>
            </div>
            <h3 className="font-semibold text-lg">Get Taxable Invoices</h3>
            <p className="text-sm text-gray-600">
           Receive taxable invoices through elharefa for the job done by the freelancer.
            </p>
          </div>
       {/*Guaranteed Satisfaction */}
       <div className="bg-white p-6 rounded-xl shadow-sm text-center space-y-4">
            <div className="w-12 h-12 mx-auto flex items-center justify-center rounded-full bg-purple-100 text-purple-700">
           <FontAwesomeIcon icon={faCircleCheck} className='text-lg'/>
            </div>
            <h3 className="font-semibold text-lg">Guaranteed Satisfaction</h3>
            <p className="text-sm text-gray-600">
          We're incredibly proud of our freelancers. With so many happy
            </p>
          </div>
    </div>
   </div>
   
{/* ===== Talent Search: Find Quality Freelancers & Remote Employees ===== */}
<div></div>
{/* ===== Platform Features ===== */}
<div></div>
{/* ===== Top Freelancers: Our Top-Rated Freelancers ===== */}
<div className='space-y-6 p-4'>
  <h2 className='font-bold text-3xl text-center'>Our Top-Rated Freelancers</h2>
  <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4'>
    <FreelancerCard/>
    <FreelancerCard/>
    <FreelancerCard/>
    <FreelancerCard/>
  </div>
</div>
{/* ===== Testimonials: Hear from Our Clients ===== */}
<div>
  <div  className='space-y-6 p-4'>
    <h2 className='font-bold text-3xl text-center'>Hear from Our Clients</h2>
    {/* details-card */}
    <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4'>
     
      <TestimonialCard/>
      <TestimonialCard/>
      <TestimonialCard/>
      <TestimonialCard/>
       
  
  

    </div>
  </div>

</div>


  </div>
  
  
  
  
  
  
  
  
  
  
  
  <WhyChooesUs/>
  
  </>
  )
}
