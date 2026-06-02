import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { ExpenseService, Expense } from '../../services/expense';
 
@Component({
  selector: 'app-expenses-list',
  standalone: true,
  imports: [],
  templateUrl: './expenses-list.html',
  styleUrl: './expenses-list.css'
})
export class ExpensesListComponent implements OnInit {
  expenses: Expense[] = [];
 
  constructor(
    private expenseService: ExpenseService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}
 
  ngOnInit(): void {
    this.load();
  }
 
  load() {
    this.expenseService.getAll().subscribe({
      next: (data) => {
        this.expenses = [...data];
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading expenses:', err);
      }
    });
  }
 
  delete(id: number) {
    this.expenseService.delete(id).subscribe(() => {
      this.load();
    });
  }
 
  goToAdd() {
    this.router.navigate(['/add']);
  }
}