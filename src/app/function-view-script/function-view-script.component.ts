import { Component, OnInit, ViewChild } from '@angular/core';
import { MasterService } from '../services/master.service';
import Swal from 'sweetalert2';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-function-view-script',
  templateUrl: './function-view-script.component.html',
  styleUrls: ['./function-view-script.component.css']
})
export class FunctionViewScriptComponent implements OnInit {
  userDetail: any;
  funViewDTO: any;
  form = new FormGroup({});
  dbDTO: any;
  submitted = false;
  schemaDTO: any;
  scr: string = "Script";
  srcflg = true;
  taflg = true;
  fb = new FormBuilder();
  textareaContent?: string;
  flgclo = true;

  constructor(
    private masterService: MasterService,
    private router: Router
  ) { }

  get f() { return this.form.controls; }

  ngOnInit(): void {
    var sessionvalue: any = sessionStorage.getItem('user');
    if (sessionvalue == null) {
      return this.logout();
    }
    this.userDetail = JSON.parse(decodeURIComponent(escape(atob(sessionvalue))));
    console.log(this.userDetail);
    this.form = this.fb.group({
      token: [this.userDetail.data.tokens],
      Schema: ['', Validators.required],
      name: ['', Validators.required],
      Database: ['', Validators.required],
      qid: ['', Validators.required]
    });
    this.getDatabase();
  }

  getDatabase() {
    this.masterService.GetDataBase({ token: this.userDetail.data.tokens })
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
        this.dbDTO = d.data;
      });
  }

  getSchemas() {
    this.taflg = true;
    this.schemaDTO = [];
    this.funViewDTO = [];
    this.form.controls['Schema'].setValue('');
    this.form.controls['name'].setValue('');
    this.form.controls['qid'].setValue('');
    this.srcflg = true;
    this.masterService.GetSchemas({
      token: this.form.value.token,
      database: this.form.controls['Database'].value
    })
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
        this.schemaDTO = d.data;
      });
  }

  onChangeSchema() {
    this.funViewDTO = [];
    this.taflg = true;
    this.form.controls['name'].setValue('');
    this.form.controls['qid'].setValue('');
    this.srcflg = true;
  }

  logout() {
    this.masterService.logout();
  }

  getFunctionViewList() {
    this.form.controls['name'].setValue('');
    this.taflg = true;
    this.srcflg = true;
    this.scr = this.form.controls['qid'].value == 3 ? 'Function' : 'Views';
    this.masterService.GetFunctionViewList({
      token: this.form.value.token,
      schema: this.form.controls['Schema'].value,
      database: this.form.controls['Database'].value,
      qid: this.form.controls['qid'].value
    })
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
        this.srcflg = d.dataLength != 0;
        this.funViewDTO = d.data;
      });
  }

  onSubmit() {
    this.submitted = true;
    this.taflg = false;
    if (this.form.valid) {
      this.masterService.GetScript(this.form.value)
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
          this.taflg = false;
          const selectedValues = d.data[0].script_defination;
          (document.getElementById('textarea') as HTMLTextAreaElement).value = selectedValues;
          this.textareaContent = selectedValues;
          this.adjustTextareaHeight();
        });
    }
    this.submitted = false;
  }

  closeTable() {
    this.taflg = !this.taflg;
  }

  adjustTextareaHeight(event?: Event): void {
    const textarea = document.getElementById('textarea') as HTMLTextAreaElement;
    textarea.style.height = 'auto';
    textarea.style.width = '100%';
    textarea.style.height = `${textarea.scrollHeight}px`;
  }

  copyToClipboard() {
    const textarea = document.getElementById('textarea') as HTMLTextAreaElement;
    textarea.disabled = false;
    textarea.select();
    try {
      document.execCommand('copy');
      Swal.fire({
        title: 'Copied!',
        text: 'Text copied to clipboard',
        icon: 'success',
        timer: 1000,
        showConfirmButton: false
      });
    } catch (err) {
      console.error('Failed to copy text', err);
    }
    window.getSelection()?.removeAllRanges();
    textarea.disabled = true;
  }

  refresh(dropdownValue: any) {
    dropdownValue.value = '';
    this.ngOnInit()
  }
}
