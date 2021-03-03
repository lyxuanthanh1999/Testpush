import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoriesComponent } from './categories/categories.component';
import { CommentsComponent } from './courses/comments/comments.component';

import { CoursesComponent } from './courses/courses.component';
import { LessonsComponent } from './courses/lessons/lessons.component';
import { PromotionsComponent } from './promotions/promotions.component';
import { AuthGuard } from '../../shared';
import { CoursesDetailComponent } from './courses/courses-detail/courses-detail.component';
import { LessonsDetailComponent } from './courses/lessons-detail/lessons-detail.component';
import { OrdersComponent } from './orders/orders.component';


const routes: Routes = [
    {
        path: '',
        component: CoursesComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'courses',
        component: CoursesComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'promotions',
        component: PromotionsComponent,
        data: {
            functionCode: 'Promotions'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'courses/:coursesId/lessons',
        component: LessonsComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'courses/:coursesId/lessons/lessons-detail/:id',
        component: LessonsDetailComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'courses/:coursesId/lessons/lessons-detail',
        component: LessonsDetailComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    }
    ,
    {
        path: 'courses/courses-detail/:coursesId',
        component: CoursesDetailComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'courses/courses-detail',
        component: CoursesDetailComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'orders',
        component: OrdersComponent,
        data: {
            functionCode: 'Orders'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'categories',
        component: CategoriesComponent,
        data: {
            functionCode: 'Categories'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'courses/:courseId/comments',
        component: CommentsComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'courses/:courseId/lessons/:lessonId/comments',
        component: CommentsComponent,
        data: {
            functionCode: 'Courses'
        },
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProductsRoutingModule { }
