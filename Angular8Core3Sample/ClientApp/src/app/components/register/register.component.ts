
import { Component, Inject, OnInit, ViewEncapsulation } from "@angular/core";

import { FormGroup, FormControl, FormBuilder, Validators, ValidationErrors } from '@angular/forms';

import { Router } from "@angular/router";

import { Language } from "../../interfaces/Home/language.module";

import { Country } from "../../interfaces/Home/country.module";

import { Province } from "../../interfaces/Home/Province";

import { State } from "../../interfaces/Home/State";

import { LanguageService } from "../../services/language.service";

import { CountryService } from "../../services/country.service";

import { SelectItem } from "primeng/api";

import { RegistrationProfile } from "../../interfaces/Home/RegistrationProfile";

import { UserProfile } from "../../interfaces/Home/UserProfile";

import { Address } from "../../interfaces/Home/address";

import { RegisterService } from "../../services/register.service";

import { RegistrationResult, RegistrationResultEnum } from "../../interfaces/Home/RegistrationResult";

import { AuthService } from "../../services/auth.service";

import { BreadcrumbService } from "../../services/breadcrumb.service";


@Component({
  selector: "register",
  templateUrl: "./register.component.html",
  styleUrls: ['./register.component.css'],
  providers: [RegisterService]
})

export class RegisterComponent implements OnInit {

    form!: FormGroup;

    languages!: SelectItem[];

    selectedLanguage!: Language;

    countries!: SelectItem[];

    selectedCountry!: Country;

    provinces!: SelectItem[];

    selectedProvince!: Province;

    states!: SelectItem[];

    selectedState!: State;

    clientId: string = "northZ.club";


    constructor(private router: Router,
                private languageService: LanguageService,
                private countryService: CountryService,
                private registerService: RegisterService,
                private authService: AuthService,
                private breadcrumbService: BreadcrumbService,
                private fb: FormBuilder) { }


    ngOnInit() {

        this.breadcrumbService.setCrumbs([{
            label: 'Register',
            routerLink: '/Register'
        }]);

        this.createForm();
        this.getLanguages();
        this.getCountries();
        this.getProvinces();
        this.getStates();
    }


    getLanguages() {

        this.languages = <SelectItem[]>[];
        this.languages.push(<SelectItem>{
            label: 'English',
            value: <Language>{
                Name: 'English',
                Culture: 'en-US',
                LanguageID: 1
            }
        });
    }


    getCountries() {
        this.countryService.getAllCountries().then((countries) => {
            this.countries = countries.map((x: Country) => <SelectItem>{
                label: x.Name,
                value: x
            });
        });
    }


    getProvinces() {
        this.countryService.getAllProvinces().then((provinces) => {
            this.provinces = provinces.map((x: Province) => <SelectItem>{
                label: x.Name,
                value: x
            });
        });
    }


    getStates() {
        this.countryService.getAllStates().then((states) => {
            this.states = states.map((x: State) => <SelectItem>{
                label: x.Name,
                value: x
            });
        });
    }


    createForm() {
      this.form = this.fb.group({
          Username: new FormControl('', {
              validators: [Validators.required, Validators.minLength(5)]
          }),
          Email: new FormControl('', {
              validators: [Validators.required, Validators.email]
          }),
          Password: new FormControl('', {
              validators: [Validators.required, Validators.minLength(8), Validators.maxLength(16), Validators.pattern('(?:(?=.*[a-z])(?=.*[A-Z]).*)')]
          }),
          ConfirmPassword: new FormControl('', {
              validators: [Validators.required, this.passwordConfirmValidator]
          }),
          FirstName: new FormControl('', {
              validators: [Validators.required, Validators.minLength(2), Validators.maxLength(16)]
          }),
          LastName: new FormControl('', {
              validators: [Validators.required, Validators.minLength(2), Validators.maxLength(16)]
          }),
          Phone: new FormControl('', {
              validators: [Validators.minLength(10), Validators.maxLength(16)]
          }),
          Address1: new FormControl('', {
              validators: [Validators.required, Validators.minLength(5), Validators.maxLength(100), Validators.pattern('(?=.*?[0-9])(?=.*?[A-Za-z]).+')]
          }),
          Address2: new FormControl(''),
          City: new FormControl('', {
              validators: [Validators.required, Validators.minLength(3), Validators.maxLength(50)]
          }),
          PostalCode: new FormControl('', {
              validators: [Validators.required, Validators.minLength(5), Validators.maxLength(10), Validators.pattern('([A-Za-z][0-9][A-Za-z][ ]?[0-9][A-Za-z][0-9])|([0-9]{5}([-][0-9]{4})?)')]
          }),
          Province2: new FormControl('', [ Validators.minLength(3), Validators.maxLength(50) ]),
          Languages: new FormControl(this.selectedLanguage, Validators.required),
          Countries: new FormControl(this.selectedCountry, Validators.required),
          Provinces: new FormControl(this.selectedProvince),
          States: new FormControl(this.selectedState)
      },
      {
          validators: [this.provinceValidator]
      });
    }


