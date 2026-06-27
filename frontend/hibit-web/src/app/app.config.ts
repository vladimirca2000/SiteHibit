import { ApplicationConfig, APP_INITIALIZER, provideZoneChangeDetection } from '@angular/core';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { authInterceptor } from './core/auth/auth.interceptor';
import { authInitializer } from './core/auth/auth.initializer';
import { themeInitializer } from './core/theme/theme.initializer';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor])),
    {
      provide: APP_INITIALIZER,
      useValue: authInitializer,
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      useValue: themeInitializer,
      multi: true,
    },
  ],
};
