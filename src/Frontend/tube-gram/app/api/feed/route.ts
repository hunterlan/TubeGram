import {NextRequest} from "next/server";

export async function GET(request: NextRequest) {
    const token = request.headers.get('Authorization');
    const endpoint = process.env.API_URL + '/Feed';
    return await fetch(endpoint, {
        method: 'GET',
        headers: {
            "Content-Type": "application/json",
            "Authorization": token!
        }
    })
}