    getRegistrationProfile() {
        let registrationProfile = <RegistrationProfile>{
            Password: this.form.controls['Password'].value,
            ClientId: this.clientId,
            UserProfile: <UserProfile>{
                Email: this.form.controls['Email'].value,
                FirstName: this.form.controls['FirstName'].value,
                LastName: this.form.controls['LastName'].value,
                Language: this.form.controls['Languages'].value,
                PhoneNumber: this.form.controls['Phone'].value,
                Username: this.form.controls['Username'].value,
                Address: <Address>{
                    Address1: this.form.controls['Address1'].value,
                    Address2: this.form.controls['Address2'].value,
                    City: this.form.controls['City'].value,
                    Country: this.form.controls['Countries'].value,
                    FirstName: this.form.controls['FirstName'].value,
                    LastName: this.form.controls['LastName'].value,
                    PhoneNumber: this.form.controls['Phone'].value
                }
            }
        };

        if (this.form.controls['Countries'].value === 'Canada') {
            registrationProfile.UserProfile.Address.Province = this.form.controls['Provinces'].value.ProvinceID;
            registrationProfile.UserProfile.Address.PostalCode = this.form.controls['PostalCode'].value;
        }
        else if (this.form.controls['Countries'].value === 'USA') {
            registrationProfile.UserProfile.Address.State = this.form.controls['States'].value.StateID;
            registrationProfile.UserProfile.Address.ZipCode = this.form.controls['PostalCode'].value;
        }
        else {
            registrationProfile.UserProfile.Address.Province2 = this.form.controls['Province2'].value;
            registrationProfile.UserProfile.Address.PostalCode = this.form.controls['PostalCode'].value;
        }

        return registrationProfile;
    }


    register(registrationProfile: RegistrationProfile) {
        this.registerService.register(registrationProfile).then((result: RegistrationResult) => {
            if (result.Result === RegistrationResultEnum.Succeeded) {

                this.authService.login(result.UserProfile.Username, this.form.controls['Password'].value).subscribe(res => {
                    this.router.navigate([this.registerService.urlToNavigate]);
                });

            }
            else if (result.Result === RegistrationResultEnum.EmailAlreadyRegistered) {

                let errors = <ValidationErrors>{};

                for (let i = 0; i < result.Errors.length; i++) {
                    errors['Error' + (i + 1)] = result.Errors[i];
                }

                this.form.setErrors(errors);
            }
            else if (result.Result === RegistrationResultEnum.UsernameAlreadyExists) {

                let errors = <ValidationErrors>{};

                for (let i = 0; i < result.Errors.length; i++) {
                    errors['Error' + (i + 1)] = result.Errors[i];
                }

                this.form.setErrors(errors);
            }
            else if (result.Result === RegistrationResultEnum.Failed) {

                let errors = <ValidationErrors>{};

                for (let i = 0; i < result.Errors.length; i++) {
                    errors['Error' + (i + 1)] = result.Errors[i];
                }

                this.form.setErrors(errors);
            }
        }).catch((reason) => {
            this.form.setErrors({ "Error": "An error occurred. Please try again later!" });
        });
    }


    onSubmit() {
        let registrationProfile = this.getRegistrationProfile();
        this.register(registrationProfile);
    }


    cancel() {
        this.router.navigate([this.registerService.urlToNavigate]);
    }


    getFormErrors() {

        if (this.form.errors) {
            let errors = '';

            for (let key in this.form.errors) {
                errors += '<div>' + this.form.getError(key) + '</div>'
            }

            return errors;
        }

        return null;
    }


    passwordConfirmValidator(control: FormControl): any {

      let p = control.root.get('Password');
      let pc = control.root.get('ConfirmPassword');

      if (p && pc) {
        if (p.value === pc.value) {
          return null;
        }
      }

      return { 'PasswordMismatch': true };
    }


    provinceValidator(control: FormControl): any {

        let province2 = control.root.get('Province2');
        let provinces = control.root.get('Provinces');
        let states = control.root.get('States');
        let countries = control.root.get('Countries');

        if(province2.dirty || provinces.dirty || states.dirty) {

            if (countries && countries.value) {
                if (countries.value.Name === 'Canada' && provinces && provinces.value) {
                    province2.setErrors(null);
                    states.setErrors(null);
                    return null;
                }
                else if (countries.value.Name === 'USA' && states && states.value) {
                    province2.setErrors(null);
                    provinces.setErrors(null);
                    return null;
                }
                else if ((countries.value.Name !== 'USA' && countries.value.Name !== 'Canada') &&
                    province2.value && province2.value.length > 2) {
                    provinces.setErrors(null);
                    states.setErrors(null);
                    return null;
                }
            }

            return { 'Provinces': 'Please input or select a Province / State' };
        }

        return null;
    }


    /**************************** FORM **************************************/

    getFormControl(name: string) {
        return this.form.get(name);
    }

    isValid(name: string) {
        var e = this.getFormControl(name);
        return e && e.valid;
    }

    isChanged(name: string) {
        var e = this.getFormControl(name);
        return e && (e.dirty || e.touched);
    }

    hasError(name: string) {
        var e = this.getFormControl(name);
        return e && (e.dirty || e.touched) && !e.valid;
    }

    /***********************************************************************/

}

