//This is TypeScript syntax. It tells the TypeScript compiler what symbols (classes, functions, constants) you're using in te file
import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { Nav } from "../layout/nav/nav";

@Component({
  selector: 'app-root',  // This tells the name of componnent
  imports: [RouterOutlet, Nav], // “These are the modules, components, pipes, or directives that this component depends on in its HTML template.”
  templateUrl: './app.html',  //html component
  styleUrl: './app.css'
})
export class App  {
 
  protected router = inject(Router);

}
