import { faFacebook, faInstagram, faSquareLinkedin, faSquareTwitter } from '@fortawesome/free-brands-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React from 'react'
import { Link } from 'react-router-dom'

export default function Footer() {
  return (
     <>
      <div className='bg-Purple-800'>
     <div className="container p-8">
      <div className='grid grid-cols-1 gap-8 text-center sm:grid-cols-2 sm:text-left lg:grid-cols-4'>
         {/* logo */}
      <div>
        <h2 className='text-white font-bold text-2xl'>KHADMA <span className='text-Purple-400'> HUB</span></h2>
        <p className='text-white '>Helping you find the right<br/> service, faster.</p>
        
      </div>
      {/* Quick Links */}
      <div className='text-white'>
        <p className='text-2xl'>Quick Links</p>
         <div className='flex flex-col gap-2'>
          <Link>Home</Link>
        <Link>Services</Link>
        <Link>Profile</Link>
        <Link>Sign Up</Link>
         </div>
      </div>
      {/* Support */}
      <div className='text-white'>
        <p className='text-2xl'>Support</p>
        <div className='flex flex-col gap-2'> 
        <Link>Privacy Policy</Link>
        <Link>FAQ</Link>
        <Link>Terms & Conditions</Link>
        </div>
      </div>
      {/* Socail-Media */}
      <div className='text-white text-2xl flex items-center gap-2'>
        <FontAwesomeIcon icon={faFacebook} />
        <FontAwesomeIcon icon={faSquareLinkedin} />
        <FontAwesomeIcon icon={faInstagram} />
        <FontAwesomeIcon icon={faSquareTwitter} />
        
      </div>
      </div>
       

     </div>
     <p className='text-white text-center mt-10 bg-Purple-600 p-4 w-full'>© 2026 KHADMA HUB. All rights reserved.</p>
       
     
     </div>
       
     
     
     
     
     
     
     </>
  )
}
