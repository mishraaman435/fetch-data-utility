import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MasterService } from '../services/master.service';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import * as XLSX from 'xlsx';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import { XLSX$Consts, XLSX$Utils } from 'xlsx';
import Swal from 'sweetalert2';
import { ActivatedRoute, Router } from '@angular/router';
import { MatAutocomplete } from '@angular/material/autocomplete';


@Component({
  selector: 'app-query-page',
  templateUrl: './query-page.component.html',
  styleUrls: ['./query-page.component.css']
})
export class QueryPageComponent implements OnInit {

  dynamicquery = new FormGroup({});
  dbDTO: any;
  userDetail: any;
  schemaDTO: any;
  submit = false;
  tableDTO: any;
  virtualData: any = [];
  resultMessage: any;
  QS?: boolean;
  dataSource = new MatTableDataSource<any>();
  displayedColumns: string[] = [];
  filteredSchemas: any[] = [];
  filteredTable: any[] = [];
  prq?: string;
  flgclo = false;
  datalength: any = 0;
  p: any;
  dataSourceWithPageSize = new MatTableDataSource<any>();

  pageSizes = [3, 5, 7];


  constructor(
    private fb: FormBuilder,
    private masterService: MasterService,
    private router: Router,
    private route: ActivatedRoute,
    private renderer: Renderer2,
    private el: ElementRef
  ) { }

  @ViewChild('auto', { static: false }) auto?: MatAutocomplete;
  @ViewChild('paginator') paginator: MatPaginator | any;
  @ViewChild('paginatorPageSize') paginatorPageSize: MatPaginator | any;



  ngOnInit(): void {
    var sessionvalue: any = sessionStorage.getItem('user');
    if (sessionvalue == null) {
      return this.logout();
    }

    this.userDetail = JSON.parse(decodeURIComponent(escape(atob(sessionvalue))));
    this.dynamicquery = this.fb.group({
      token: new FormControl(this.userDetail.data.tokens),
      Database: new FormControl('', [Validators.required]),
      query: new FormControl('', [Validators.required])
    });
    this.getDatabase();

    // Check for transaction type query parameter
    this.route.queryParams.subscribe(params => {
      if (params['transType']) {
        this.getTransaction(params['transType']);
      }
    });
  }

