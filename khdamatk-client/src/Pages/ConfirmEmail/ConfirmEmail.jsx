// import { useEffect } from "react"
// import { useNavigate, useSearchParams } from "react-router-dom"
// import axios from "axios"
// import { API_CONFIG } from "../../Config"
// import { toast } from "react-toastify"

// export default function ConfirmEmail() {
//   const navigate = useNavigate()
//   const [searchParams] = useSearchParams()

//   useEffect(() => {
//     const userId = searchParams.get("UserId")
//     const code = searchParams.get("Code")

//     async function confirmEmail() {
//       try {
//         await axios.get(
//           `${API_CONFIG.baseURL}/Auth/Confirm`,
//           {
//             params: {
//               UserId: userId,
//               Code: code
//             }
//           }
//         )

//         toast.success("Email confirmed successfully ✅")

//         setTimeout(() => {
//           navigate("/login")
//         }, 2000)

//       } catch (error) {
//         toast.error("Invalid or expired confirmation link ❌")
//       }
//     }

//     if (userId && code) {
//       confirmEmail()
//     }
//   }, [])

//   return (
//     <div className="flex justify-center items-center min-h-screen">
//       <h1 className="text-xl font-semibold">
//         Confirming your email...
//       </h1>
//     </div>
//   )
// }
import { useEffect, useState } from "react"
import { useSearchParams, useNavigate } from "react-router-dom"
import axios from "axios"

export default function ConfirmEmail() {
  const [searchParams] = useSearchParams()
 const navigate = useNavigate()

  const [status, setStatus] = useState("wait") // wait | success | error
  const [message, setMessage] = useState("")

  useEffect(() => {
    const userId = searchParams.get("UserId")
    const code = searchParams.get("Code")

    if (!userId || !code) {
      setStatus("error")
      setMessage("رابط تأكيد البريد الإلكتروني غير صالح")
      return
    }

    const fetchData = async () => {
      try {
        const res = await axios.get(
          "https://localhost:7210/Auth/Confirm",
          {
            params: {
              UserId: userId,
              Code: code
            }
          }
        )

        if (res.data?.isSuccess) {
          setStatus("success")
          setMessage(res.data.message || "تم تأكيد البريد الإلكتروني بنجاح")
        } else {
          setStatus("error")
          setMessage(res.data.message || "فشل تأكيد البريد الإلكتروني")
        }

      } catch (err) {
        if (err.response?.status === 409) {
          setStatus("success")
          setMessage("البريد الإلكتروني مؤكد بالفعل ✅")
        } else {
          setStatus("error")
          setMessage(
            err.response?.data?.errors?.[0]?.message ||
            err.response?.data?.message ||
            "فشل تأكيد البريد الإلكتروني"
          )
        }
      }
    }

    fetchData()
  }, [searchParams])

  // ===== UI =====
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 px-4">
      <div className="bg-white shadow-lg rounded-xl p-8 max-w-md w-full text-center">

        {status === "wait" && (
          <>
            <div className="animate-spin mx-auto mb-4 h-10 w-10 rounded-full border-4 border-purple-500 border-t-transparent"></div>
            <h2 className="text-xl font-semibold">
              جاري تأكيد البريد الإلكتروني...
            </h2>
            <p className="text-gray-500 mt-2">
              من فضلك انتظر لحظات
            </p>
          </>
        )}

        {status !== "wait" && (
          <>
            <div
              className={`mx-auto mb-4 h-14 w-14 flex items-center justify-center rounded-full text-2xl
                ${status === "success" ? "bg-green-100 text-green-600" : "bg-red-100 text-red-600"}`}
            >
              {status === "success" ? "✓" : "✕"}
            </div>

            <h2
              className={`text-2xl font-bold mb-2
                ${status === "success" ? "text-green-600" : "text-red-600"}`}
            >
              {status === "success"
                ? "تم تأكيد البريد الإلكتروني"
                : "فشل تأكيد البريد الإلكتروني"}
            </h2>

            <p className="text-gray-600 mb-6">
              {message}
            </p>

            {status === "success" && (
              <button
                onClick={() => navigate("/login")}
                className="w-full py-3 bg-purple-600 text-white rounded-lg font-semibold hover:bg-purple-700 transition"
              >
                الذهاب إلى تسجيل الدخول
              </button>
            )}
          </>
        )}

      </div>
    </div>
  )
}

