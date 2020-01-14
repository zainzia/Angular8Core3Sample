import { Language } from "./language.module";

export interface Country {

    CountryID?: number;
    Name?: string;
    Language?: Language;
    Currency?: string;
}
