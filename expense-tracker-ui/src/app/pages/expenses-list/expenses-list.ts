import { Component, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { Expense } from '../../shared/interfaces/expense';

type SortField = 'expenseDate' | 'categoryName' | 'amount';
type BackendSortField = 'date' | 'category' | 'amount';

@Component({
  selector: 'app-expenses-list',
  standalone: true,
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './expenses-list.html',
  styleUrl: './expenses-list.css',
})
export class ExpensesListComponent implements OnInit {
  readonly expenses = signal<Expense[]>([]);
  readonly sortField = signal<SortField>('expenseDate');
  readonly sortDirection = signal<'asc' | 'desc'>('desc');
  readonly isLoading = signal(false);

  constructor(
    private expenseService: ExpenseService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.load();
  }

  private mapToBackendField(field: SortField): BackendSortField {
    const fieldMap: Record<SortField, BackendSortField> = {
      expenseDate: 'date',
      categoryName: 'category',
      amount: 'amount',
    };
    return fieldMap[field];
  }

  load(): void {
    this.isLoading.set(true);
    const backendField = this.mapToBackendField(this.sortField());
    this.expenseService.getAll(backendField, this.sortDirection()).subscribe((data) => {
      this.expenses.set(data);
      this.isLoading.set(false);
    });
  }

  toggleSort(field: SortField): void {
    if (this.sortField() === field) {
      this.sortDirection.update((d) => (d === 'asc' ? 'desc' : 'asc'));
    } else {
      this.sortField.set(field);
      this.sortDirection.set(field === 'expenseDate' ? 'desc' : 'asc');
    }
    this.load();
  }

  sortLabel(field: SortField): string {
    const labels: Record<SortField, string> = {
      expenseDate: 'Date',
      categoryName: 'Category',
      amount: 'Amount',
    };
    const arrow = this.sortField() === field ? (this.sortDirection() === 'asc' ? ' ↑' : ' ↓') : '';
    return labels[field] + arrow;
  }

  isSortActive(field: SortField): boolean {
    return this.sortField() === field;
  }

  edit(id: number): void {
    this.router.navigate(['/edit', id]);
  }

  delete(id: number): void {
    this.expenseService.delete(id).subscribe(() => {
      this.load();
    });
  }

  goToAdd(): void {
    this.router.navigate(['/add']);
  }

  goToCategories(): void {
    this.router.navigate(['/categories']);
  }
}
