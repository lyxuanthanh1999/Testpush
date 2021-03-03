import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { StatisticsRoutingModule } from './statistics-routing.module';
import { NewUserComponent } from './new-user/new-user.component';
import { RevenueComponent } from './revenue/revenue.component';
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
import { NotificationService } from '../../shared/services';
import { ChartModule } from 'primeng/chart';
import { SharedPipesModule } from './../../shared/pipes/shared-pipes.module';
@NgModule({
  declarations: [NewUserComponent,
     RevenueComponent
    ],
  imports: [
    CommonModule,
    StatisticsRoutingModule,
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
    ChartModule,
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
export class StatisticsModule { }
