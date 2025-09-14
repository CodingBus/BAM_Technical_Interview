import { Component, Inject, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AstronautDuty, Person } from '../../services/person';
import { DutyTitleEnum } from '../../enums/duty-title.enum';
import { RankEnum } from '../../enums/rank.enum';
import { getEnumName } from '../../enums/enum-helper';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-duty-dialog',
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatIconModule],
  templateUrl: './duty-dialog.html',
  styleUrl: './duty-dialog.scss'
})
export class DutyDialog implements OnInit, AfterViewInit {
  displayedColumns = ['rank', 'dutyTitle', 'dutyStartDate', 'dutyEndDate'];
  dataSource = new MatTableDataSource<AstronautDuty>();

  constructor(private dialogRef: MatDialogRef<DutyDialog>, @Inject(MAT_DIALOG_DATA) public data: { person: Person }) { }

  ngOnInit(): void {
    console.log("dialog says:", this.mapPersonToDisplay(this.data.person));
    this.dataSource.data = this.mapPersonToDisplay(this.data.person);
  }

  ngAfterViewInit(): void {

  }

  onCancel(): void {
    this.dialogRef.close("cancel");
  }

  onConfirm(): void {
    this.dialogRef.close("save");
  }

  mapPersonToDisplay(person: Person): AstronautDuty[] {
    const duty_list: AstronautDuty[] = [];

    person.astronautDuties.forEach(duty => {
      const astronautDuty = new AstronautDuty();
      astronautDuty.name = person.name;
      astronautDuty.rank = getEnumName(RankEnum, duty.rankId);
      astronautDuty.dutyTitle = getEnumName(DutyTitleEnum, duty.dutyTitleId);
      astronautDuty.dutyStartDate = duty.dutyStartDate;
      astronautDuty.dutyEndDate = duty.dutyEndDate;

      duty_list.push(astronautDuty);
    });

    return duty_list;
  }

}
