import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
 
export interface Expense {
  id: number;
  title: string;
  amount: number;
  expenseDate: string;
  categoryName: string;
}
 
export interface Category {
  id: number;
  name: string;
}
 
@Injectable({ providedIn: 'root' })
export class ExpenseService {
  private apiUrl = 'http://localhost:5018/api/expense';
  private categoryUrl = 'http://localhost:5018/api/category';
 
  constructor(private http: HttpClient) {}
 
  getAll(): Observable<Expense[]> {
    return this.http.get<Expense[]>(this.apiUrl);
  }
 
  create(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }
 
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
 
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.categoryUrl);
  }
}