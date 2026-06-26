import { Component, inject } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { siteContent } from '../../shared/content/site-content';
import { environment } from '../../../environments/environment';
import { ContactService } from './contact.service';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss',
})
export class ContactComponent {
  private readonly fb = inject(FormBuilder);
  private readonly contactService = inject(ContactService);
  private readonly authService = inject(AuthService);
  private readonly sanitizer = inject(DomSanitizer);

  readonly content = siteContent;
  readonly mapUrl: SafeResourceUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
    environment.mapEmbedUrl,
  );
  readonly contactEmail = environment.contactEmail;

  submitting = false;
  successMessage = '';
  errorMessage = '';

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(320)]],
    phone: ['', Validators.maxLength(30)],
    subject: ['', [Validators.required, Validators.maxLength(200)]],
    message: ['', [Validators.required, Validators.maxLength(5000)]],
    consentGiven: [false, Validators.requiredTrue],
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;
    this.successMessage = '';
    this.errorMessage = '';

    const { name, email, phone, subject, message, consentGiven } = this.form.getRawValue();

    this.authService
      .ensureAuthenticated()
      .then(() =>
        this.contactService.submit({
          name,
          email,
          phone: phone || null,
          subject,
          message,
          consentGiven,
        }),
      )
      .then((observable) => {
        observable.subscribe({
          next: () => {
            this.successMessage = 'Mensagem enviada com sucesso! Entraremos em contato em breve.';
            this.form.reset({ consentGiven: false });
            this.submitting = false;
          },
          error: () => {
            this.errorMessage =
              'Não foi possível enviar sua mensagem. Verifique se a API está rodando e tente novamente.';
            this.submitting = false;
          },
        });
      })
      .catch(() => {
        this.errorMessage =
          'Não foi possível conectar à API. Inicie o backend e tente novamente.';
        this.submitting = false;
      });
  }

  hasError(controlName: keyof typeof this.form.controls): boolean {
    const control = this.form.controls[controlName];
    return control.invalid && control.touched;
  }
}
