import { Component } from '@angular/core';
import { siteContent } from '../../../shared/content/site-content';

@Component({
  selector: 'app-whatsapp-fab',
  standalone: true,
  templateUrl: './whatsapp-fab.component.html',
  styleUrl: './whatsapp-fab.component.scss',
})
export class WhatsappFabComponent {
  readonly whatsapp = siteContent.company.whatsapp;
}
