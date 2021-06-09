import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public records: PatientRecord[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<PatientRecord[]>(baseUrl + 'api/Patient').subscribe(result => {
      this.records = result;

    }, error => console.error(error));

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
