// import { useFormik } from "formik"
// import { toast } from "react-toastify"
// import { sendDataToConfirmEmail } from "../../Services/auth-services"

// export default function SendConfirmEmail() {
//  const formik = useFormik({
//     initialValues: {
//       email: ""
//     },
//     onSubmit: async (values) => {
//       try {
//         const res = await sendDataToConfirmEmail(values)
//         toast.success("Confirmation email sent again ✅")
//       } catch (error) {
//         toast.error("Something went wrong ❌")
//       }
//     }
//   })

//   return (
//     <div className="container flex justify-center items-center min-h-screen">
//       <div className="bg-white shadow-lg p-8 w-full max-w-md space-y-6">
//         <h1 className="text-2xl font-bold text-center">
//           Confirm your email
//         </h1>

//         <p className="text-center text-gray-500">
//           We have sent a confirmation link to your email.
//         </p>

//         <form onSubmit={formik.handleSubmit} className="space-y-4">
//           <input
//             type="email"
//             name="email"
//             placeholder="Enter your email"
//             className="form-control"
//             value={formik.values.email}
//             onChange={formik.handleChange}
//           />

//           <button type="submit" className="btn w-full text-white">
//             Resend confirmation email
//           </button>
//         </form>
//       </div>
//     </div>
//   )
// }
import { useFormik } from "formik"
import { toast } from "react-toastify"
import { sendDataToConfirmEmail } from "../../Services/auth-services"
import { useNavigate } from "react-router-dom"

export default function SendConfirmEmail() {
  const navigate = useNavigate()

  const formik = useFormik({
    initialValues: {
      email: ""
    },
    onSubmit: async (values) => {
      try {
        await sendDataToConfirmEmail(values)

        toast.success("Confirmation email sent successfully ✅")
      } catch (error) {
        // الإيميل متأكد قبل كده
        if (error.response?.status === 409) {
          toast.info("This email is already confirmed ✅")

          setTimeout(() => {
            navigate("/login")
          }, 2000)
        } else {
          toast.error(
            error.response?.data?.message ||
            "Something went wrong ❌"
          )
        }
      }
    }
  })

  return (
    <div className="container flex justify-center items-center min-h-screen">
      <div className="bg-white shadow-lg p-8 w-full max-w-md space-y-6">
        <h1 className="text-2xl font-bold text-center">
          Confirm your email
        </h1>

        <p className="text-center text-gray-500">
          We have sent a confirmation link to your email.
        </p>

        <form onSubmit={formik.handleSubmit} className="space-y-4">
          <input
            type="email"
            name="email"
            placeholder="Enter your email"
            className="form-control"
            value={formik.values.email}
            onChange={formik.handleChange}
            required
          />

          <button type="submit" className="btn w-full text-white">
            Resend confirmation email
          </button>
        </form>
      </div>
    </div>
  )
}
