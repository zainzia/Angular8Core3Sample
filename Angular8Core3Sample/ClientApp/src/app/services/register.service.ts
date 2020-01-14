import { EventEmitter, Inject, Injectable, PLATFORM_ID } from "@angular/core";
import { isPlatformBrowser } from '@angular/common';
import 'rxjs/Rx';
import { NodeService } from "./node.service";
import { Router } from "@angular/router";
import { RegistrationProfile } from "../interfaces/Home/RegistrationProfile";


@Injectable()
export class RegisterService {


    urlToNavigate: string;

    constructor(private nodeService: NodeService,
        private router: Router,
        @Inject(PLATFORM_ID) private platformId: any) {
    }


    register(registrationProfile: RegistrationProfile): Promise<any> {

        let url = 'API/Registration';

        return this.nodeService.postObject(url, registrationProfile).then(result => {

            if (result) {
                if (this.urlToNavigate) {
                    this.router.navigate([this.urlToNavigate]);
                }
            }

            return Promise.resolve(result);

        }, (reason) => {
            return Promise.reject(reason);
        }).catch(error => {
            return Promise.reject(error);
        });
    }


}

