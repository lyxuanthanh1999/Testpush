import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatOrder'
})
export class FormatOrderPipe implements PipeTransform {

  transform(value: number, args: string): string {
    if (args === 'paymentMethod') {
      if (value === 0) {
        return 'Cash On Delivery';
      } else if (value === 1) {
        return 'Online Banking';
      } else if (value === 2) {
        return 'Payment Gateway';
      } else {
        return 'Visa';
      }
    } else {
      if (value === 0) {
        return 'New Order';
      } else if (value === 1) {
        return 'In Progress';
      } else if (value === 2) {
        return 'Returned';
      } else if (value === 3) {
        return 'Cancelled';
      } else {
        return 'Completed';
      }
    }
  }

}
