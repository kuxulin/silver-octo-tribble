import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-initial',
  standalone: true,
  imports: [MatButtonModule, RouterModule],
  templateUrl: './initial.component.html',
  styleUrl: './initial.component.scss',
})
export class InitialComponent {}
