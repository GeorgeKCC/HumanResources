import { Component } from '@angular/core';
import { Navbar } from '../navbar/navbar';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-template',
  imports: [Navbar, RouterOutlet],
  templateUrl: './template.html'
})
export class Template {

}
