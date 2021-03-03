import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { routerTransition } from '../router.animations';
import { AuthService } from '../shared/services/auth.service';
import { ClientRequest } from '../shared/models/client-request.model';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    animations: [routerTransition()]
})
export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    error = '';
    constructor(
        private formBuilder: FormBuilder,
        private auThenService: AuthService,
        private route: ActivatedRoute,
        private router: Router,
        private spinner: NgxSpinnerService) {
                    // redirect to home if already logged in
        if (this.auThenService.isAuthenticated()) {
            // this.router.navigate(['/']);
            window.location.href = '/';
        }
        }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
         // get return url from route parameters or default to '/'
         this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }
    login() {

        this.submitted = true;
        // stop here if form is invalid
        if (this.loginForm.invalid) {
            return;
        }
        this.loading = true;
       this.spinner.show();
      // const item = this.loginForm.getRawValue();
       const client = new ClientRequest();
       client.userName = this.f.username.value;
       client.password = this.f.password.value;
       client.clientId = 'client_angular';
       client.clientSecret = 'secret';
       client.scope = 'openid email profile api.khoahoc';
       client.rememberMe = false;
       this.auThenService.login(client).subscribe(res => {
        this.auThenService.saveToken(res.accessToken);
        window.location.href = this.returnUrl;
        // this.router.navigate([this.returnUrl]);
       }, error => {
           this.error = error;
           this.loading = false;
       });
    }
}
