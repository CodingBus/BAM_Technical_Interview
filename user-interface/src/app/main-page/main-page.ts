import { Component, OnInit } from '@angular/core';
import { PersonService, Person } from '../services/person';

@Component({
  selector: 'app-main-page',
  imports: [],
  templateUrl: './main-page.html',
  styleUrl: './main-page.scss'
})
export class MainPage implements OnInit{
  constructor(private personService: PersonService) {}

  public ngOnInit(): void {
    console.log("oninit");

    this.personService.updatePerson("devin", "devin2").subscribe({
      next: (result: Person) => {
        console.log('Updated person:', result);
      },
      error: (err) => {
        console.error('Error creating person:', err);
      }
    });
  }
}
