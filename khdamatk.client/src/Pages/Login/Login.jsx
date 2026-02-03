import { Link } from 'react-router-dom'
import logoPhoto from '../../assets/Images/Logo.png'
import SocialButtons from '../../Components/SocialButtons/SocialButtons'
import { faEye, faEyeSlash } from '@fortawesome/free-regular-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useFormik } from 'formik'
import * as yup from 'yup'
import axios from 'axios'
import { toast } from 'react-toastify'
import { useNavigate } from 'react-router-dom'
import { useState } from 'react'
import { API_CONFIG} from '../../Config'
import { sendDataToLogin } from '../../Services/auth-services'

export default function Login() {

     const navigate=useNavigate();
     const [isExistError,setIsExistError]=useState(null)
     const [isExistErrorEmail, setIsExistErrorEmail]=useState(null)
     const [isShownPassword,setIsShownPassword]=useState(false)
    const regexpassword=/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$/
     

    const  validationSchema= yup.object({
 
       email: yup.string().email("Invalid email address").required("Email is required"),
       password: yup.string().min(8, "Password must be at least 8 characters").required("Password is required").matches(regexpassword,"Password must have eight characters, at least one upper case English letter, one lower case English letter, one number and one special character"),
       
    })
     async function handleLogin(values){
        
        try {
             
             
            const response= await sendDataToLogin(values)
            if(response.isSuccess){
                toast("ًWelcome Back")
                setTimeout(()=>{
                    navigate('/')
                },3000)
                
                
            }
        }
         catch (error) {

              console.log(error);
              if(error.response.data.errors[0].title === "InvalidPassword"){
                 setIsExistError(error.response.data.errors[0].message)
                 setIsExistErrorEmail(null)
                  
                 
              }
               
          else if(error.response.data.errors[0].title === "UserNotFound"){
            setIsExistErrorEmail(error.response.data.errors[0].message)
            setIsExistError(null)
          }
           else {
    setIsExistError("Server not responding, try again later");
    setIsExistErrorEmail(null);
  }
        }   
 

    }
 


    const formik=useFormik({
        initialValues:{
            email: '',
            password: '',
            rememberMe: false

        },
        validationSchema,
        onSubmit:handleLogin
    })
    function togglePasswordVisibility(){
        setIsShownPassword(!isShownPassword)
    }
     function handleChange(e){
        setIsExistError("")
        setIsExistErrorEmail("")
        formik.handleChange(e)
    }
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
                <form className='mt-5 space-y-5' onSubmit={formik.handleSubmit}>
                         {/* Email field&& Phone */}
                         <div className=' flex gap-2'>
                             <div className='relative w-full'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Email</span>
                            <input type='email' className='form-control' name='email' value={formik.values.email} onChange={handleChange} onBlur={formik.handleBlur}/>
                            {formik.touched.email && formik.errors.email &&(<p className='text-red-500 text-sm mt-1'>{formik.errors.email}</p>)}
                            {isExistErrorEmail && (<p className='text-red-500'>{isExistErrorEmail}</p>)}

                        
                           </div>
                            
                         </div>
                           {/* Password field */} 
                           <div className='relative'>
                        <span className='bg-white px-2  absolute left-4 -top-3 text-sm text-gray-500'>Password</span>
                            <input type={isShownPassword ? "text" : "password"} className='form-control' name='password' value={formik.values.password} onChange={handleChange} onBlur={formik.handleBlur}/>
                            <button   type="button"  onClick={togglePasswordVisibility} className='absolute right-3 top-1/2 -translate-y-1/2 text-gray-800'>
                                   {isShownPassword ? ( <FontAwesomeIcon icon={faEyeSlash} />) : (<FontAwesomeIcon icon={faEye} />)}
                            </button>
                           </div>
                            {formik.touched.password && formik.errors.password &&(<p className='text-red-500 text-sm -mt-3'>{formik.errors.password}</p>)}
                            {isExistError && (<p className='text-red-500 -mt-3 text-sm'>{isExistError}</p>)}
                             {/* terms and conditions */}
                             <div className='flex justify-between items-center -mt-3'>
                                 <div className='flex  gap-1 '>
                                    <input type='checkbox'/>
                                <span>Remember me</span>
                                 </div>
                                <Link to="/forget-password" className='text-red-500'>ForgetPassword?</Link>
                             </div>
                                {/* Submit button */}
                                <div>
                                    <button className='btn text-white' type='submit'>Login</button>
                                </div>
                </form>
                {/* Don’t have an account? Sign in */}
                <p className='mt-4 text-center text-sm'>Don’t have an account?  <Link to="/signup" className='text-red-500 font-semibold'>Sign in</Link></p>
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
