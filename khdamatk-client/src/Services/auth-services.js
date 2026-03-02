import axios from "axios"
import { API_CONFIG } from "../Config"
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
 export async function sendDataToConfirmEmail(values){
    try {
       const option = {
         method: "POST",
         url: "/Auth/resend-confirmation-email",
         data: {
           email: values.email,
         },
       }
   
       const { data } = await apiClient.request(option)
       console.log(data)
       return data
   
     } catch (error) {
       throw error
     }
}
export async function confirmEmail(token) {
  try {
    const { data } = await apiClient.get(`/Auth/Confirm?token=${token}`);
    return data;
  } catch (error) {
    throw error;
  }
}