import { Invoice, InvoiceFilter } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiCrudService } from './api-crud.service';

@Injectable({
  providedIn: 'root'
})
export class ApiInvoiceService extends ApiCrudService<Invoice, InvoiceFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/invoices';
  }
}
