'use server'

import {cookies} from "next/headers";

async function deleteToken() {
    cookies().detele('token');
}