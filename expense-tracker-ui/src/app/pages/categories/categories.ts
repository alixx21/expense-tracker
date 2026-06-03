import { Component, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../services/category';
import { Category } from '../../shared/interfaces/category';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
})
export class CategoriesComponent implements OnInit {
  readonly categories = signal<Category[]>([]);
  readonly editingId = signal<number | null>(null);
  newCategoryName = '';
  editName = '';

  constructor(
    private categoryService: CategoryService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.categoryService.getAll().subscribe((data) => {
      this.categories.set(data);
    });
  }

  add(): void {
    const name = this.newCategoryName.trim();
    if (!name) return;

    this.categoryService.create(name).subscribe(() => {
      this.newCategoryName = '';
      this.load();
    });
  }

  startEdit(category: Category): void {
    this.editingId.set(category.id);
    this.editName = category.name;
  }

  cancelEdit(): void {
    this.editingId.set(null);
    this.editName = '';
  }

  saveEdit(id: number): void {
    const name = this.editName.trim();
    if (!name) return;

    this.categoryService.update(id, name).subscribe(() => {
      this.cancelEdit();
      this.load();
    });
  }

  delete(id: number): void {
    this.categoryService.delete(id).subscribe(() => {
      if (this.editingId() === id) {
        this.cancelEdit();
      }
      this.load();
    });
  }

  back(): void {
    this.router.navigate(['/']);
  }
}
