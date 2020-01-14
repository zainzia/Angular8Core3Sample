import { EventEmitter, Inject, Injectable, PLATFORM_ID } from "@angular/core";
import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import 'rxjs/Rx';
import { NodeService } from "./node.service";
import { Router } from "@angular/router";

import { TokenResponse } from '../interfaces/Home/token.response';

@Injectable()
export class AuthService {

  authKey: string = "auth";
  clientId: string = "northZ.club";
  urlToNavigate: string;

  constructor(private http: HttpClient,
    private nodeService: NodeService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: any) {
  }

    // performs the login
    login(username: string, password: string): Observable<boolean> {
        var url = "API/Identity/Token/Auth";
        var data = {
          username: username,
          password: password,
          client_id: this.clientId,
          // required when signing up with username/password
          grant_type: "password",
          // space-separated list of scopes for which the token is issued
          scope: "offline_access profile email"
        };

        return this.getAuthFromServer(url, data).map(result => {

          console.log(this.urlToNavigate);
          if (result) {
            if (this.urlToNavigate != undefined && this.urlToNavigate != null) {
              this.router.navigate([this.urlToNavigate]);
            }
            else {
              this.router.navigate(["Home"]);
            }
          }

          return result;
        });
    }


    // try to refresh token
    refreshToken(): Observable<boolean> {
        var url = "API/Identity/Token/Auth";
        var data = {
          client_id: this.clientId,
          // required when signing up with username/password
          grant_type: "refresh_token",
          refresh_token: this.getAuth()!.refresh_token,
          // space-separated list of scopes for which the token is issued
          scope: "offline_access profile email"
        };

        return this.getAuthFromServer(url, data);
    }

    // retrieve the access & refresh tokens from the server
    getAuthFromServer(url: string, data: any): Observable<boolean> {
        return this.http.post<TokenResponse>(url, data)
          .map((res) => {
            let token = res && res.token;
            // if the token is there, login has been successful
            if (token) {
              // store username and jwt token
              this.setAuth(res);
              // successful login
              return true;
            }

            // failed login
            return Observable.throw('Unauthorized');
          })
          .catch(error => {
            return new Observable<any>(error);
          });
    }

    // performs the logout
    logout() {

      let authKey = JSON.parse(localStorage.getItem(this.authKey));

      this.nodeService.postObject("API/Identity/Token/Logout", { client_id: this.clientId, refereshToken: authKey.refresh_token }).then((result) => {

        this.setAuth(null);

        return true;
      });

      return false;
    }

    // Persist auth into localStorage or removes it if a NULL argument is given
    setAuth(auth: TokenResponse | null): boolean {
      if (auth) {
        localStorage.setItem(
          this.authKey,
          JSON.stringify(auth));
      }
      else {
        localStorage.removeItem(this.authKey);
      }
      return true;
    }

    // Retrieves the auth JSON object (or NULL if none)
    getAuth(): TokenResponse | null {
      var i = localStorage.getItem(this.authKey);
      if (i) {
        return JSON.parse(i);
      }
      else {
        return null;
      }
    }

    // Returns TRUE if the user is logged in, FALSE otherwise.
    isLoggedIn(): boolean {
      return localStorage.getItem(this.authKey) != null;
    }

    async isUserLoggedIn(userId: string): Promise<{}> {
        let token = JSON.parse(localStorage.getItem(this.authKey));

        if (token != null) {
            var url = `API/Identity/Token/IsUserLoggedIn/${userId}`;
            return this.nodeService.postObject(url, { "token": token.token }).then((result) => {
                if (this.isLoggedIn && !result) {
                    this.logout();
                }
                return result;
            });
        }

        return Promise.resolve(false);
    }

    async isAdminLoggedIn(): Promise<{}> {

      let token = JSON.parse(localStorage.getItem(this.authKey));

      if (token != null) {
        var url = "API/Identity/Token/IsUserAdmin";
        return this.nodeService.postObject(url, { "token": token.token });
      }

      return null;
    }
} 
