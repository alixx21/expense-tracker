import { Routes } from '@angular/router';
import { ExpensesListComponent } from './pages/expenses-list/expenses-list';
import { ExpenseFormComponent } from './pages/expense-form/expense-form';
import { CategoriesComponent } from './pages/categories/categories';

export const routes: Routes = [
  { path: '', component: ExpensesListComponent },
  { path: 'add', component: ExpenseFormComponent },
  { path: 'edit/:id', component: ExpenseFormComponent },
  { path: 'categories', component: CategoriesComponent },
];