  getDatabase() {
    this.masterService.GetDataBase({ token: this.dynamicquery.get('token')?.value })
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

  getTransaction(option: string) {
    this.resultMessage = '';
    this.datalength = 0;
    var usid;
    if (option == 'A') usid = '0';
    else usid = this.userDetail.data.user_id.toString();

    this.masterService.GetTransaction({
      Token: this.userDetail.data.tokens,
      user_id: usid
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
        this.QS = d.isSuccess;
        if (d.isSuccess) {
          this.resultMessage = d.message;
          this.flgclo = true;
          this.dataSource.data = d.data;
          this.datalength = d.dataLength;
          this.displayedColumns = Object.keys(d.data[0]);
        } else {
          this.resultMessage = d.message || 'Failed to retrieve data';
          this.flgclo = false;
          this.datalength = d.dataLength;
        }
      });
  }

  closeTable() {
    this.flgclo = false;
  }

  getSchemas() {
    this.masterService.GetSchemas(this.dynamicquery.value)
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

  getTable(curSch: string) {
    this.masterService.GetTable({
      token: this.dynamicquery.value.token,
      schema: curSch,
      database: this.dynamicquery.value.Database
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
        this.tableDTO = d.data;
      });
  }


  suggestNames(inputText: any) {
    if (inputText.target.value.lastIndexOf('.') && inputText.target.value.lastIndexOf('.') > (inputText.target.value.lastIndexOf('from ') + 5)) {
      this.suggestTable(inputText);
    }
    else if (inputText.target.value.lastIndexOf('from ')) {
      this.suggestSchema(inputText);
    }
  }

  suggestSchema(inputText: any) {
    const query = inputText.target.value.toLowerCase();
    this.prq = inputText.target.value;
    const fromIndex = query.lastIndexOf('from ');
    if (fromIndex !== -1) {
      const searchText = query.substring(fromIndex + 5);
      this.filteredSchemas = this.schemaDTO.filter((schema: { schema_name: string; }) =>
        schema.schema_name.toLowerCase().startsWith(searchText)
      );
    } else {
      this.filteredSchemas = [];
    }
  }

  suggestTable(inputText: any) {
    const query = inputText.target.value.toLowerCase();
    this.prq = inputText.target.value;
    const dotIndex = query.lastIndexOf('.');
    if (dotIndex !== -1) {
      const searchText = query.substring(dotIndex + 1);
      this.filteredTable = this.tableDTO.filter((schema: { table_name: string; }) =>
        schema.table_name.toLowerCase().startsWith(searchText)
      );
    } else {
      this.filteredTable = [];
    }
  }


  selectSchema(schemaName: string) {
    var currentValue;
    if (this.prq && this.prq?.lastIndexOf('.') > this.prq?.lastIndexOf('from ')) {
      currentValue = this.prq?.substring(0, this.prq?.lastIndexOf('.') + 1) + this.dynamicquery.controls['query'].value;
    }
    else {
      currentValue = this.prq?.substring(0, this.prq?.lastIndexOf('from ') + 5) + this.dynamicquery.controls['query'].value;
      this.getTable(schemaName);
    }
    this.dynamicquery.get('query')!.patchValue(currentValue);
    this.filteredSchemas = [];
    this.filteredTable = [];
  }


  OnSubmit() {
    this.resultMessage = '';
    this.dataSource.data = [];
    this.displayedColumns = [];
    this.submit = true;
    this.datalength = 0;
    if (this.dynamicquery.valid) {
      this.masterService.GetQuery(this.dynamicquery.value)
        .subscribe(d => {
          if (d.responseCode === 440) {
            Swal.fire({
              title: d.message,
              text: '',
              icon: 'error',
              showCancelButton: false,
              confirmButtonText: 'LogIn',
            });
            this.logout();
          } else {
            this.QS = d.isSuccess;
            if (d.isSuccess) {
              this.resultMessage = d.message;
              this.dataSource.data = d.data;
              this.flgclo = true;
              this.datalength = d.dataLength;
              this.displayedColumns = Object.keys(d.data[0]);
            } else {
              this.resultMessage = d.message || 'Failed to retrieve data';
            }
          }
          this.submit = false;
        }, error => {
          this.resultMessage = 'An error occurred';
          this.submit = false;
        });
    } else {
      this.flgclo = false;
      this.submit = false;
    }
  }

  downloadData(format: string) {
    const data = this.dataSource.data;
    const fileName = 'grid-data';

    if (format === 'excel') {
      const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);
      const wb: XLSX.WorkBook = { Sheets: { 'data': ws }, SheetNames: ['data'] };
      XLSX.writeFile(wb, `${fileName}.xlsx`);
    } else if (format === 'pdf') {
      const doc = new jsPDF();
      const col = this.displayedColumns;
      const rows = data.map((row: any) => this.displayedColumns.map(column => row[column]));

      let startY = 10;
      doc.setFontSize(10);

      col.forEach((header, index) => {
        doc.text(header, 10 + index * 30, startY);
      });

      startY += 10;

      rows.forEach((row, rowIndex) => {
        row.forEach((cell, index) => {
          const text = cell?.toString() || '';
          doc.text(text, 10 + index * 30, startY);
        });
        startY += 10;
      });

      doc.save(`${fileName}.pdf`);
    }
  }

  logout() {
    this.masterService.logout();
  }

  reset(dropdownValue: any) {
    dropdownValue.value = '';
    this.dynamicquery.reset();
    this.resultMessage = '';
    this.dataSource.data = [];
    this.displayedColumns = [];
    this.filteredSchemas = [];
    this.submit = false;
    this.ngOnInit();
  }
}