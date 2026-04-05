import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MasterService } from '../services/master.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {
  @ViewChild('profileModal') profileModal: any;
  @ViewChild('mymodal') mymodal: any;

  userDetail: any;
  isMenuOpen = false;
  closeResult?: string;

  constructor(
    private router: Router,
    private modalService: NgbModal,
    private masterService: MasterService
  ) { }

  ngOnInit(): void {
    var sessionvalue: any = sessionStorage.getItem('user');
    if (sessionvalue == null) {
      return this.logout();
    }
    this.userDetail = JSON.parse(decodeURIComponent(escape(atob(sessionvalue))));
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  openScript() {
    this.isMenuOpen = false;
    this.router.navigate(['./ScriptRead']);
  }

  getTransaction(option: string) {
    this.isMenuOpen = false;
    // Navigate to QueryExecuter with transaction type as query parameter
    this.router.navigate(['./QueryExecuter'], { queryParams: { transType: option } });
  }

  open(modalName: any) {
    this.isMenuOpen = false;
    let content: any;
    if (modalName === 'mymodal') {
      content = this.mymodal;
    } else if (modalName === 'profileModal') {
      content = this.profileModal;
    }
    if (content) {
      this.modalService.open(content, {
        ariaLabelledBy: 'modal-basic-title',
        size: 'lg',
        scrollable: true,
        windowClass: 'showtable',
        backdrop: 'static',
        keyboard: false
      }).result.then((result) => {
        this.closeResult = `Closed with: ${result}`;
      }, (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });
    }
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else {
      return `with: ${reason}`;
    }
  }

  logout() {
    this.modalService.dismissAll();
    this.masterService.logout();
  }
}
