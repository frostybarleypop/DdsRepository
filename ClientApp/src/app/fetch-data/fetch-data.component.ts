import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
//import 'rxjs/add/operator/toPromise';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public records: PatientRecord[];
  public unauthenticated: boolean;
  public user: string;
  public pass: string;
  private url;
  private token: any;
  private myHttp: HttpClient;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.myHttp = http;
    this.url = baseUrl;
    this.unauthenticated = true;
  }

  ngoninit() {


  }

  public async loginUser() {
    console.log('button clicked');
    const headers: HttpHeaders = new HttpHeaders();
    headers.set('Content-Type', 'application/json');
    const opts = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }) ,
      responseType: 'text'
    };
    console.log({ 'username': this.user, 'password': this.pass });
    //this.myHttp.post<string>(this.url + 'api/Authenticate', { 'username': this.user, 'password': this.pass }, opts)
    //  .subscribe(data => this.token = data);
    //console.log('token:', this.token);
    //this.getData();

     //tslint:disable-next-line:max-line-length
    this.myHttp.post<string>(this.url + 'api/Authenticate', { 'username': this.user, 'password': this.pass }, opts)
      .toPromise()
      .then(res => {
       
      console.log('inside promise result', res);
        this.token = res;
      console.log('token:', this.token);
      this.getData();
     }).catch(err => console.log('post error',err));
    

  }

  public getData() {
    console.log('in getdata', this.token);
    const dataHeaders: HttpHeaders = new HttpHeaders({ 'Authorization': this.token });
    const opts1 = {
      headers: new HttpHeaders({ 'Authorization': this.token , 'Content-Type': 'application/json' }),
         // responseType:  json
    };
    console.log('data headers', dataHeaders.get('Authorization'));

    this.myHttp.get<PatientRecord[]>(this.url + 'api/Patient', opts1 ).subscribe(result => {
      console.log('data result', result);
      this.records = result;
      this.unauthenticated = false;
    }, error => {
      console.error('data error', error);
        this.unauthenticated = error.statusText === 'Unauthorized';

    });
  }
}


interface PatientRecord {
  id: number;
  firstName: number;
  lastName: number;
  imageUrl: string;
  visits: Visits[];
}

interface Visits {
  visitDate: Date;
  notes: string;
}
