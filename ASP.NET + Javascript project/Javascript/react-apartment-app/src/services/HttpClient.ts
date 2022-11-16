import axios from "axios";

export const httpClient = axios.create({
    baseURL: "https://apartmentwebapp.azurewebsites.net/api/v1/",
    headers: {
    "Content-type": "application/json"
    }
});

export default httpClient;