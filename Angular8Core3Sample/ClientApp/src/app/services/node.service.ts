import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Router, ActivatedRoute } from '@angular/router';
import { LoaderService } from '../services/loader.service';

import { Inject } from '@angular/core';
import { throwError } from 'rxjs';


@Injectable({
    providedIn: 'root',
})
export class NodeService {

    private numberOfCurrentRequests: number = 0;


    constructor(private http: HttpClient,
        private loaderService: LoaderService,
        @Inject('BASE_URL') private baseUrl: string) { }

    fetchURL(url: string) {
      this.startRequest();

        let promise = new Promise((resolve, reject) => {
            this.http.get<any>(this.baseUrl + url)
                .toPromise()
                .then(res => {
                    resolve(res);
                }, msg => {
                  reject(msg);
              }).then(() => { this.completeRequest() })
        });
        
        return promise;
    }

    putObject(url: string, objectToPut: object) {
      this.startRequest();

      const headers = new HttpHeaders().set('content-type', 'application/json');

      let promise = new Promise((resolve, reject) => {
        this.http.put<any>(this.baseUrl + url, JSON.stringify(objectToPut), { headers } )
          .toPromise()
          .then(res => {
            resolve(res);
          }, msg => {
            reject(msg);
          }).then(() => { this.completeRequest() })
      });

      return promise;
    }

    postObject(url: string, objectToPost: object) {
      this.startRequest();

      const headers = new HttpHeaders().set('content-type', 'application/json');

      let promise = new Promise((resolve, reject) => {
        this.http.post<any>(this.baseUrl + url, objectToPost, { headers })
          .toPromise()
          .then(res => {
            resolve(res);
          }, msg => {
            reject(msg);
          }).then(() => { this.completeRequest() })
      });

      return promise;
    }

    deleteObject(url: string) {
      this.startRequest();

      let promise = new Promise((resolve, reject) => {
        this.http.delete<any>(this.baseUrl + url)
          .toPromise()
          .then(res => {
            resolve(res);
          }, msg => {
            reject(msg);
          }).then(() => { this.completeRequest() })
      });

      return promise;
    }
    
    getFiles() {

        //return this.http.get<any>('assets/showcase/data/files.json')
        //    .toPromise()
        //    .then(res => <TreeNode[]>res.data);

    }

    getLazyFiles() {

        //return this.http.get<any>('assets/showcase/data/files-lazy.json')
        //    .toPromise()
        //    .then(res => <TreeNode[]>res.data);

    }

    getFilesystem() {

        //return this.http.get<any>('assets/showcase/data/filesystem.json')
        //    .toPromise()
        //    .then(res => <TreeNode[]>res.data);

    }

    getLazyFilesystem() {

        //return this.http.get<any>('assets/showcase/data/filesystem-lazy.json')
        //    .toPromise()
        //    .then(res => <TreeNode[]>res.data);

    }

    startRequest() {
      this.numberOfCurrentRequests++;
      this.loaderService.show();
    }

    completeRequest() {
      this.numberOfCurrentRequests--;
      if (this.numberOfCurrentRequests == 0) {
        this.loaderService.hide();
      }
    }

}
