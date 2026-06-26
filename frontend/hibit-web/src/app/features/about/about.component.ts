import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { siteContent } from '../../shared/content/site-content';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss',
})
export class AboutComponent {
  readonly content = siteContent;
}
