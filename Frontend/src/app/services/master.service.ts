import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule, HttpHeaders, HttpParams } from '@angular/common/http';

import { environment } from 'src/environments/environment';
import { DatabseRequest } from '../model/databse-request';
import { LoginRequest } from '../model/login-request';
import { TableRequest } from '../model/table-request';
import { QueryRequest } from '../model/query-request';
import { SchemaRequest } from '../model/schema-request';
import { ResultRequest } from '../model/result-request';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { UserInfo } from '../model/user-info';
import { catchError, map, timeout } from 'rxjs/operators';
import { Router } from '@angular/router';

import { EncryptionService } from './encryption.service';
import { AddUserRequest } from '../model/add-user-request';
import { TransactionRequest } from '../model/transaction-request';
import { DesignationRequest } from '../model/designation-request';
import { FunctioViewRequest } from '../model/functio-view-request';
import { SriptRequest } from '../model/sript-request';


@Injectable({
  providedIn: 'root'
})
export class MasterService {
  private userSubject?: BehaviorSubject<UserInfo>;
  public user?: Observable<UserInfo>;
  public userDetail: any;


  constructor(
    private http: HttpClient,
    private router: Router,
    private encryptionService: EncryptionService
  ) { }



  GetDataBase(databseRequest: DatabseRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Home/GetDataBase`, { Data: this.encryptionService.encryptData(databseRequest) }, { withCredentials: true });
  }
  GetSchemas(schemaRequest: SchemaRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Home/GetSchemas`, { Data: this.encryptionService.encryptData(schemaRequest) }, { withCredentials: true });
  }
  GetTable(tableRequest: TableRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Home/GetTable`, { Data: this.encryptionService.encryptData(tableRequest) }, { withCredentials: true });
  }
  GetQuery(queryRequest: QueryRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Home/GetQuery`, { Data: this.encryptionService.encryptData(queryRequest) }, { withCredentials: true });
  }
  GetTransaction(transactionRequest: TransactionRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Home/GetTransaction`, { Data: this.encryptionService.encryptData(transactionRequest) }, { withCredentials: true });
  }
  AddUser(addUserRequest: AddUserRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Login/InsertUser`, { Data: this.encryptionService.encryptData(addUserRequest) }, { withCredentials: true });
  }

  GetDesignation(designationRequest: DesignationRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Master/GetDesignation`, { Data: this.encryptionService.encryptData(designationRequest) }, { withCredentials: true });
  }

  GetFunctionViewList(functioViewRequest: FunctioViewRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Home/GetFunctionsOrViewsList`, { Data: this.encryptionService.encryptData(functioViewRequest) }, { withCredentials: true });
  }

  GetScript(sriptRequest: SriptRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Home/GetScript`, { Data: this.encryptionService.encryptData(sriptRequest) }, { withCredentials: true });
  }

  GetLogin(loginRequest: LoginRequest) {
    return this.http.post<ResultRequest>(`${environment.apiUrl}/Login/Login`, { Data: this.encryptionService.encryptData(loginRequest) }, { withCredentials: true })
      .pipe(map(user => {
        this.userDetail = user.data
        if (user.data != null) {
          var j = JSON.stringify(user);
          var enco = btoa(unescape(encodeURIComponent(j)));
          sessionStorage.setItem('user', enco);
          return user;
        }
        else {
          return user;
        }
      })
      );
  }
  logout(): Promise<void> {
    return new Promise((resolve, reject) => {
      try {
        sessionStorage.removeItem('user');
        resolve();
      } catch (ex) {
        console.error('Error while removing user from sessionStorage:', ex);
        reject(ex);
      }
      finally {
        this.http.post(`${environment.apiUrl}/Login/logout`, {}, { withCredentials: true })
          .subscribe(() => {
            this.router.navigate(['./']);
          },
        error=>{
          this.router.navigate(['./']);
        });
      }
    });
  }
}
