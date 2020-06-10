import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // currentUser: User;

  constructor(
    private titleService: Title,
    // private authenticationService: AuthenticationService,
    private router: Router,
    public translate: TranslateService
  ) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.translate.addLangs(['en', 'vi']);
    this.translate.setDefaultLang('vi');
    this.translate.use('vi');
  }

  ngOnInit() {

  }

  // logout() {
  //   this.authenticationService.logout();
  //   this.router.navigate(['login']);
  // }
}
