import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { KeycloakService } from 'keycloak-angular';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent implements OnInit {
  constructor(private keyCloak: KeycloakService, private router: Router) {}

  async ngOnInit(): Promise<void> {
    // if (await this.keyCloak.isLoggedIn) {
    //   this.router.navigateByUrl('/play');
    // }
  }

  async login() {
    await this.keyCloak.login({
      redirectUri: window.location.origin + '/play'
    });
  }
}
