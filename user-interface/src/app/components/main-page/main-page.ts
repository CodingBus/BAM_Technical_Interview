import { Component, OnInit } from '@angular/core';
import { PersonService, Person } from '../../services/person';
import { AstronautDutyResponse, AstronautDutyService, CreateAstronautDutyRequest } from '../../services/astronaut-duty';
import { PersonTable } from '../person-table/person-table';

@Component({
  selector: 'app-main-page',
  imports: [PersonTable],
  templateUrl: './main-page.html',
  styleUrl: './main-page.scss'
})
export class MainPage implements OnInit{
  constructor(private personService: PersonService, private astronautDutyService: AstronautDutyService) {}

  public ngOnInit(): void {}
}
