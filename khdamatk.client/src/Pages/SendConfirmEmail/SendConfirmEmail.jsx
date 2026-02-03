import { useState } from "react";
import { useFormik } from "formik";
import * as yup from "yup";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import logoPhoto from "../../assets/Images/Logo.png";
import { sendConfirmEmail } from "../../Services/auth-services";

export default function SendConfirmEmail() {
  const [serverError, setServerError] = useState(null);
  const navigate = useNavigate();

  async function handelSendConfirmEmail(values) {
    setServerError(null); // امسح أي رسالة خطأ سابقة
    try {
      const response = await sendConfirmEmail(values.email);

      if (response.isSuccess) {
        toast.success("تم إرسال رابط تأكيد الإيميل");
        setTimeout(() => navigate("/login"), 3000); // redirect بعد 3 ثواني
      } else {
        const errorMessage =
          response.errors?.[0]?.message || response.message || "حدث خطأ";
        setServerError(errorMessage);
      }
    } catch (error) {
      const errorMessage =
        error.response?.data?.errors?.[0]?.message ||
        error.response?.data?.message ||
        error.message ||
        "حدث خطأ غير معروف";
      setServerError(errorMessage);
      console.error("Full error:", error);
    }
  }

  const validationSchema = yup.object({
    email: yup
      .string()
      .email("الإيميل غير صالح")
      .required("الإيميل مطلوب"),
  });

  const formik = useFormik({
    initialValues: { email: "" },
    validationSchema,
    onSubmit: handelSendConfirmEmail,
  });

  return (
    <div className="container items-center justify-between gap-16 grid lg:grid-cols-2">
      {/* Left side */}
      <div className="p-8 bg-white shadow-lg rounded-lg">
        <h1 className="text-2xl font-bold mb-4 text-center">تأكيد الإيميل</h1>
        <form onSubmit={formik.handleSubmit}>
          <p className="text-gray-600 text-sm mb-4 text-center">
            أدخل الإيميل الخاص بك لتأكيد الحساب
          </p>

          <div className="relative w-full mb-4">
            <span className="absolute left-4 -top-3 bg-white px-2 text-sm text-gray-500">
              Email
            </span>
            <input
              type="email"
              name="email"
              value={formik.values.email}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className="form-control w-full px-4 py-2 border rounded"
            />
            {formik.touched.email && formik.errors.email && (
              <p className="text-red-500 text-sm mt-1">{formik.errors.email}</p>
            )}
          </div>

          {serverError && (
            <p className="text-red-500 text-sm mb-2 text-center">{serverError}</p>
          )}

          <button
            type="submit"
            className="btn text-white w-full py-2 rounded bg-blue-600 hover:bg-blue-700"
          >
            إرسال رابط التأكيد
          </button>
        </form>
      </div>

      {/* Right side */}
      <div className="flex justify-center items-center">
        <img src={logoPhoto} alt="Logo" className="w-64" />
      </div>
    </div>
  );
}
