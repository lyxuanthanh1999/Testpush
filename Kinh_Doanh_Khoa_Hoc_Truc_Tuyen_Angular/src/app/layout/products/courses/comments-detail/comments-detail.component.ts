
import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { Comments } from '../../../../shared/models';
import { CommentsService } from '../../../../shared/services/comments.service';
@Component({
  selector: 'app-comments-detail',
  templateUrl: './comments-detail.component.html',
  styleUrls: ['./comments-detail.component.scss']
})
export class CommentsDetailComponent implements OnInit, OnDestroy {
  constructor(
    public bsModalRef: BsModalRef,
    private commentsService: CommentsService) {
  }

  private subscription = new Subscription();
  public dialogTitle: string;
  public commentId: number;
  public btnDisabled = false;
  public blockedPanel = false;
  public comment: Comments;

  ngOnInit() {
    if (this.commentId) {
      this.loadFormDetails(this.commentId);
    }
  }
  private loadFormDetails(commentId) {
    this.blockedPanel = true;
    this.subscription.add(this.commentsService.getDetail(commentId)
      .subscribe((response: Comments) => {
        this.comment = response;
        setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; this.btnDisabled = false; }, 1000);
      }));
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
