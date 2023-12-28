import {NextRequest} from "next/server";

export async function GET(request: NextRequest) {
    const searchParams = request.nextUrl.searchParams
    const count = searchParams.get('c')
    const token = request.headers.get('Authorization');
    const endpoint = process.env.API_URL + '/Image?c=' + count;
    return await fetch(endpoint, {
        method: 'GET',
        headers: {
            "Content-Type": "application/json",
            "Authorization": token!
        }
    })
}