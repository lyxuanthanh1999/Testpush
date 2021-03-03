import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';

@NgModule({
    imports: [CommonModule, MessageModule, MessagesModule, TranslateModule, FormsModule, ReactiveFormsModule, LoginRoutingModule],
    declarations: [LoginComponent],
    providers: [NgxSpinnerService]
})
export class LoginModule {}
