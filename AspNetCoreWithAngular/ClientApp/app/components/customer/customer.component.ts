import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
//import { Customer } from './Customer';

@Component({
    selector: 'customer',
    templateUrl: './customer.component.html'
})
export class CustomerComponent {
    public customers: Customer[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/customers').subscribe(result => {
            this.customers = result.json() as Customer[];
        }, error => console.error(error));
    }

}
class Customer {
    id: number | undefined;
    firstName: string | undefined;
    lastName: string | undefined;
}
