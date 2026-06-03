import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Expense, ExpenseCreate } from '../shared/interfaces/expense.interface';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  private readonly apiUrl = '/api/expense';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Expense[]> {
    return this.http.get<Expense[]>(this.apiUrl);
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
