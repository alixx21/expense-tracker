export interface Expense {
  id: number;
  title: string;
  amount: number;
  expenseDate: string;
  categoryName: string;
  categoryId: number;
}

export interface ExpenseCreate {
  title: string;
  amount: number;
  expenseDate: string;
  categoryId: number;
}
