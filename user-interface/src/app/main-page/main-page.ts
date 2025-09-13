import { Component, OnInit } from '@angular/core';
import { PersonService, Person } from '../services/person';
import { AstronautDutyResponse, AstronautDutyService, CreateAstronautDutyRequest } from '../services/astronaut-duty';

@Component({
  selector: 'app-main-page',
  imports: [],
  templateUrl: './main-page.html',
  styleUrl: './main-page.scss'
})
export class MainPage implements OnInit{
  constructor(private personService: PersonService, private astronautDutyService: AstronautDutyService) {}

  public ngOnInit(): void {
    console.log("oninit");

    // this.personService.updatePerson("devin", "devin2").subscribe({
    //   next: (result: Person) => {
    //     console.log('Updated person:', result);
    //   },
    //   error: (err) => {
    //     console.error('Error creating person:', err);
    //   }
    // });

    // this.astronautDutyService.createAstronautDuty({
    //     name: 'derek',
    //     rank: 3,
    //     dutyTitle: 1,
    //     dutyStartDate: new Date().toISOString()
    //   }).subscribe({
    //   next: (result: AstronautDutyResponse) => {
    //     console.log(result);
    //   },
    //   error: (err) => {
    //     console.error('Error retrieving AstronautDuty', err);
    //   }
    // });

  }
}
