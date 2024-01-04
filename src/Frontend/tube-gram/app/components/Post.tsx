import {ContentFeedVm} from "@/app/models/feed/contentFeedVm";

export default async function Post({post} : {post: ContentFeedVm}) {
    const imageBuffer = Buffer.from(await post.blobData.arrayBuffer()).toString('base64');
    const base64Image = 'data:' + post.blobData.type + ';base64,' + imageBuffer;
    return <div className='m-2 max-h-fit flex flex-col w-[300px]'>
        <div>
            Posted by {post.username}
        </div>
        <div>
            <img src={base64Image} className='object-fill'></img>
            {/*<BlobImage blobJson={blobJson}></BlobImage>*/}
        </div>
        <div>
            {post.description}
        </div>
    </div>
}

function blobToBase64(blob: Blob): Promise<string> {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onloadend = () => {
            if (typeof reader.result === 'string') {
                resolve(reader.result);
            } else {
                reject(new Error('Failed to convert Blob to Base64.'));
            }
        };

        reader.onerror = () => {
            reject(new Error('Error reading Blob as Base64.'));
        };

        reader.readAsDataURL(blob);
    });
}