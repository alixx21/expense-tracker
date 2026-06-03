import { Component, OnInit, computed, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ExpenseService } from '../../services/expense';
import { CategoryService } from '../../services/category';
import { Category } from '../../shared/interfaces/category.interface';
import { Expense, ExpenseCreate } from '../../shared/interfaces/expense.interface';

interface ExpenseFormState {
  title: string;
  amount: number | null;
  expenseDate: string;
  categoryId: number;
}

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './expense-form.html',
  styleUrl: './expense-form.css',
})
export class ExpenseFormComponent implements OnInit {
  readonly categories = signal<Category[]>([]);
  readonly expenseId = signal<number | null>(null);
  readonly isEdit = computed(() => this.expenseId() !== null);
  /** Original expense — used for placeholders and fallback on save */
  readonly original = signal<Expense | null>(null);

  readonly form = signal<ExpenseFormState>({
    title: '',
    amount: null,
    expenseDate: '',
    categoryId: 0,
  });

  constructor(
    private expenseService: ExpenseService,
    private categoryService: CategoryService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.expenseId.set(Number(idParam));
    }

    this.categoryService.getAll().subscribe((data) => {
      this.categories.set(data);
      if (!this.isEdit() && data.length > 0 && this.form().categoryId === 0) {
        this.updateField('categoryId', data[0].id);
      }
    });

    const id = this.expenseId();
    if (id !== null) {
      this.expenseService.getById(id).subscribe((expense) => {
        this.original.set(expense);
        this.form.set({
          title: expense.title,
          amount: expense.amount,
          expenseDate: expense.expenseDate.slice(0, 10),
          categoryId: expense.categoryId,
        });
      });
    }
  }

  updateField<K extends keyof ExpenseFormState>(key: K, value: ExpenseFormState[K]): void {
    this.form.update((f) => ({ ...f, [key]: value }));
  }

  placeholderTitle(): string {
    return this.original()?.title ?? '';
  }

  placeholderAmount(): string {
    const amount = this.original()?.amount;
    return amount != null ? String(amount) : '';
  }

  placeholderDate(): string {
    const date = this.original()?.expenseDate;
    return date ? date.slice(0, 10) : '';
  }

  placeholderCategory(): string {
    return this.original()?.categoryName ?? '';
  }

  private buildPayload(): ExpenseCreate {
    const f = this.form();
    const o = this.original();

    if (this.isEdit() && o) {
      return {
        title: f.title.trim() || o.title,
        amount: f.amount ?? o.amount,
        expenseDate: f.expenseDate || o.expenseDate.slice(0, 10),
        categoryId: f.categoryId || o.categoryId,
      };
    }

    return {
      title: f.title,
      amount: f.amount ?? 0,
      expenseDate: f.expenseDate,
      categoryId: f.categoryId,
    };
  }

  save(): void {
    const id = this.expenseId();
    const payload = this.buildPayload();
    const request =
      id !== null
        ? this.expenseService.update(id, payload)
        : this.expenseService.create(payload);

    request.subscribe(() => {
      this.router.navigate(['/']);
    });
  }

  cancel(): void {
    this.router.navigate(['/']);
  }
}
