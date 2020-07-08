import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';
import {AnnouncementsComponent} from './shared/components/announcements/announcements.component';
import {AnnouncementDetailsComponent} from './shared/components/announcement-details/announcement-details.component';
import {RefDirective} from './shared/directives/ref.directive';
import {AnnouncementEditFormComponent} from './shared/components/announcement-edit-form/announcement-edit-form.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    AnnouncementsComponent,
    AnnouncementEditFormComponent,
    RefDirective,
    AnnouncementDetailsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: AnnouncementsComponent, pathMatch: 'full' },
      { path: 'announcement/:id', component: AnnouncementDetailsComponent }
    ])
  ],
  providers: [],
  entryComponents: [AnnouncementEditFormComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
