 import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import homePhoto from '../../assets/Images/Home.png'
import { faFileInvoiceDollar, faPhoneVolume, faShieldHalved } from '@fortawesome/free-solid-svg-icons'
import { faCircleCheck } from '@fortawesome/free-regular-svg-icons'
import TestimonialCard from '../../Components/TestimonialCard/TestimonialCard'
 

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
        <button className='btn w-fit bg-primary-button text-white'>post jop </button>
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
    <div className='text-center'>
      <FontAwesomeIcon className='text-lg' icon={faPhoneVolume} />
      <h3 className=' text-black font-bold'>Dedicated Support</h3>
      <h4 className='text-sm'>Managers are always there to help you out with any kind of challanges</h4>
    </div>
      {/* Pay Safe */}
    <div className='text-center'>
      <FontAwesomeIcon icon={faShieldHalved} className='text-lg'/>
      <h3  className=' text-black font-bold'>Pay Safe</h3>
      <h4  className='text-sm '>holds the money you pay to the Freelancers in Safe Deposit until the work is completed </h4>
    </div>
       {/* Get Taxable Invoices */}
    <div className='text-center'>
      <FontAwesomeIcon icon={faFileInvoiceDollar} className='text-lg'/>
      <h3  className=' text-black font-bold'>Get Taxable Invoices</h3>
      <h4  className='text-sm'>Receive taxable invoices through elharefa for the job done by the freelancer.</h4>
    </div>
       {/*Guaranteed Satisfaction */}
    <div className='text-center'>
     <FontAwesomeIcon icon={faCircleCheck} className='text-lg'/>
      <h3  className=' text-black font-bold'>Guaranteed Satisfaction</h3>
      <h4  className='text-sm'>We're incredibly proud of our freelancers. With so many happy </h4>
    </div>
    </div>
   </div>
   
{/* ===== Talent Search: Find Quality Freelancers & Remote Employees ===== */}
<div></div>
{/* ===== Platform Features ===== */}
<div></div>
{/* ===== Top Freelancers: Our Top-Rated Freelancers ===== */}
<div></div>
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
  
  
  
  
  
  
  
  
  
  
  
  
  
  </>
  )
}
