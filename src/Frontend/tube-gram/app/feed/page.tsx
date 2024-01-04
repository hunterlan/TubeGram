import {cookies} from "next/headers";
import {UserToken} from "@/app/models/users/user-token";
import {redirect} from "next/navigation";
import {ContentFeed} from "@/app/models/feed/contentFeed";
import {ContentFeedVm} from "@/app/models/feed/contentFeedVm";
import Post from "@/app/components/Post";

export default async function Feed() {
    const userTokenCookie = cookies().get('token');
    if (userTokenCookie === undefined) {
        redirect('/login');
    }

    const userToken = JSON.parse(userTokenCookie.value) as UserToken;
    let posts = await getPosts(userToken.token);
    if (typeof posts !== 'number') {
        posts = posts!.sort((e1, e2) => (e1.timestamp > e2.timestamp ? -1 : 1));
        const postsVm = await mapToContentFeedVm(posts!, userToken.token);
        return postsVm.map((post, index) => {
            return <Post key={index} post={post}></Post>
        })
    } else {
        if (posts === 401) {
            redirect('/logout');
        }
        return <div>During fetching posts something went wrong.</div>
    }
}

export async function getPosts(token: string): Promise<ContentFeed[]|number> {
    const response = await fetch(process.env.API_URL + '/Feed', {
        method: 'GET',
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        cache: 'no-store'
    });

    if (response.ok) {
        return await response.json();
    } else {
        return response.status;
    }
}

export async function mapToContentFeedVm(posts: ContentFeed[], token: string): Promise<ContentFeedVm[]> {
    const feed: ContentFeedVm[] = [];

    for (const post of posts) {
        if (post.type === 'Photo') {
            const blobData = await getImage(post.id, token);
            if (typeof blobData !== 'number') {
                feed.push(new ContentFeedVm(post.id, post.type, post.username, post.description, post.timestamp, blobData));
            } else {
                if (blobData === 401) {
                    redirect('/logout');
                }
                feed.push(new ContentFeedVm(post.id, post.type, post.username, post.description, post.timestamp, new Blob()));
            }
        } else {
            //TODO: write code for video, it should be stream
        }
    }

    return feed;
}

export async function getImage(id: number, token: string): Promise<Blob|number> {
    const response = await fetch(process.env.API_URL + '/Image/' + id, {
        method: 'GET',
        headers: {
            "Authorization": "Bearer " + token
        },
    })

    if (response.ok) {
        return await response.blob();
    } else {
        return response.status;
    }
}