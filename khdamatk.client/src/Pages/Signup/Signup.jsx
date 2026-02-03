import React, { useState } from 'react'
import logoPhoto from '../../assets/Images/Logo.png'
import SocialButtons from '../../Components/SocialButtons/SocialButtons'
import { faEye, faEyeSlash } from '@fortawesome/free-regular-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useFormik } from 'formik'
import * as yup from 'yup'
import { toast } from 'react-toastify'
import { useNavigate } from 'react-router-dom'
import WhyChooesUs from '../../Components/WhyChooesUs/WhyChooesUs'
import { sendDataToSignup } from '../../Services/auth-services'
import { checkPasswordStrenght } from '../../Utils/Validation'

export default function Signup() {
    const navigate = useNavigate();
    const [usernameError, setUsernameError] = useState(null);
    const [isExistErrorEmail, setIsExistErrorEmail] = useState(null)
    const [isShownPassword, setIsShownPassword] = useState(false)
    const [isShownConPassword, setIsShownConPassword] = useState(false)

    const regexpassword = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$/
    const regexEgyptPhone = /^01[0125][0-9]{8}$/

    const validationSchema = yup.object({
        userName: yup.string().required("Name is required").min(3, "Name must be at least 3 characters").max(15, "Name must be at most 15 characters"),
        lastName: yup.string().required("Last name is required").min(3, "Last name must be at least 3 characters").max(15, "Last name must be at most 15 characters"),
        email: yup.string().email("Invalid email address").required("Email is required"),
        password: yup.string().min(8, "Password must be at least 8 characters").required("Password is required").matches(regexpassword, "Password must have eight characters, at least one upper case English letter, one lower case English letter, one number and one special character"),
        rePassword: yup.string().oneOf([yup.ref("password")], "Passwords must match").required("Please confirm your password"),
        phone: yup.string().matches(regexEgyptPhone, "Phone number must be a valid Egyptian number").required("Phone number is required"),
        terms: yup.boolean().oneOf([true], "You must accept the terms and conditions")
    })

    const formik = useFormik({
        initialValues: {
            userName: '',
            lastName: '',
            email: '',
            password: '',
            rePassword: '',
            phone: '',
            terms: false
        },
        validationSchema,
        onSubmit: handleSignUp
    })

   async function handleSignUp(values){
          
       try {
              
              const response= await sendDataToSignup(values)
              if(response.isSuccess){
                  toast("Your account has been created✅")
                  setTimeout(()=>{
                      navigate('/confirm-email')
                  },3000)
                  
                  
              }
          } catch (error) {
              if(error.response.data.errors[0].title === "DuplicateUserName"){
                  setUsernameError(error.response.data.errors[0].message)
  
              }
              console.log(error)
            setIsExistErrorEmail(error.response.data.message)
          }   }
    const togglePasswordVisibility = () => setIsShownPassword(!isShownPassword);
    const toggleConPasswordVisibility = () => setIsShownConPassword(!isShownConPassword);

    const handleChange = (e) => {
        setIsExistErrorEmail("")
        setUsernameError("")
        formik.handleChange(e)
    }

    const passwordFeedBack = checkPasswordStrenght(formik.values.password)

    return (
        <>
            <div className="container items-center justify-between gap-16 grid lg:grid-cols-2">
                <div className='p-8 bg-white shadow-lg'>
                    <div className="Title space-y-2">
                        <h1 className='font-bold text-2xl'>Sign up</h1>
                        <p className='text-gray-400/70'>Let’s get you all set up so you can access your personal account.</p>
                    </div>

                    <form className='mt-5 space-y-6' onSubmit={formik.handleSubmit}>
                        {/* Name fields */}
                        <div className='flex gap-2'>
                            <div className="First-Name relative w-1/2">
                                <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>First Name</span>
                                <input type='text' className='form-control' name='userName' value={formik.values.userName} onChange={handleChange} onBlur={formik.handleBlur} />
                                {formik.touched.userName && formik.errors.userName && <p className='text-red-500 text-sm'>{formik.errors.userName}</p>}
                                {usernameError && <p className='text-red-500 text-sm'>{usernameError}</p>}
                            </div>

                            <div className="Last-Name relative w-1/2">
                                <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Last Name</span>
                                <input type='text' className='form-control' name='lastName' value={formik.values.lastName} onChange={handleChange} onBlur={formik.handleBlur} />
                                {formik.touched.lastName && formik.errors.lastName && <p className='text-red-500 text-sm'>{formik.errors.lastName}</p>}
                            </div>
                        </div>

                       {/* Email field&& Phone */}
                         <div className=' flex gap-2'>
                             <div className='relative w-1/2'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Email</span>
                            <input type='email' className='form-control' name='email' value={formik.values.email} onChange={handleChange} onBlur={formik.handleBlur}/>
                            {formik.touched.email && formik.errors.email &&(<p className='text-red-500 text-sm'>{formik.errors.email}</p>)}
                            {isExistErrorEmail && (<p className='text-red-500 text-sm'>{isExistErrorEmai}</p>)}
                        
                           </div>
                            <div className='relative w-1/2'>
                            <span className='absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500'>Phone</span>
                            <input type='tel' className='form-control' name='phone' value={formik.values.phone} onChange={formik.handleChange} onBlur={formik.handleBlur}/>
                            {formik.touched.phone && formik.errors.phone &&(<p className='text-red-500 text-sm'>{formik.errors.phone}</p>)}
                        
                           </div>
                         </div>

                        {/* Password */}
                        <div className='relative'>
                            <span className='bg-white px-2 absolute left-4 -top-3 text-sm text-gray-500'>Password</span>
                            <input type={isShownPassword ? "text" : "password"} className='form-control' name='password' value={formik.values.password} onChange={formik.handleChange} onBlur={formik.handleBlur} />
                            <button type="button" onClick={togglePasswordVisibility} className='absolute right-3 top-1/2 -translate-y-1/2 text-gray-800'>
                                {isShownPassword ? <FontAwesomeIcon icon={faEyeSlash} /> : <FontAwesomeIcon icon={faEye} />}
                            </button>
                        </div>
                        {formik.values.password && <div className='password-strenght flex gap-2 items-center -mt-6'>
                            <div className='bar rounded-xl overflow-hidden w-full h-1 bg-gray-200'>
                                <div className={`progress ${passwordFeedBack.width} ${passwordFeedBack.background} h-full`}></div>
                            </div>
                            <span className='text-nowrap text-center w-28'>{passwordFeedBack.text}</span>
                        </div>}
                        {formik.touched.password && formik.errors.password ? (<p className='text-red-500 text-sm -mt-6'>{formik.errors.password}</p>) : (<p className='-mt-6'>Must be at least 8 characters with number and symbols</p>)}

                        {/* Confirm Password */}
                        <div className='relative mt-4'>
                            <span className='bg-white px-2 absolute left-4 -top-3 text-sm text-gray-500'>Confirm Password</span>
                            <input type={isShownConPassword ? "text" : "password"} className='form-control' name='rePassword' value={formik.values.rePassword} onChange={formik.handleChange} onBlur={formik.handleBlur} />
                            <button type="button" onClick={toggleConPasswordVisibility} className='absolute right-3 top-1/2 -translate-y-1/2 text-gray-800'>
                                {isShownConPassword ? <FontAwesomeIcon icon={faEyeSlash} /> : <FontAwesomeIcon icon={faEye} />}
                            </button>
                        </div>
                        {formik.touched.rePassword && formik.errors.rePassword && <p className='text-red-500 text-sm'>{formik.errors.rePassword}</p>}

                        {/* Terms */}
                        <div className='flex gap-2 mt-2'>
                            <input type='checkbox' name='terms' checked={formik.values.terms} onChange={formik.handleChange} onBlur={formik.handleBlur} />
                            <span>I agree to all the Terms and Privacy Policies</span>
                        </div>
                        {formik.touched.terms && formik.errors.terms && <p className='text-red-500 text-sm'>{formik.errors.terms}</p>}

                        {/* Submit */}
                        <button className='btn text-white mt-4' type='submit'>Create Account</button>
                    </form>

                    <p className='mt-4 text-center text-lg'>Already have an account? Login</p>

                    {/* OR */}
                    <div className='text-center relative m-9'>
                        <div className='w-full h-0.5 border border-gray-300'></div>
                        <p className='text-gray-400 text-sm absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-white px-2'>OR Signup With</p>
                    </div>

                    <SocialButtons />
                </div>

                <div>
                    <img src={logoPhoto} alt="Logo" />
                </div>
            </div>

            <WhyChooesUs />
        </>
    )
}
