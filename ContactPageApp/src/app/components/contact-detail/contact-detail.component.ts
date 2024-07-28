import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact';

@Component({
  selector: 'app-contact-detail',
  templateUrl: './contact-detail.component.html',
  styleUrls: ['./contact-detail.component.css']
})

// Komponent odpowiedzialny za wyświetlenie szczegółów kontaktu 
export class ContactDetailComponent implements OnInit {
  contact!: Contact;

  constructor(
    private route: ActivatedRoute,
    private contactService: ContactService
  ) { }
//Metoda na wejściu pobiera id wybranego kontaktu, i pobiera jego dane 
  ngOnInit(): void {
    const contactId = this.route.snapshot.paramMap.get('id');
    if (contactId) {
      this.contactService.getContact(+contactId).subscribe(data => {
        this.contact = data;
      });
    }
  }
}
