import { DOCUMENT } from '@angular/common';
import { Injectable, inject, signal } from '@angular/core';

type Theme = 'light' | 'dark';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly document = inject(DOCUMENT);
  private readonly storageKey = 'hibit_theme';

  readonly currentTheme = signal<Theme>('light');
  readonly isDarkTheme = signal(false);

  init(): void {
    const savedTheme = this.getStoredTheme();
    const preferredTheme = this.getSystemTheme();
    this.applyTheme(savedTheme ?? preferredTheme);
  }

  toggleTheme(): void {
    const nextTheme: Theme = this.currentTheme() === 'dark' ? 'light' : 'dark';
    this.applyTheme(nextTheme);
  }

  private applyTheme(theme: Theme): void {
    const root = this.document.documentElement;
    root.setAttribute('data-theme', theme);
    this.currentTheme.set(theme);
    this.isDarkTheme.set(theme === 'dark');
    localStorage.setItem(this.storageKey, theme);
  }

  private getStoredTheme(): Theme | null {
    const theme = localStorage.getItem(this.storageKey);
    return theme === 'dark' || theme === 'light' ? theme : null;
  }

  private getSystemTheme(): Theme {
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  }
}
