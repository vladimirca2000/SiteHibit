import { Component, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { siteContent } from '../../../shared/content/site-content';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  readonly company = siteContent.company;
  readonly navItems = [
    { path: '/', label: 'Início', exact: true },
    { path: '/sobre', label: 'Sobre', exact: false },
    { path: '/contato', label: 'Contato', exact: false },
  ];

  readonly menuOpen = signal(false);

  toggleMenu(): void {
    this.menuOpen.update((open) => !open);
  }

  closeMenu(): void {
    this.menuOpen.set(false);
  }
}
