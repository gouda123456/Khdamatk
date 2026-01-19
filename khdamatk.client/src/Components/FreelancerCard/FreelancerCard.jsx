import { Link } from 'react-router-dom'
import photo from '../../assets/Images/Logo.png'

export default function FreelancerCard() {
  return (
     <>
     <div className=" flex items-center justify-center mb-4">
     <div className="card space-y-4 w-72 text-center border border-black/10  rounded-2xl py-6 shadow-sm hover:shadow-md transition-shadow duration-300 ">
        <div>
              <img src={photo} className='size-14 object-cover rounded-full mx-auto mb-3'/>
         <h2 className='font-semibold text-xl'>Name</h2>
         <h3 className='text-sm text-gray-600'>Track Name</h3>
        </div>
         <div>
            <h3 className='w-full h-0.5 border border-gray-300/30'></h3>
            <div className='flex items-center justify-between mx-4'>
            <p>HourlyRate</p>
            <span>350/EG</span>
        </div>
            <h3 className='w-full h-0.5 border border-gray-300/30'></h3>

         </div>
        <div className='flex items-center justify-around '>
            <h3 className='bg-gray-500 text-white px-3 py-1 rounded-2xl'>UI</h3>
            <h3 className='bg-gray-500 text-white px-3 py-1 rounded-2xl'>UI</h3>
            <h3 className='bg-gray-500 text-white px-3 py-1 rounded-2xl'>UI</h3>
        </div>
        <div>
            <Link to="profile"> 
            <button className='btn text-white'>Profile</button>
            </Link>
        </div>
     </div>
     </div>
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     
     </>
  )
}
