import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {IAnnouncementGet} from '../models/announcementGet';
import {IAnnouncementDetails} from '../models/announcementDetails';
import {IAnnouncementPost} from '../models/announcementPost';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<IAnnouncementGet[]> {
    return this.http.get<IAnnouncementGet[]>('/api/announcements/');
  }

  getById(id: number): Observable<IAnnouncementDetails> {
    return this.http.get<IAnnouncementDetails>('/api/announcements/' + id);
  }

  post(announcement: IAnnouncementPost) {
    return this.http.post('/api/announcements/', announcement);
  }

  put(announcementId: number, announcement: FormData) {
    return this.http.put('/api/announcements/' + announcementId, announcement);
  }
  delete(announcementId: number) {
    return this.http.delete('/api/announcements/' + announcementId);
  }

}
