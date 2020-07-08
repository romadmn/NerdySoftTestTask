import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {IAnnouncementGet} from '../../../core/models/announcementGet';
import {AnnouncementService} from '../../../core/services/announcement.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {IAnnouncementDetails} from '../../../core/models/announcementDetails';
import {IAnnouncementPut} from '../../../core/models/announcementPut';

@Component({
  selector: 'app-announcement-edit-form',
  templateUrl: './announcement-edit-form.component.html',
  styleUrls: ['./announcement-edit-form.component.css']
})
export class AnnouncementEditFormComponent implements OnInit {
  @Output() onCancel: EventEmitter<void> = new EventEmitter<void>();
  @Input() announcement: IAnnouncementDetails;
  editAnnouncementForm: FormGroup;

  constructor(private announcementService: AnnouncementService ) { }

  ngOnInit(): void {
    this.editAnnouncementForm = new FormGroup({
      title: new FormControl(this.announcement.title, Validators.required),
      description: new FormControl(this.announcement.description, Validators.required),
    });
  }

  onSubmit() {
    let announcement: IAnnouncementPut = {
      id: this.announcement.id,
      fieldMasks: []
    };
    if (this.editAnnouncementForm.get('title').value !== this.announcement.title) {
      announcement.fieldMasks.push('Title');
      announcement.title = this.editAnnouncementForm.get('title').value;
    }
    if (this.editAnnouncementForm.get('description').value !== this.announcement.description) {
      announcement.fieldMasks.push('Description');
      announcement.description = this.editAnnouncementForm.get('description').value;
    }
    if (announcement.fieldMasks.length < 1) {
      this.cancel();
    } else {
      const formData: FormData = this.getFormData(announcement);
      this.announcementService.put(announcement.id, formData).subscribe(
        () => {
          this.onCancel.emit();
        });
    }
  }
  getFormData(announcement: IAnnouncementPut): FormData {
    const formData = new FormData();
    Object.keys(announcement).forEach((key, index) => {
      if (announcement[key]) {
        if (Array.isArray(announcement[key])) {
          announcement[key].forEach((i, index) => {
            if (key === 'fieldMasks') {
              formData.append(`${key}[${index}]`, announcement[key][index]);
            } else {
              formData.append(`${key}[${index}][id]`, announcement[key][index]['id']);
            }
          });
        } else {
          formData.append(key, announcement[key]);
        }
      }
    });
    return formData;
  }
  cancel() {
    this.onCancel.emit();
  }
}
