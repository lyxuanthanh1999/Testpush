import { AccountRoutingModule } from './account-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { AccountDetailComponent } from './account-detail/account-detail.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AnnouncementComponent } from './announcement/announcement.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule, BsModalService } from 'ngx-bootstrap/modal';
import { BlockUIModule } from 'primeng/blockui';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { EditorModule } from 'primeng/editor';
import { FileUploadModule } from 'primeng/fileupload';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { KeyFilterModule } from 'primeng/keyfilter';
import { MessageModule } from 'primeng/message';
import { MessagesModule } from 'primeng/messages';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { RadioButtonModule } from 'primeng/radiobutton';
import { TableModule } from 'primeng/table';
import { TreeTableModule } from 'primeng/treetable';
import { ValidationMessageModule } from '../../shared/modules/validation-message/validation-message.module';
import { SharedPipesModule } from '../../shared/pipes/shared-pipes.module';
import { NotificationService } from '../../shared/services';
import { AnnouncementDetailComponent } from './announcement/announcement-detail/announcement-detail.component';

@NgModule({
  declarations: [AccountDetailComponent, ChangePasswordComponent, AnnouncementComponent, AnnouncementDetailComponent],
  imports: [
    CommonModule,
    AccountRoutingModule,
    PanelModule,
    ButtonModule,
    TableModule,
    PaginatorModule,
    BlockUIModule,
    InputTextModule,
    FileUploadModule,
    ProgressSpinnerModule,
    ValidationMessageModule,
    FormsModule,
    ReactiveFormsModule,
    KeyFilterModule,
    CalendarModule,
    CheckboxModule,
    MessageModule,
    MessagesModule,
    TreeTableModule,
    InputNumberModule,
    DropdownModule,
    InputTextareaModule,
    RadioButtonModule,
    EditorModule,
    SharedPipesModule,
    ModalModule.forRoot()
  ],
  providers: [
    NotificationService,
    BsModalService,
    DatePipe
  ]
})
export class AccountModule { }
