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
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MAT_NATIVE_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';


@Component({
  selector: 'app-duty-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule    
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-US' },
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MAT_NATIVE_DATE_FORMATS }
  ],
  templateUrl: './duty-dialog.html',
  styleUrl: './duty-dialog.scss'
})
export class DutyDialog implements OnInit {
  astronautDuties = new Array<AstronautDuty>();
  displayedColumns = ['rank', 'dutyTitle', 'dutyStartDate', 'dutyEndDate'];
  dataSource = new MatTableDataSource<AstronautDuty>();
  dutyForm!: FormGroup;

  getEnumName = getEnumName;
  RankEnum = RankEnum;
  DutyTitleEnum = DutyTitleEnum;


  ranks = Object.values(RankEnum).filter(v => typeof v === 'number') as RankEnum[];
  dutyTitles = Object.values(DutyTitleEnum).filter(v => typeof v === 'number') as DutyTitleEnum[];

  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<DutyDialog>, @Inject(MAT_DIALOG_DATA) public data: { person: Person }) { }

  ngOnInit(): void {
    this.dutyForm = this.fb.group({
      rank: [ RankEnum, Validators.required],
      dutyTitle: [DutyTitleEnum, Validators.required],
      dutyStartDate: ['', Validators.required]
    });

    console.log("dialog says:", this.mapPersonToDisplay(this.data.person));
    this.dataSource.data = this.mapPersonToDisplay(this.data.person);
  }

  onAddDuty(): void {
    if (this.dutyForm.valid) {
      const astronautDuty = new AstronautDuty();
      astronautDuty.name = this.data.person.name;
      astronautDuty.rank = getEnumName(RankEnum, this.dutyForm.value.rank);
      astronautDuty.dutyTitle = getEnumName(DutyTitleEnum, this.dutyForm.value.dutyTitle);
      astronautDuty.dutyStartDate = this.dutyForm.value.dutyStartDate;

      this.dataSource.data = [...this.dataSource.data, astronautDuty];

      this.astronautDuties.push(astronautDuty);

      this.dutyForm.reset();
    }
  }


  onCancel(): void {
    this.dialogRef.close({status: "cancel", newDuties: []});
  }

  onConfirm(): void {
    this.dialogRef.close({status: "save", newDuties: this.astronautDuties});
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
