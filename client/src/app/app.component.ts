import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Inject, inject } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
http = inject(HttpClient);
title = 'client';
users:any;

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/user').subscribe({
      next: response => this.users=response,
      error: error => console.log(error),
      complete: ()=> console.log('Request has been conpleted')
    })
  }
}