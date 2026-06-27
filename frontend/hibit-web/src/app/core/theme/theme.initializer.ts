import { inject } from '@angular/core';
import { ThemeService } from './theme.service';

export function themeInitializer(): void {
  inject(ThemeService).init();
}
