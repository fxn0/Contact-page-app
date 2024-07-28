// src/app/services/contact-category.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContactCategory } from '../models/contact-category';

@Injectable({
  providedIn: 'root'
})
export class ContactCategoryService {
  private apiUrl = 'https://localhost:7166/api/Categories';

  constructor(private http: HttpClient) { }

  getCategories(): Observable<ContactCategory[]> {
    return this.http.get<ContactCategory[]>(this.apiUrl);
  }

  getCategory(id: number): Observable<ContactCategory> {
    return this.http.get<ContactCategory>(`${this.apiUrl}/${id}`);
  }
}
