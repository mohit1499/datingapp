//This is TypeScript syntax. It tells the TypeScript compiler what symbols (classes, functions, constants) you're using in te file
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnDestroy, OnInit, Signal, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Nav } from "../layout/nav/nav";
import { lastValueFrom } from 'rxjs';
import { AccountService } from '../core/services/account-service';
import { Home } from "../features/home/home";
import { User } from '../types/user';

@Component({
  selector: 'app-root',  // This tells the name of componnent
  imports: [RouterOutlet, Nav, Home], // “These are the modules, components, pipes, or directives that this component depends on in its HTML template.”
  templateUrl: './app.html',  //html component
  styleUrl: './app.css'
})
export class App implements OnInit {
  private accountService = inject(AccountService);
  private http = inject(HttpClient); //This is dependency injection, we can also inject in constructor.
  protected title = 'Dating App';
  protected members = signal<User[]>([]); /// we used signal here because our project is Angular zoneless

  async ngOnInit() {
    this.members.set(await this.getMembers());
    this.setCurrentUser();
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;

    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  async getMembers() {
    try {
      return lastValueFrom( this.http.get<User[]>('https://localhost:5001/api/members'));
    } catch (error) {
      console.log(error);
      throw error;
    }
  }
}
