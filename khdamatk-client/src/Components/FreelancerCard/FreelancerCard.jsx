import { Link } from 'react-router-dom'
import photo from '../../assets/Images/Logo.png'

export default function FreelancerCard({ data }) {
      const {
    profilePictureUrl,
    userName,
    jobTitle,
    hourlyRate,
    skills
  } = data

  return (
     <>
     <div className=" flex items-center justify-center dark:text-white">
     <div className="card space-y-4 w-72 text-center   rounded-2xl py-6   hover:-translate-y-2 
hover:shadow-xl 
transition-all duration-300
bg-white dark:bg-gray-900
border border-gray-100 dark:border-gray-800
shadow-sm dark:shadow-lg

">
        <div>
              <img src={photo} className='size-14 object-cover rounded-full mx-auto mb-3'/>
         <h2 className='font-semibold text-xl'>{userName}</h2>
         <h3 className='text-sm text-gray-600 dark:text-white'>{jobTitle}</h3>
        </div>
        
         <div className="mt-4">
  <div className="w-full h-px bg-gray-300/40 mb-2"></div>

  <div className="flex items-center justify-between px-2">
    <p className="text-sm text-gray-500 dark:text-white">Hourly Rate</p>
    <span className="font-semibold text-gray-800 dark:text-white">
      {hourlyRate} EGP / hr
    </span>
  </div>
  <div className="w-full h-px bg-gray-300/40 mt-2"></div>
</div>

        
         <div className="flex flex-wrap gap-2 justify-center mt-3">
        {skills.map((skill, index) => (
          <span
            key={index}
            className="text-xs bg-gray-100 dark:bg-gray-900/5 px-2 py-1 rounded-full"
          >
            {skill}
          </span>
        ))}
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
