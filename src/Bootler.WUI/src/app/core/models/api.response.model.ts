export interface BaseResponse<T> {
  success: boolean;
  statusCode: number;
  message?: string;
  errors?: string[];
  data: T;
}

export interface PaginatedList<T> {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  items: T[];
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
