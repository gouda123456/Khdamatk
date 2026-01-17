import logoPhoto from '../../assets/Images/Logo.png'
import SocialButtons from '../../Components/SocialButtons/SocialButtons'
import { faEyeSlash } from '@fortawesome/free-regular-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export default function Login() {
  return (
<>
<div>
    <div className="container items-center justify-between gap-16 grid lg:grid-cols-2">
        {/* Left side: Login form and text content */}
        <div className=' p-8 bg-white shadow-lg'>
            {/* Title & description */}
             <div className="Title space-y-2">
                <h1 className=' font-bold text-2xl'>Login</h1>
            <p className='text-gray-400/70'>Login to access your travelwise  account</p>
             </div>
             {/* Login form  */}
             <div>
                <form className='mt-5 space-y-6'>
                         {/* Email field&& Phone */}
                         <div className=' flex gap-2'>
                             <div className='relative w-full'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Email</span>
                            <input type='email' className='form-control'/>
                        
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
                             {/* terms and conditions */}
                             <div className='flex gap-2 -mt-2'>
                                <input type='checkbox'/>
                                <span>Remember me</span>
                             </div>
                            
                             
                                {/* Submit button */}
                                <div>
                                    <button className='btn text-white'>Login</button>
                                </div>
                </form>
                {/* Don’t have an account? Sign in */}
                <p className='mt-4 text-center text-sm'>Don’t have an account? Sign in</p>
                {/* ------------------------OR------------------------------ */}
                <div className='text-center relative m-9'>
                    <div className='w-full h-0.5 border border-gray-300'></div>
                    <p className='text-gray-400 text-sm absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-white px-2'>Or login with</p>
                </div>
                {/* Social media login buttons */}
                <div>
                    <SocialButtons/>
                </div>
             </div>
        
        </div>
        {/* Right side: Illustration image for the login page */}
        <div>
            <img src={logoPhoto} alt="Sign in" />
        </div>
    </div>
</div>










</>
  )
}
