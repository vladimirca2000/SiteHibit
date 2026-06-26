import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { siteContent } from '../../../shared/content/site-content';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss',
})
export class FooterComponent {
  readonly company = siteContent.company;
  readonly content = siteContent;
  readonly year = new Date().getFullYear();
}
