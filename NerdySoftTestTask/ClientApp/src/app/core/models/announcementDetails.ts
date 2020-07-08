import {IAnnouncementGet} from './announcementGet';

export interface IAnnouncementDetails {
  id?: number;
  title: string;
  description: string;
  dateAdded: Date;
  similarAnnouncements: IAnnouncementGet[];
}
