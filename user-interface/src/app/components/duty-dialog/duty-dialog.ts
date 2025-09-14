import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Person } from '../../services/person';
import { DutyTitleEnum } from '../../enums/duty-title.enum';
import { RankEnum } from '../../enums/rank.enum';
import { getEnumName } from '../../enums/enum-helper';

@Component({
  selector: 'app-duty-dialog',
  imports: [],
  templateUrl: './duty-dialog.html',
  styleUrl: './duty-dialog.scss'
})
export class DutyDialog implements OnInit{
   constructor(private dialogRef: MatDialogRef<DutyDialog>, @Inject(MAT_DIALOG_DATA) public data: { person: Person }) {}

   ngOnInit(): void {
     console.log("dialog says:", this.mapPersonToDisplay(this.data.person));
   }

   onCancel(): void {
    this.dialogRef.close("cancel");
  }

  onConfirm(): void {
    this.dialogRef.close("save");
  }

  mapPersonToDisplay(person: Person){
    let duty_list = new Array();
    person.astronautDuties.forEach(duty => {
      duty_list.push({
        name : person.name,
        rank : getEnumName(RankEnum, duty.rankId),
        dutyTitle: getEnumName(DutyTitleEnum, duty.dutyTitleId),
        dutyStartDate: duty.dutyStartDate,
        dutyEndDate: duty.dutyEndDate
      });
    });
    return duty_list;
  }
}
