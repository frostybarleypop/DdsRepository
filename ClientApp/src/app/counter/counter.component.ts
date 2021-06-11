import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public currentCount = 0;
  public results: any;
  public user: string;
  public pass: string;
  public firstname: string;
  public lastname: string;
  public custId: number;
  public token: string;
  private url;
  private myHttp: HttpClient;
  public visits: Visit[];
  public newNote: string;
  public newVisitDate: Date;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  this.myHttp = http;
    this.url = baseUrl;
    this.visits = [];
  }

  public incrementCounter() {
    this.currentCount++;
  }

  public GetPatient() {
    
    const options = {
      headers: new HttpHeaders({ 'Authorization': this.token, 'Content-Type': 'application/json' }),
      // responseType:  json
    };
    
    this.myHttp.get<PatientRecord>(this.url + 'api/Patient/'+ this.custId, options).subscribe(result => {
     console.log('data result', result);
      this.results = JSON.stringify(result);
      this.firstname = result.firstName;
      this.lastname = result.lastName;
      this.visits = result.visits;
    }, error => {
      console.error('data error', error);
        this.results = JSON.stringify(error);
    });
  
  }

  public CreatePatient() {

    const options = {
      headers: new HttpHeaders({ 'Authorization': this.token, 'Content-Type': 'application/json' }),
      // responseType:  json
    };
    let patient: PatientRecord = { id: 0, firstName: this.firstname, lastName: this.lastname, imageUrl: null, visits: this.visits };

    this.myHttp.post<PatientRecord>(this.url + 'api/Patient', patient, options).subscribe(result => {
      // console.log('data result', result);
      this.results = JSON.stringify(result);
      this.firstname = '';
      this.lastname = '';
    }, error => {
      console.error('data error', error);
      this.results = JSON.stringify(error);
    });

  }

  public UpdatePatient() {

    const options = {
      headers: new HttpHeaders({ 'Authorization': this.token, 'Content-Type': 'application/json' }),
      // responseType:  json
    };
    let patient: PatientRecord = {id :this.custId, firstName:this.firstname, lastName:this.lastname, imageUrl:null, visits:this.visits };

    this.myHttp.put<PatientRecord>(this.url + 'api/Patient/' + this.custId, patient, options).subscribe(result => {
      // console.log('data result', result);
      this.results = JSON.stringify(result);
      this.firstname = '';
      this.lastname = '';
    }, error => {
      console.error('data error', error);
      this.results = JSON.stringify(error);
    });
  }

  public DeletePatient() {
    const options = {
      headers: new HttpHeaders({ 'Authorization': this.token, 'Content-Type': 'application/json' }),
      // responseType:  json
    };

    this.myHttp.delete<number>(this.url + 'api/Patient/' + this.custId, options).subscribe(result => {
      // console.log('data result', result);
      this.results = JSON.stringify(result);
      this.firstname = '';
      this.lastname = '';
      this.visits = [];
    }, error => {
      console.error('data error', error);
      this.results = JSON.stringify(error);
    });
  }

  public login() {
    const opts = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }) //,
      // responseType: 'text'
    };
    this.myHttp.post<DdsAuth>(this.url + 'api/Authenticate', { 'username': this.user, 'password': this.pass }, opts)
      .toPromise()
      .then(res => {
        this.token = res.value;

      }).catch(err => {
        console.log('post error', err);
        this.results = JSON.stringify(err);
      });
  }

  public addVisit() {
    if (!isNullOrUndefined(this.newNote) && !isNullOrUndefined(this.newVisitDate)) {
      let visit: Visit = { notes: this.newNote, visitDate: this.newVisitDate, id: 0, customerId: this.custId };
     
      this.visits.push(visit);
    }
  }
}


//interfaces
interface PatientRecord {
  id: number;
  firstName: string;
  lastName: string;
  imageUrl: string;
  visits: Visit[];
}

interface Visit {
  id: number;
  visitDate: Date;
  notes: string;
  customerId: number;
}

interface DdsAuth {
  value: string;
}
