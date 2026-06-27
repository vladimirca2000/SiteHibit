import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { siteContent } from '../../shared/content/site-content';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  readonly content = siteContent;
  readonly heroImageUrl =
    'https://images.pexels.com/photos/6804068/pexels-photo-6804068.jpeg?auto=compress&cs=tinysrgb&w=1600';
}
