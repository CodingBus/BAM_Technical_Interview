import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { PersonService, Person } from '../../services/person';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DutyDialog } from '../duty-dialog/duty-dialog';
import { AstronautDutyService } from '../../services/astronaut-duty';

@Component({
  selector: 'person-table',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './person-table.html',
  styleUrls: ['./person-table.scss']
})
export class PersonTable implements OnInit, AfterViewInit {
  displayedColumns = ['name', 'currentRank', 'currentDutyTitle', 'careerStartDate', 'careerEndDate'];
  dataSource = new MatTableDataSource<Person>();

  editingPersonId: number | null = null;
  editingPersonName: string | null = null;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private personService: PersonService, private dialog: MatDialog, private astronautDutyService: AstronautDutyService) {}

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

  startEditing(person: Person) {
    this.editingPersonName = person.name;
    this.editingPersonId = person.personId;
  }

  saveName(person: Person, newName: string) {
    console.log(this.editingPersonName, newName);
    this.personService.updatePerson(this.editingPersonName!, newName).subscribe({
      next: (result: any) => {
        person.name = newName;
        this.editingPersonId = null;
      },
      error: (err) =>{
        person.name = this.editingPersonName!;
        this.editingPersonName = null;
        this.editingPersonId = null;
      }
    })
  }

  cancelEdit(person: Person) {
    person.name = this.editingPersonName!;
    this.editingPersonName = null;
    this.editingPersonId = null;
  }

  openDutyDialog(person: Person){
    const dialogRef = this.dialog.open(DutyDialog, {
      // width: '400px',
      disableClose: true,
      data: { person: person }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.status == "save") {
        //update duty
        this.astronautDutyService.createAstronautDuty({
          name: result.newDuty.name,
          rank: result.newDuty.rankId,
          dutyTitle: result.newDuty.dutyTitleId,
          dutyStartDate: result.newDuty.dutyStartDate
        }).subscribe({
          next: (result: any) => {
            // refresh table
            this.personService.getAllPeople().subscribe({
              next: (result: any) => {
                console.log("new duty", result.newDuty);
                this.dataSource.data = result.people;
              },
              error: (err) => console.error('Error fetching people:', err)
            });
          },
          error: (err) => console.error("Error creating astronaut duty:", err)
        })
      }
      console.log('Dialog closed with:', result);
    });
  }
}
