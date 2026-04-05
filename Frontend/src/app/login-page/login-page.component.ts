import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MasterService } from '../services/master.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {
  loginForm = new FormGroup({});
  sumbmit = false;

  constructor(private fb: FormBuilder,
    private masterService: MasterService,
    private router: Router,

  ) {
    this.loginForm = this.fb.group({
      user_name: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  ngOnInit(): void {
    sessionStorage.clear();
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.sumbmit = true;
      this.masterService.GetLogin(this.loginForm.value)
        .subscribe(d => {
          if (d.isSuccess == true) {
            // alert('OK');
            this.sumbmit=false;
            this.router.navigate(['./QueryExecuter']);
          }
          else {
            Swal.fire({
              title: d.message,
              text: '',
              icon: 'error',
              showCancelButton: false,
              confirmButtonText: 'OK',
              //cancelButtonText: 'No'
            });
            this.sumbmit=false;
          }
        });
    } 
    else {
      Swal.fire({
        title: 'Please enter Valid Username and Password',
        text: '',
        icon: 'error',
        showCancelButton: false,
        confirmButtonText: 'OK',
        //cancelButtonText: 'No'
      });
      this.sumbmit=false;
      // console.log('Form is invalid');
    }
    // this.sumbmit=false;
  }

  onReset(): void {
    this.loginForm.reset();
  }

}
