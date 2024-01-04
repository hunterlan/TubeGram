import {ContentFeed} from "@/app/models/feed/contentFeed";

export class ContentFeedVm extends ContentFeed {
    constructor(id: number, type: string, username: string,
    description: string, timestamp: number, public blobData: Blob
    ) {
        super(id, type, username, description, timestamp);
    }
}