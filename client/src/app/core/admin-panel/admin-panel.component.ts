import { Component, OnInit } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { UserService } from '../../shared/services/user.service';
import { CommonModule } from '@angular/common';
import { DataSource } from '@angular/cdk/collections';
import User from '../../shared/models/user';
import { Observable } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';


@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [
    MatTableModule,
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    MatSlideToggleModule
  ],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.scss',
})
export class AdminPanelComponent implements OnInit {
  displayedColumns = [
    'username',
    'firstName',
    'lastName',
    'phoneNumber',
    'registrationDate',
    'roles',
    'blocked',
    'actions',
  ];
  date = new Date();
  users$!: Observable<User[]>;
  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.users$ = this.userService.getAllUsers();
  }

  

  deleteUser(id:string) {
    this.userService.deleteUser(id).subscribe(() =>{console.log('deleted')}); //update users
  }
}
