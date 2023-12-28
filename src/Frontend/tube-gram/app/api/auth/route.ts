export async function POST(request: Request) {
    const credentialsJson = JSON.stringify(await request.json());
    const endpoint = process.env.API_URL + '/User';
    return await fetch(endpoint, {
        method: 'POST',
        body: credentialsJson,
        headers: {
            "Content-Type": "application/json",
        }
    });
}