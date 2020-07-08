import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AnnouncementService} from '../../../core/services/announcement.service';
import {switchMap} from 'rxjs/operators';
import {IAnnouncementGet} from '../../../core/models/announcementGet';
import {IAnnouncementDetails} from '../../../core/models/announcementDetails';
import {RefDirective} from '../../directives/ref.directive';
import {AnnouncementEditFormComponent} from '../announcement-edit-form/announcement-edit-form.component';

@Component({
  selector: 'app-announcement-details',
  templateUrl: './announcement-details.component.html',
  styleUrls: ['./announcement-details.component.css']
})
export class AnnouncementDetailsComponent implements OnInit {
  @ViewChild(RefDirective, {static: false}) refDir: RefDirective;
  announcementId: number;
  announcement: IAnnouncementDetails;
  constructor(
    private announcementService: AnnouncementService,
    private routeActive: ActivatedRoute,
    private resolver: ComponentFactoryResolver,
    private router: Router
  ) {}

  ngOnInit() {
    this.routeActive.paramMap.pipe(
      switchMap(params => params.getAll('id'))
    )
    .subscribe(data => this.announcementId = +data);
    this.announcementService.getById(this.announcementId).subscribe((value: IAnnouncementDetails) => {
      this.announcement = value;
    });
  }

  showEditForm(announcement: IAnnouncementDetails) {
    const formFactory = this.resolver.resolveComponentFactory(AnnouncementEditFormComponent);
    const instance = this.refDir.containerRef.createComponent(formFactory).instance;
    instance.announcement = announcement;
    instance.onCancel.subscribe(() => {this.refDir.containerRef.clear(); this.ngOnInit(); });
  }
  deleteAnnouncement(id: number) {
    this.announcementService.delete(id).subscribe(() => {
      this.router.navigate(['/']);
    });
  }
}
