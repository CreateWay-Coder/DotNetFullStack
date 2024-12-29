import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Inject, inject } from '@angular/core';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/account.service';
import { HomeComponent } from "./home/home.component";
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NavComponent, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  private accoutService = inject(AccountService);
  title = 'client';

  ngOnInit(): void {
    this.setCurretUser();
  }

  setCurretUser() {
    const userString = localStorage.getItem("user");
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accoutService.currentUser.set(user);
  }
}
