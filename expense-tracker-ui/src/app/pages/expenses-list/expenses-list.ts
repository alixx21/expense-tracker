import { Component, OnInit, computed, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { Expense } from '../../shared/interfaces/expense.interface';

type SortField = 'expenseDate' | 'categoryName' | 'amount';

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

  readonly sortedExpenses = computed(() => {
    const field = this.sortField();
    const direction = this.sortDirection();
    const list = [...this.expenses()];

    list.sort((a, b) => {
      let cmp = 0;
      if (field === 'amount') {
        cmp = a.amount - b.amount;
      } else if (field === 'expenseDate') {
        cmp = new Date(a.expenseDate).getTime() - new Date(b.expenseDate).getTime();
      } else {
        cmp = a.categoryName.localeCompare(b.categoryName, undefined, { sensitivity: 'base' });
      }
      return direction === 'asc' ? cmp : -cmp;
    });

    return list;
  });

  constructor(
    private expenseService: ExpenseService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.expenseService.getAll().subscribe((data) => {
      this.expenses.set(data);
    });
  }

  toggleSort(field: SortField): void {
    if (this.sortField() === field) {
      this.sortDirection.update((d) => (d === 'asc' ? 'desc' : 'asc'));
    } else {
      this.sortField.set(field);
      this.sortDirection.set(field === 'expenseDate' ? 'desc' : 'asc');
    }
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
