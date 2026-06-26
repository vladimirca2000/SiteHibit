import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface ContactFormData {
  name: string;
  email: string;
  phone?: string | null;
  subject: string;
  message: string;
  consentGiven: boolean;
}

@Injectable({ providedIn: 'root' })
export class ContactService {
  private readonly http = inject(HttpClient);

  submit(data: ContactFormData): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${environment.apiUrl}/api/contact`, data);
  }
}
