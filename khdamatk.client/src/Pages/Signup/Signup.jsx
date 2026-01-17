import React, { useState } from 'react'
import logoPhoto from '../../assets/Images/Logo.png'
import SocialButtons from '../../Components/SocialButtons/SocialButtons'
// import { faEyeSlash } from '@fortawesome/free-regular-svg-icons'
// import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useFormik } from 'formik'
import * as yup from 'yup'
import axios from 'axios'
import { toast } from 'react-toastify'
import { useNavigate } from 'react-router-dom'

export default function Signup() {
     const navigate=useNavigate();
     const [isExistError,setIsExistError]=useState(null)
    const regexpassword=/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$/
    const regexEgyptPhone = /^01[0125][0-9]{8}$/

    const  validationSchema= yup.object({
       userName: yup.string().required("Name is required").min(3, "Name must be at least 3 characters").max(15, "Name must be at most 15 characters"),
       lastName: yup.string().required("Last name is required").min(3, "Last name must be at least 3 characters").max(15, "Last name must be at most 15 characters"),
       email: yup.string().email("Invalid email address").required("Email is required"),
       password: yup.string().min(8, "Password must be at least 8 characters").required("Password is required").matches(regexpassword,"Password must have eight characters, at least one upper case English letter, one lower case English letter, one number and one special character"),
       rePassword: yup.string().oneOf([yup.ref("password")], "Passwords must match").required("Please confirm your passeord"),
       phone: yup.string().matches(regexEgyptPhone, "Phone number must be a valid Egyption number").required("Phone number is required"),
       terms:yup.boolean().oneOf([true],"You must accept the terms and conditions")
       
    })
     async function handleSignUp(values){
        
        try {
            const optain={
                method:"POST",
                url:'https://localhost:7210/Auth/Register',
                data:{
                    userName:values.userName,
                    email:values.email,
                    password:values.password
                }
            }
            const {data}= await axios.request(optain)
            console.log(data)
            if(data.isSuccess){
                toast("Your account has been created✅")
                setTimeout(()=>{
                    navigate('/login')
                },3000)
                
                
            }
        } catch (error) {
            console.log(error)
            setIsExistError(error.response.data.message)
        }   
    }
    const formik=useFormik({
        initialValues:{
            userName: '',
            lastName:'',
            email: '',
            password: '',
            rePassword:'',
            phone:'',
            terms: false

        },
        validationSchema,
        onSubmit:handleSignUp
    })
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
                <form className='mt-5 space-y-6' onSubmit={formik.handleSubmit}>
                       {/* Name field */}
                       <div className=' flex gap-2'>
                        <div className="First-Name  relative w-1/2">
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>First Name</span>
                            <input type='text' className='form-control' name='userName' value={formik.values.userName} onChange={formik.handleChange} onBlur={formik.handleBlur}/>
                            {formik.touched.userName && formik.errors.userName &&(<p className='text-red-500 text-sm'>{formik.errors.userName}</p>)}
                        </div>
                        <div className="Last-Name relative w-1/2">
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>Last Name</span>
                            <input type='text' className='form-control' name='lastName' value={formik.values.lastName} onChange={formik.handleChange} onBlur={formik.handleBlur}/>
                            {formik.touched.lastName && formik.errors.lastName &&(<p className='text-red-500 text-sm'>{formik.errors.lastName}</p>)}
                        </div>
                       </div>
                         {/* Email field&& Phone */}
                         <div className=' flex gap-2'>
                             <div className='relative w-1/2'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Email</span>
                            <input type='email' className='form-control' name='email' value={formik.values.email} onChange={formik.handleChange} onBlur={formik.handleBlur}/>
                            {formik.touched.email && formik.errors.email &&(<p className='text-red-500 text-sm'>{formik.errors.email}</p>)}
                            {isExistError && (<p className='text-red-500'>{isExistError}</p>)}
                        
                           </div>
                            <div className='relative w-1/2'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Phone</span>
                            <input type='tel' className='form-control' name='phone' value={formik.values.phone} onChange={formik.handleChange} onBlur={formik.handleBlur}/>
                            {formik.touched.phone && formik.errors.phone &&(<p className='text-red-500 text-sm'>{formik.errors.phone}</p>)}
                        
                           </div>
                         </div>

                           {/* Password field */}
                           
                            <div className='relative '>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>Password</span>
                            <input type='password' className='form-control pr-10' name='password' value={formik.values.password} onChange={formik.handleChange} onBlur={formik.handleBlur}/>
                            {/* <span className='absolute right-4 top -translate-y-1/2'>
                                 <FontAwesomeIcon icon={faEyeSlash} />
                            </span> */}
                            {formik.touched.password && formik.errors.password ?(<p className='text-red-500 text-sm'>{formik.errors.password}</p>):
                            (<p>Must be at least 8 characters with number and symbols</p>)}

                        
                           </div>
   

                             {/* Confirm-Password field */}
                             <div className='space-y-2'>
                                 <div className='relative'>
                                  <span className='absolute left-4 -top-3 bg-white px-2 text-sm  text-gray-500'>Confirm-Password</span>
                            <input type='password' className='form-control' name='rePassword' value={formik.values.rePassword} onChange={formik.handleChange} onBlur={formik.handleBlur}/>
                              {/* <span className='absolute right-4 top-1/2 transform -translate-y-1/2'>
                                 <FontAwesomeIcon icon={faEyeSlash} />
                            </span> */}
                               {formik.touched.rePassword && formik.errors.rePassword &&(<p className='text-red-500 text-sm'>{formik.errors.rePassword}</p>)}
                             </div>
                              {/* terms and conditions */}
                             <div>
                                <div className='flex gap-2'>
                                     <input type='checkbox' name='terms' checked={formik.values.terms} onChange={formik.handleChange} onBlur={formik.handleBlur}/>
                                <span>I agree to all the Terms and Privacy Policies</span>
                                </div>
                               {formik.touched.terms && formik.errors.terms &&(<p className='text-red-500 text-sm'>{formik.errors.terms}</p>)}

                             </div>
                             </div>
                             
                                {/* Submit button */}
                                <div>
                                    <button className='btn text-white' type='submit'>Create Account</button>
                                </div>
                </form>
                {/* Already have an account? Login */}
                <p className='mt-4 text-center text-lg'>Already have an account? Login</p>
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
