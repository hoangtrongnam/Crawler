import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@environments/environment';

import { List } from 'linq-typescript';

import { User } from '@/_models';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    getChirrentMenu(menu, orgMenus){
        menu.menus = new List<any>(orgMenus).where(w=>w.parentid === menu.id).toArray();
        menu.menus.forEach(item => {
            this.getChirrentMenu(item, orgMenus);
        });
    }

    login(username, password) {
        return this.http.post<any>(`${environment.apiUrl}/account/login`, { username, password })
            .pipe(map(user => {

                let menus = new List<any>(user.data.info.menus).where(w=>w.parentid === null).toArray();
                
                menus.forEach(menu => {
                    this.getChirrentMenu(menu, user.data.info.menus);
                });


                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify({token : user.data.token, info: user.data.info }));
                localStorage.setItem('orgmenu', JSON.stringify(user.data.info.menus));
                localStorage.setItem('menus', JSON.stringify(menus));
               
                 this.currentUserSubject.next(user.data);
                 return user;
             }));
    }

    logout() {
        // remove user from local storage and set current user to null
        localStorage.removeItem('currentUser');
        localStorage.removeItem('menus');
        this.currentUserSubject.next(null);
        //location.reload(true);
    }
}