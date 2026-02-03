import { faArrowLeft } from '@fortawesome/free-solid-svg-icons'
import logoPhoto from '../../assets/Images/Logo.png'
import SocialButtons from '../../Components/SocialButtons/SocialButtons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export default function ForgetPassword() {
  return (
    <>
    <div>
    <div className="container items-center justify-between gap-16 grid lg:grid-cols-2">
          {/* ===== Left side: Forgot Password Form ===== */}
        <div className=' p-10 bg-white shadow-lg'>
           {/* Back button to return to Login page */}
            <div className='mb-4 '>
                <button className='flex items-center gap-2'>
                    <FontAwesomeIcon icon={faArrowLeft} />
                    <span>Back to login</span>
                </button>
            </div>
             {/* --- Title & Description --- */}
             <div className="Title space-y-2">
                <h1 className=' font-bold text-2xl'>Forgot your password?</h1>
            <p className='text-gray-400/70'>Don’t worry, happens to all of us. Enter your email below to recover your password</p>
             </div>
                  {/* --- Forgot Password Form --- */}
             <div>
                <form className='mt-5 space-y-6'>
                         {/* Email field&& Phone */}
                         <div className=' flex gap-2'>
                             <div className='relative w-full'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Email</span>
                            <input type='email' className='form-control'/>
                        
                           </div>
                            
                         </div>
                         {/* Submit button */}
                                <div>
                                    <button className='btn text-white'>Submit</button>
                                </div>
                </form>
                
                     {/* --- Divider with "Or login with" --- */}
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
             {/* ===== Right side: Illustration Image ===== */}
        <div>
            <img src={logoPhoto} alt="Sign in" />
        </div>
    </div>
</div>
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    </>
  )
}
