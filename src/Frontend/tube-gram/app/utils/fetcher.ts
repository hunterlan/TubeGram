export async function fetchRequest<Type>(url: string, jsonBody: string | undefined, method: string, contentType: string,
                                         token: string | undefined): Promise<Type|undefined> {
    const headerRequest = new Headers();
    headerRequest.set('Content-Type', method);
    if (token !== undefined) {
        headerRequest.set('Authorization', 'Bearer ' + token);
    }
    const response = await fetch(url, {
        body: jsonBody,
        method: method,
        headers: headerRequest
    });

    if (response.ok) {
        return await response.json();
    }

    return undefined;
}
