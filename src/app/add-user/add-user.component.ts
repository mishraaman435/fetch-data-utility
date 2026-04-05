import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MasterService } from '../services/master.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  registerForm = new FormGroup({});
  userDetail: any;
  designationDTO?: any = [];

  constructor(private fb: FormBuilder,
    private masterService: MasterService,
    private router: Router,
    private modalService: NgbModal,

  ) { }

  ngOnInit(): void {
    var sessionvalue: any = sessionStorage.getItem('user');
    if (sessionvalue == null) {
      return this.logout();
    }
    this.userDetail = JSON.parse(decodeURIComponent(escape(atob(sessionvalue))));
    this.registerForm = this.fb.group({
      token: [this.userDetail.data.tokens],
      first_name: ['', Validators.required],
      last_name: ['', Validators.required],
      mobile_number: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
      contact_email: ['', [Validators.required, Validators.email]],
      user_name: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      roll: ['2'],
      designation: ['', Validators.required]
    });
    this.getDesignation();
    // console.log('close');
  }
  logout() {
    this.modalService.dismissAll();
    this.masterService.logout();
  }
  getDesignation() {
    this.designationDTO = [];
    this.masterService.GetDesignation({ Token: this.registerForm.value.token })
      .subscribe(d => {
        if (d.responseCode == 440) {
          Swal.fire({
            title: d.message,
            text: '',
            icon: 'error',
            showCancelButton: false,
            confirmButtonText: 'LogIn',
          });
          this.logout();
        }
        else if (d.isSuccess) {
          this.designationDTO = d.data;
        }
      }
      );
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.masterService.AddUser(this.registerForm.value)
        .subscribe(d => {
          if (d.responseCode == 440) {
            Swal.fire({
              title: d.message,
              text: '',
              icon: 'error',
              showCancelButton: false,
              confirmButtonText: 'LogIn',
            });
            this.modalService.dismissAll();
            this.logout();
          }
          else if (d.isSuccess) {
            Swal.fire({
              title: d.message,
              text: '',
              icon: 'success',
              showCancelButton: false,
              confirmButtonText: 'OK',
            });
            this.modalService.dismissAll();
          } else {
            Swal.fire({
              title: d.message,
              text: '',
              icon: 'warning',
              showCancelButton: false,
              confirmButtonText: 'OK',
            });
          }
        });
      // console.log('Form Submitted!', this.registerForm.value);
      // Here, you can handle the form submission, e.g., send the data to the server
    } else {

    }
  }
}
