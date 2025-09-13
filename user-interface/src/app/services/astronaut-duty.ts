import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Duty {
  id: number;
  personId: number;
  rankId: number;
  rank: string | null;
  dutyTitleId: number;
  dutyTitle: string | null;
  dutyStartDate: string;
  dutyEndDate: string | null;
  person: any | null;
}

export interface PersonWithDuties {
  personId: number;
  name: string;
  currentRank: string;
  currentDutyTitle: string;
  careerStartDate: string;
  careerEndDate: string | null;
  astronautDuties: Duty[];
}

export interface AstronautDutyResponse {
  person: PersonWithDuties;
  success: boolean;
  message: string;
  responseCode: number;
}

export interface CreateAstronautDutyRequest {
  name: string;
  rank: number;
  dutyTitle: number;
  dutyStartDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class AstronautDutyService {

  private apiUrl = 'https://localhost:7204';

  constructor(private http: HttpClient) { }

  getAstronautDutyByName(name: string): Observable<AstronautDutyResponse> {
    return this.http.get<AstronautDutyResponse>(`${this.apiUrl}/AstronautDuty/${name}`);
  }

  createAstronautDuty(request: CreateAstronautDutyRequest): Observable<AstronautDutyResponse> {
    return this.http.post<AstronautDutyResponse>(`${this.apiUrl}/AstronautDuty`, request);
  }
}
