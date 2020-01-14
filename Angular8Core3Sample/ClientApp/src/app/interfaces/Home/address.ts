
import { Province } from './Province';

import { Country } from './country.module';

import { State } from './State';

export interface Address {

    FirstName: string,
    LastName: string,
    Address1: string,
    Address2: string,
    City: string,
    Province: Province,
    State: State,
    Province2: string,
    PostalCode: string,
    ZipCode: string,
    Country: Country,
    PhoneNumber: string

}
