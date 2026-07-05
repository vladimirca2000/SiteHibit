import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { siteContent } from '../../../shared/content/site-content';
import { ThemeService } from '../../theme/theme.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  host: {
    '[class.header-host--menu-open]': 'menuOpen()',
  },
})
export class HeaderComponent {
  private readonly themeService = inject(ThemeService);
  readonly company = siteContent.company;
  readonly navItems = [
    { path: '/', label: 'Início', exact: true },
    { path: '/sobre', label: 'Sobre', exact: false },
    { path: '/contato', label: 'Contato', exact: false },
  ];

  readonly menuOpen = signal(false);
  readonly isDarkTheme = this.themeService.isDarkTheme;

  toggleMenu(): void {
    this.menuOpen.update((open) => !open);
  }

  closeMenu(): void {
    this.menuOpen.set(false);
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }
}
