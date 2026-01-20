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