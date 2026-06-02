import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExpenseService, Category } from '../../services/expense';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './expense-form.html',
  styleUrl: './expense-form.css'
})
export class ExpenseFormComponent implements OnInit {
  categories: Category[] = [];
  form = {
    title: '',
    amount: 0,
    expenseDate: '',
    categoryId: 0
  };

  constructor(private expenseService: ExpenseService, private router: Router) {}

  ngOnInit(): void {
    this.expenseService.getCategories().subscribe(data => {
      this.categories = data;
    });
  }

  save() {
    this.expenseService.create(this.form).subscribe(() => {
      this.router.navigate(['/']);
    });
  }

  cancel() {
    this.router.navigate(['/']);
  }
}
