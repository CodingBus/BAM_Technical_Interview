import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MainPage } from './main-page/main-page';

@Component({
  selector: 'app-root',
  imports: [MainPage],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('user-interface');
}
