export class ContentFeed {
    id: number;
    type: string;
    username: string;
    description: string;
    timestamp: number;


    constructor(id: number, type: string, username: string, description: string, timestamp: number) {
        this.id = id;
        this.type = type;
        this.username = username;
        this.description = description;
        this.timestamp = timestamp;
    }
}