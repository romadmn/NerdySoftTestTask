import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';
import {IAnnouncementGet} from '../../../core/models/announcementGet';
import {AnnouncementService} from '../../../core/services/announcement.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {IAnnouncementPost} from '../../../core/models/announcementPost';
import {RefDirective} from '../../directives/ref.directive';
import {IAnnouncementDetails} from '../../../core/models/announcementDetails';
import {AnnouncementEditFormComponent} from '../announcement-edit-form/announcement-edit-form.component';

@Component({
  selector: 'app-announcements',
  templateUrl: './announcements.component.html',
  styleUrls: ['./announcements.component.css']
})
export class AnnouncementsComponent implements OnInit {
  @ViewChild(RefDirective, {static: false}) refDir: RefDirective;
  announcements: IAnnouncementGet[];
  addAnnouncementForm: FormGroup = new FormGroup({
    id: new FormControl(0),
    title: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
  });

  constructor(private announcementService: AnnouncementService,
              private resolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
    this.getAllAnnouncements();
  }
  getAllAnnouncements() {
      this.announcementService.getAll().subscribe((value: IAnnouncementGet[]) => {
        this.announcements = value;
      });
  }

  onSubmit() {
    const newAnnouncement: IAnnouncementPost = {
      id: 0,
      title: this.addAnnouncementForm.get('title').value,
      description: this.addAnnouncementForm.get('description').value
    };
    console.log(newAnnouncement);
    this.announcementService.post(newAnnouncement).subscribe(() => {
      this.ngOnInit();
      this.addAnnouncementForm.reset();
    });
  }
  showEditForm(announcement: IAnnouncementGet) {
    const formFactory = this.resolver.resolveComponentFactory(AnnouncementEditFormComponent);
    const instance = this.refDir.containerRef.createComponent(formFactory).instance;
    instance.announcement = {
      id: announcement.id,
      title: announcement.title,
      description: announcement.description,
      dateAdded: announcement.dateAdded,
      similarAnnouncements: null
    };
    instance.onCancel.subscribe(() => {this.refDir.containerRef.clear(); this.ngOnInit(); });
  }
  deleteAnnouncement(id: number) {
    this.announcementService.delete(id).subscribe(() => {
      this.ngOnInit();
    });
  }
}
