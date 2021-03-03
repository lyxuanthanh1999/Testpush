import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'convertPath'
})
export class ConvertPathPipe implements PipeTransform {

  transform(value: string, args: string): any {
    if (value.lastIndexOf(args)) {
      return value.substring(value.lastIndexOf(args) + 1);
    }
     return value;
   }

}
