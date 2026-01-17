import { faApple, faFacebook } from '@fortawesome/free-brands-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import GooglePhoto from '../../assets/Images/Google.png'
import React from 'react'

export default function SocialButtons() {
  return (
 <>
 <div className='Social-buttons flex gap-3 justify-center items-center'>
   <button className='Facebook-butto border border-primary-button px-10 py-2 flex items-center justify-center '>
     <FontAwesomeIcon icon={faFacebook} className='text-blue-700 text-2xl' />
   </button>
   <button className='Google-button  border border-primary-button px-10 py-2 flex items-center justify-center  '>
     {/* <FontAwesomeIcon icon={faGoogle} /> */}
     <img src={GooglePhoto} alt="GooglePhoto"  className="w-6 h-6"  />
   </button>
   <button className='Apple-button border border-primary-button px-10 py-2 flex items-center justify-center  '>
     <FontAwesomeIcon icon={faApple} className='text-2xl' />
   </button>
 </div>
 
 
 
 
 
 </>
  )
}
