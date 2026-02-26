 
import { apiClient } from "./api-client";

export async function getHomeData() {
  try {
    const options = {
      url: "api/Home",
      method: "GET",
    };

    const response = await apiClient.request(options);
    return response.data;
  } catch (error) {
    throw error;
  }
}
