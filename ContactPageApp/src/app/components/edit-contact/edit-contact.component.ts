import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit-contact.component.html',
  styleUrls: ['./edit-contact.component.css']
})

//Komponent do obsługi edycji kontaktu
export class EditContactComponent implements OnInit {

  contact!: Contact;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private contactService: ContactService
  ) { }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    const id = idParam !== null ? +idParam : 0; // Lub inna domyślna wartość

    if (id === 0) {
        // Obsługa przypadku, gdy id jest nieprawidłowe
        console.error('Invalid contact ID');
        this.router.navigate(['/contacts']); // Przekierowanie do listy kontaktów
        return;
    }
    this.contactService.getContact(id).subscribe(contact => {
      this.contact = contact;
    });
  }

  onSubmit() {
    this.contactService.updateContact(this.contact).subscribe(() => {
      this.router.navigate(['/contacts']);
    });
  }
}
