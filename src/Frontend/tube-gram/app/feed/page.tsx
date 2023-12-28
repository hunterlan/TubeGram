'use client'
import {useUserStore} from "@/app/store/zustand";
import {redirect} from "next/navigation";
import {fetchRequest} from "@/app/utils/fetcher";
import {ContentFeed} from "@/app/models/feed/contentFeed";
import Post from "@/app/components/Post";

export default async function Feed() {
    const { userToken } = useUserStore();

    if (userToken === undefined) {
        redirect('/');
    }

    let posts = await fetchRequest<ContentFeed[]>('api/feed',
        undefined, 'GET', 'application/json', undefined);
    posts = posts!.sort((e1, e2) => (e1.timestamp > e2.timestamp ? -1 : 1))

    return posts.map((post, index) => {
        return <Post key={index} post={post}></Post>
    })
}