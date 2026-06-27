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
  readonly storyImageUrl =
    'https://images.pexels.com/photos/7988116/pexels-photo-7988116.jpeg?auto=compress&cs=tinysrgb&w=1200';
}
