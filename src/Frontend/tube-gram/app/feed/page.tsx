'use client'
import {useUserStore} from "@/app/store/zustand";
import {redirect} from "next/navigation";

export default async function Feed() {
    const { userToken } = useUserStore();

    if (userToken === undefined) {
        redirect('/');
    }

    return <div>Test</div>
}