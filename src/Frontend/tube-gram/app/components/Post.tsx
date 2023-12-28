import {ContentFeed} from "@/app/models/feed/contentFeed";

export default function Post({post} : {post: ContentFeed}) {
    return <div className='m-2'>
        <div>
            Posted by {post.username}
        </div>
        <div>
            CONTENT
        </div>
        <div>
            {post.description}
        </div>
    </div>
}