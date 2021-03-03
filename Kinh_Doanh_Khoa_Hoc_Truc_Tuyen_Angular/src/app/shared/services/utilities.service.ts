import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class UtilitiesService extends BaseService {
  constructor(private http: HttpClient) {
    super();
  }
  UnflatteringForLeftMenu = (arr: any[]): any[] => {
    const map = {};
    const roots: any[] = [];
    for ( let i = 0; i < arr.length; i += 1) {
      const node = arr[i];
      node.children = [];
      map[node.id] = i; // use map to look-up the parents
      if (node.parentId !== null) {
        delete node['children'];
        arr[map[node.parentId]].children.push(node);
      } else {
        roots.push(node);
      }
    }
    return roots;
  }
  UnflatteringForTree = (arr: any[]): any[] => {
    const roots: any[] = [];
    for (let i = 0; i < arr.length; i++) {
      const node = {
        data: {
          id: '',
          parentId: '',
          name: '',
          sortOrder: '',
          hasCreate: '',
          hasUpdate: '',
          hasDelete: '',
          hasView: '',
          hasApprove: '',
          hasExportExcel: '',
        },
        expanded: false,
        children: []
      };
      node.data = arr[i];
      if (node.data.parentId !== null) {
        for (let y = 0; y < roots.length; y++) {
          if (roots[y].data.id === node.data.parentId) {
            roots[y].children.push(node);
          }
        }
      } else {
        roots.push(node);
      }
    }
    return roots;
  }
  ToFormData(formValue: any) {
    const formData = new FormData();
    for (const key of Object.keys(formValue)) {
      const value = formValue[key];
      formData.append(key, value);
    }
    return formData;
  }
}
