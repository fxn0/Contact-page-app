// src/app/components/contact-list/contact-list.component.ts
import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})

// komponent odpowiedzialny za wyświetlanie listy kontaktów
export class ContactListComponent implements OnInit {
  contacts: any[] = [];
  isLoggedIn: boolean = false;

  constructor(
    private contactService: ContactService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadContacts();
    this.authService.getLoggedIn().subscribe(status => {
      this.isLoggedIn = status;
    });
  }
  //Metoda odpowiedzialna za załadowanie kontaktów
  loadContacts(): void {
    this.contactService.getContacts().subscribe(data => {
      this.contacts = data;
    });
  }
   //Metoda odpowiedzialna za usunięcie wybranego kontaktu
  deleteContact(id: number): void {
    if (this.isLoggedIn) {
      this.contactService.deleteContact(id).subscribe(() => {
        this.loadContacts();
      });
    } else {
      alert('You need to be logged in to delete a contact');
    }
  }
}
