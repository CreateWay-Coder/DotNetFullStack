import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const accoutService = inject(AccountService);
  const toastr = inject(ToastrService);

  if (accoutService.currentUser()) {
    console.log('user exit');
    return true;
  }
  else{
    console.log('user noy exit');
    toastr.error('You shall not pass!')
    return false;
  }
};
