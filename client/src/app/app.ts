//This is TypeScript syntax. It tells the TypeScript compiler what symbols (classes, functions, constants) you're using in te file
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnDestroy, OnInit, Signal, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',  // This tells the name of componnent
  imports: [RouterOutlet], // “These are the modules, components, pipes, or directives that this component depends on in its HTML template.”
  templateUrl: './app.html',  //html component
  styleUrl: './app.css'
})
export class App implements OnInit{
  
  private http = inject(HttpClient); //This is dependency injection, we can also inject in constructor.
  protected title = 'Dating App';
  protected members=signal<any>([]); 

  ngOnInit(): void {
     this.http.get('https://localhost:5001/api/members').subscribe({
        next: response => this.members.set(response),
        error: error=> console.log(error),
        complete: () => console.log("completed")        
     })
  }
}
