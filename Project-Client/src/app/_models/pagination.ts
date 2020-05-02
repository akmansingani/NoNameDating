export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export class PaginationClass<T>{
  pagination: Pagination;
  result: T;
}
