import { Component, EventEmitter, inject, input, Input, Output, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AsyncScheduler } from 'rxjs/internal/scheduler/AsyncScheduler';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  @Input() userFormHomeComponent: any; //old way
  // userFormHomeComponent = input.required<any>(); //new way
  @Output() cancelRegister = new EventEmitter();//old way
  //cancelRegister = output<boolean>();//new way

  model: any = {}
  test: any;

  register() {
    this.accountService.register(this.model).subscribe({
      next: response=>{
        console.log(response);
        this.cancel();
      },
      error: error=> console.log(error)
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}