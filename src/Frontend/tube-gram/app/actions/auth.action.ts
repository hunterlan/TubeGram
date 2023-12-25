import {UserCredentials} from "@/app/models/users/user-credentials";

export async function login(username: string, password: string) {
    const credentialsJson = JSON.stringify(new UserCredentials(username, password));
    const endpoint = process.env.API_URL + '/User';
    const response = await fetch(endpoint, {
        method: 'POST',
        body: credentialsJson,
        headers: {
            "Content-Type": "application/json",
        }
    });

    if (response.ok) {
        console.log(response);
    } else {
        console.log(response)
    }
}