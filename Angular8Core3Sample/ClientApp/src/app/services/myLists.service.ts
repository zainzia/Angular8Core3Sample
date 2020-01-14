
import { Injectable } from '@angular/core';
import { NodeService } from './node.service';
import { AuthService } from './auth.service';

import { JsonNetDecycleModule } from '../helpers/json-net-decycle.module';


@Injectable()
export class MyListsService {

    decycle = new JsonNetDecycleModule();


    constructor(private nodeService: NodeService, private authService: AuthService) {

    }


    isLoggedIn() {
        return this.authService.isLoggedIn();
    }

    getMyListNames(languageId: number, countryId: number) {

        if (!this.isLoggedIn()) {
            return Promise.reject("You must login to continue to My Lists.");
        }

        return this.nodeService.fetchURL(`API/MyLists/MyListNames/${languageId}/${countryId}`)
            .then(
                (myListNames) => {
                    return this.decycle.retrocycle(myListNames);
                },
                (reason) => {
                    return Promise.reject(reason);
                });
    }

    createMyList(listName: string) {

        if (!this.isLoggedIn()) {
            return Promise.reject("You must login to continue to My Lists.");
        }

        return this.nodeService.postObject('API/MyLists/createMyList', Object(listName))
            .then(
                (myLists) => {
                    return this.decycle.retrocycle(myLists);
                },
                (reason) => {
                    return Promise.reject(reason);
                });

    }

    deleteMyList(myListId: number) {

        if (!this.isLoggedIn()) {
            return Promise.reject("You must login to continue to My Lists.");
        }

        return this.nodeService.deleteObject(`API/MyLists/deleteMyList/${myListId}`)
            .then(
                (result) => {
                    return result;
                },
                (reason) => {
                    return Promise.reject(reason);
                });

    }

    getMyLists(languageId: number, countryId: number) {

        if (!this.isLoggedIn()) {
            return Promise.reject("You must login to continue to My Lists.");
        }

        return this.nodeService.fetchURL(`API/MyLists/${languageId}/${countryId}`)
            .then(
              (myLists) => {
                return this.decycle.retrocycle(myLists);
              },
              (reason) => {
                  return Promise.reject(reason);
              });

    }

    deleteMyListItem(productId: number, myListId: number) {
        if (!this.isLoggedIn()) {
            return Promise.reject("You must login to continue to My Lists.");
        }

        return this.nodeService.deleteObject(`API/MyLists/deleteMyListItem/${productId}/${myListId}`)
            .then(
                (result) => {
                    return result;
                },
                (reason) => {
                    return Promise.reject(reason);
                });
    }

    moveMyListItem(moveMyListItem) {
        if (!this.isLoggedIn()) {
            return Promise.reject("You must login to continue to My Lists.");
        }

        return this.nodeService.putObject('API/MyLists/moveMyListItem', moveMyListItem)
            .then(
                (result) => {
                    return result;
                },
                (reason) => {
                    return Promise.reject(reason);
                });
    }

    addMyListItem(addMyListItem) {
        if (!this.isLoggedIn()) {
            return Promise.reject("You must login to continue to My Lists.");
        }

        return this.nodeService.putObject('API/MyLists/addMyListItem', addMyListItem)
            .then(
                (result) => {
                    return result;
                },
                (reason) => {
                    return Promise.reject(reason);
                });
    }
}
