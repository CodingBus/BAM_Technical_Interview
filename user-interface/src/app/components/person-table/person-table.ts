import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { PersonService, Person } from '../../services/person';

@Component({
  selector: 'person-table',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  templateUrl: './person-table.html',
  styleUrls: ['./person-table.scss']
})
export class PersonTable implements OnInit {
  displayedColumns = ['name', 'currentRank', 'currentDutyTitle', 'careerStartDate', 'careerEndDate'];
  dataSource = new MatTableDataSource<Person>();

  constructor(private personService: PersonService) {}

  ngOnInit() {
    this.personService.getAllPeople().subscribe({
      next: (result: any) => {
        console.log('People from service:', result.people);
        this.dataSource.data = result.people;
      },
      error: (err) => console.error('Error fetching people:', err)
    });
  }
}
