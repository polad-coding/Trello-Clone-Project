import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  constructor(private http: HttpClient) {

  }

  public printSomeStuff() {
    console.log('printing');
  }

  public TestApplication() {
    this.http.get(`https://localhost:44336/api/test/Test`, { observe: 'response' }).subscribe(result => {
      console.log(result);
    },
      error => {
        console.log('error');
      });
  }
}
