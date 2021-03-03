import { AnnouncementUser } from './announcement-user.model';
export class Announcement {
    id: string;
    title: string;
    content: string;
    image: string;
    entityType: string;
    entityId: string;
    userId: string;
    userFullName: string;
    creationTime: string;
    lastModificationTime: string;
    tmpHasRead: boolean;
    status: number;
    announcementUser: AnnouncementUser[];
}
