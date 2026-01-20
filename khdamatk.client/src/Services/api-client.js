import axios from "axios";
import { API_CONFIG } from "../Config";

export const apiClient=axios.create({
    baseURL:API_CONFIG.baseURL,
    timeout:30000
});


 