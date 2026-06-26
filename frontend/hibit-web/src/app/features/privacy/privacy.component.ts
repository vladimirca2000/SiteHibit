import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { siteContent } from '../../shared/content/site-content';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-privacy',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './privacy.component.html',
  styleUrl: './privacy.component.scss',
})
export class PrivacyComponent {
  readonly content = siteContent;
  readonly contactEmail = environment.contactEmail;
}
