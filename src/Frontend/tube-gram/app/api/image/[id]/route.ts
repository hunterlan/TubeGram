import {NextRequest} from "next/server";

export async function GET(request: NextRequest, { params }: { params: { id: string }}) {
    const token = request.headers.get('Authorization');
    const endpoint = process.env.API_URL + '/Image/' + params.id;
    return await fetch(endpoint, {
        method: 'GET',
        headers: {
            "Authorization": token!
        }
    });
}