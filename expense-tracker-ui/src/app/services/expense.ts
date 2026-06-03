import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Expense, ExpenseCreate } from '../shared/interfaces/expense';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  private readonly apiUrl = '/api/expense';

  constructor(private http: HttpClient) {}

  getAll(sortBy?: string, sortDirection?: string): Observable<Expense[]> {
    let params = new HttpParams();
    if (sortBy) params = params.set('sortBy', sortBy);
    if (sortDirection) params = params.set('sortDirection', sortDirection);
    return this.http.get<Expense[]>(this.apiUrl, { params });
  }

  getById(id: number): Observable<Expense> {
    return this.http.get<Expense>(`${this.apiUrl}/${id}`);
  }

  create(data: ExpenseCreate): Observable<Expense> {
    return this.http.post<Expense>(this.apiUrl, data);
  }

  update(id: number, data: ExpenseCreate): Observable<Expense> {
    return this.http.put<Expense>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
