import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { PersonService, Person } from '../../services/person';

@Component({
  selector: 'person-table',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule
  ],
  templateUrl: './person-table.html',
  styleUrls: ['./person-table.scss']
})
export class PersonTable implements OnInit, AfterViewInit {
  displayedColumns = ['name', 'currentRank', 'currentDutyTitle', 'careerStartDate', 'careerEndDate'];
  dataSource = new MatTableDataSource<Person>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private personService: PersonService) {}

  ngOnInit() {
    this.personService.getAllPeople().subscribe({
      next: (result: any) => {
        this.dataSource.data = result.people;
      },
      error: (err) => console.error('Error fetching people:', err)
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
}
