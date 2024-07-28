import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ContactService } from '../../services/contact.service';
import { ContactCategoryService } from '../../services/contact-category.service';
import { Contact } from '../../models/contact';
import { ContactCategory } from '../../models/contact-category';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})

//Komponent odpowiedzialny za form do dodania nowego kontaktu 
export class ContactFormComponent implements OnInit {
  contactForm!: FormGroup;
  categories: ContactCategory[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private contactService: ContactService,
    private categoryService: ContactCategoryService
  ) { }

  ngOnInit(): void {
    this.contactForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      categoryId: ['', Validators.required],
      subCategory: ['',Validators.required],
      phoneNumber: ['', Validators.required],
      dateOfBirth: ['', Validators.required]
    });

    // Pobranie kategorii
    this.categoryService.getCategories().subscribe(data => {
      this.categories = data;
    });
  }

  onSubmit(): void {
    if (this.contactForm.valid) {
      const contact: Contact = {
        ...this.contactForm.value,
        //Przypisanie kategorii z bazy odpowiadającej Id kategorii
        category: this.categories.find(cat => cat.id == this.contactForm.value.categoryId)
      };

      this.contactService.addContact(contact).subscribe(response => {
        console.log('Contact added:', response);
        this.router.navigate(['/contacts']);
      });
    }
  }
}
