import {ContentFeedVm} from "@/app/models/feed/contentFeedVm";
import {isEmptyOrSpaces} from "@/app/utlis";

export default async function Post({post} : {post: ContentFeedVm}) {
    const imageBuffer = Buffer.from(await post.blobData.arrayBuffer()).toString('base64');
    const base64Image = 'data:' + post.blobData.type + ';base64,' + imageBuffer;
    const isEmptyDescription = isEmptyOrSpaces(post.description);
    return <div className="rounded overflow-hidden shadow-lg m-2">
        <img className="w-full h-[300px] object-cover" src={base64Image} alt="Sunset in the mountains"/>
            <div className="px-6 py-4">
                <div className="font-bold text-xl mb-2">Posted by {post.username}</div>
                <p className="text-gray-700 text-base">
                    {isEmptyDescription ? (<p>User did not add any description</p>) :
                        (<p>{post.description}</p>)}
                </p>
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