import {ContentFeedVm} from "@/app/models/feed/contentFeedVm";

export default function Post({post} : {post: ContentFeedVm}) {
    return <div className='m-2'>
        <div>
            Posted by {post.username}
        </div>
        <div>
            <img src={URL.createObjectURL(post.blobData)}></img>
            {/*IMAGE CONTENT*/}
        </div>
        <div>
            {post.description}
        </div>
    </div>
}