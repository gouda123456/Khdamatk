 import homePhoto from '../../assets/Images/Home.png'

export default function Home() {
  return (
  <>
  <div className="container ">
    {/* Top-Home */}
   <div className='grid lg:grid-cols-2 items-center '>
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
   {/* ============================================================$$$======================================================= */}
  </div>
  
  
  
  
  
  
  
  
  
  
  
  
  
  </>
  )
}
