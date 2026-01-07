import React from 'react'
import logoPhoto from '../../assets/Images/Logo.png'
import SocialButtons from '../../Components/SocialButtons/SocialButtons'
import { faEyeSlash } from '@fortawesome/free-regular-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export default function Signup() {
  return (
<>
<div>
    <div className="container items-center justify-between gap-16 grid lg:grid-cols-2">
        {/* Left side: Sign up form and text content */}
        <div className=' p-8 bg-white shadow-lg'>
                      {/* Title & description */}
             <div className="Title space-y-2">
                <h1 className=' font-bold text-2xl'>Sign up</h1>
            <p className='text-gray-400/70'>Let’s get you all st up so you can access your personal account.</p>
             </div>
             {/* Sign up form  */}
             <div>
                <form className='mt-5 space-y-6'>
                       {/* Name field */}
                       <div className=' flex gap-2'>
                        <div className="First-Name  relative w-1/2">
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>First Name</span>
                            <input type='text' className='form-control'/>
                        </div>
                        <div className="Last-Name relative w-1/2">
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>Last Name</span>
                            <input type='text' className='form-control'/>
                        </div>
                       </div>
                         {/* Email field&& Phone */}
                         <div className=' flex gap-2'>
                             <div className='relative w-1/2'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Email</span>
                            <input type='email' className='form-control'/>
                        
                           </div>
                            <div className='relative w-1/2'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Phone</span>
                            <input type='tel' className='form-control'/>
                        
                           </div>
                         </div>

                           {/* Password field */}
                           
                            <div className='relative'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>Password</span>
                            <input type='password' className='form-control'/>
                            <span className='absolute right-4 top-1/2 transform -translate-y-1/2'>
                                 <FontAwesomeIcon icon={faEyeSlash} />
                            </span>
                        
                           </div>
                             {/* Confirm-Password field */}
                             <div className='space-y-2'>
                                 <div className='relative'>
                                  <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>Confirm-Password</span>
                            <input type='password' className='form-control'/>
                              <span className='absolute right-4 top-1/2 transform -translate-y-1/2'>
                                 <FontAwesomeIcon icon={faEyeSlash} />
                            </span>
                             </div>
                              {/* terms and conditions */}
                             <div className='flex gap-2'>
                                <input type='checkbox'/>
                                <span>I agree to all the Terms and Privacy Policies</span>
                             </div>
                             </div>
                             
                                {/* Submit button */}
                                <div>
                                    <button className='btn text-white'>Create Account</button>
                                </div>
                </form>
                {/* Already have an account? Login */}
                <p className='mt-4 text-center text-sm'>Already have an account? Login</p>
                {/* ------------------------OR------------------------------ */}
                <div className='text-center relative m-9'>
                    <div className='w-full h-0.5 border border-gray-300'></div>
                    <p className='text-gray-400 text-sm absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-white px-2'>OR Signup With</p>
                </div>
                {/* Social media login buttons */}
                <div>
                    <SocialButtons/>
                </div>
             </div>
        
        </div>
        {/* Right side: Illustration image for the sign up page */}
        <div>
            <img src={logoPhoto} alt="Logo" />
        </div>
    </div>
</div>










</>
  )
}
