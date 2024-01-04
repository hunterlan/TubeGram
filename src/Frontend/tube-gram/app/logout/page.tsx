'use client'
import {useEffect} from "react";
import {userLogout} from "@/app/actions";

export default function Logout() {
    useEffect(() => {
        userLogout();
    }, []);
}