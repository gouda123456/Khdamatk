 
import { apiClient } from "./api-client"
 

  export async function sendDataToLogin(values){
    try {
        const optain={
                method:"POST",
                url:`/Auth`,
                data:{
                    email:values.email,
                    password:values.password
                }
            }
            const {data}= await apiClient.request(optain)
            console.log(data)
            return data
        
    } catch (error) {
        throw error
        
    }
}
 
 export async function sendDataToSignup(values){
    try {
        const optain={
                method:"POST",
                url:`/Auth/Register`,
                data:{
                    userName:values.userName,
                    email:values.email,
                    password:values.password
                }
            }
            const {data}= await apiClient.request(optain)
            return data
        
    } catch (error) {
        throw error
        
    }
}
  export async function confirmEmail({UserId,Code}){
    try {
        const optain={
                method:"GET",
                url:`/Auth/Confirm`,
                 params: {
        UserId,
        Code,
      },
               
            }
            const {data}= await apiClient.request(optain)
            return data
        
    } catch (error) {
        throw error
        
    }
    
}
// export async function sendConfirmEmail(email) {
//   try {
//      const options = {
//     method: "POST",
//     url: `/Auth/resend-confirmation-email`,
//     data:  { email } ,
//      headers: { "Content-Type": "application/json" }
//   };

//   const { data } = await apiClient.request(options);
//   return data;
//   } catch (error) {
//     throw error
//   }
// }
export async function sendConfirmEmail(email) {
  try {
    const options = {
      method: "POST",
      url: `/Auth/resend-confirmation-email`,
      data: { email },
      headers: { "Content-Type": "application/json" },
    };

    const { data } = await apiClient.request(options);
    return data;

  } catch (error) {
    // لو فيه response من السيرفر، ارجع data منه
    return error.response?.data || { isSuccess: false, message: error.message };
  }
}



 