import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export function authInitializer(): Promise<void> {
  const authService = inject(AuthService);
  return authService.ensureAuthenticated().catch(() => {
    // Permite carregar o site mesmo se a API estiver offline no momento.
    // O login será tentado novamente ao enviar o formulário de contato.
    return undefined;
  });
}
