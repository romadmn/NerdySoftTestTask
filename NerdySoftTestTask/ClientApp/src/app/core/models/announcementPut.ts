export interface IAnnouncementPut {
  id?: number;
  title?: string;
  description?: string;
  dateAdded?: Date;
  fieldMasks?: string[];
}
