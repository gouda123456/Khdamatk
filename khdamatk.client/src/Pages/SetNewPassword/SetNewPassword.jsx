import { faArrowLeft, faEyeSlash } from '@fortawesome/free-solid-svg-icons'
import logoPhoto from '../../assets/Images/Logo.png'
import SocialButtons from '../../Components/SocialButtons/SocialButtons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export default function SetNewPassword() {
  return (
   <>
          <div>
    <div className="container items-center justify-between gap-16 grid lg:grid-cols-2">
      
    {/* ===== Left side: Set New Password Section ===== */}
        <div className=' p-10 bg-white shadow-lg'>
            {/* Back button to return to Login page */}
            <div className='mb-4 '>
                <button className='flex items-center gap-2'>
                    <FontAwesomeIcon icon={faArrowLeft} />
                    <span>Back to login</span>
                </button>
            </div>
                {/* --- Page Title & Instructions --- */}
             <div className="Title space-y-2">
                <h1 className=' font-bold text-2xl'>Set a password</h1>
            <p className='text-gray-400/70'>Your previous password has been reseted. Please set a new password for your account.</p>
             </div>
               {/* --- New Password Form --- */}
             <div>
                <form className='mt-5 space-y-6'>
                         {/* New password input field */}
                          <div className='relative'>
                 <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>Create Password</span>
                <input type='password' className='form-control'/>
                 <span className='absolute right-4 top-1/2 transform -translate-y-1/2'>
                 <FontAwesomeIcon icon={faEyeSlash} />
                 </span>
                                                 
                         </div>
                         {/* Confirm new password input field */}
                          <div className='relative'>
                 <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>Re-enter Password </span>
                <input type='password' className='form-control'/>
                 <span className='absolute right-4 top-1/2 transform -translate-y-1/2'>
                 <FontAwesomeIcon icon={faEyeSlash} />
                 </span>
                                                 
                         </div>
                             {/* Submit new password button */}
                                <div>
                                    <button className='btn text-white'>Set password</button>
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
