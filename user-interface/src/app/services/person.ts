import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Person {
  id: number;
  name: string;
  success?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  private apiUrl = 'https://localhost:7204';

  constructor(private http: HttpClient) { }

  createPerson(name: string): Observable<Person> {
    return this.http.post<Person>(`${this.apiUrl}/Person`, JSON.stringify(name), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  updatePerson(name: string, newName: string): Observable<Person> {
    return this.http.put<Person>(`${this.apiUrl}/Person/${name}`, JSON.stringify(newName), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  getPersonByName(name: string): Observable<Person> {
    return this.http.get<Person>(`${this.apiUrl}/Person/${ name }`)
  }

  getAllPeople(): Observable<Person[]> {
    return this.http.get<Person[]>(`${this.apiUrl}/Person`)
  }
}
