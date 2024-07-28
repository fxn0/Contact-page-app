import { ContactCategory } from './contact-category';


export interface Contact {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    categoryId: number;
    category: ContactCategory;
    subCategory: string;
    phoneNumber: string;
    dateOfBirth: string;
}