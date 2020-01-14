
import { Component, OnInit } from '@angular/core';

import { ConfirmDialogParams } from './../../interfaces/Home/ConfirmDialogParams';

import { ConfirmDialogResult } from './../../interfaces/Home/ConfirmDialogResult';

import { ConfirmDialogBinders } from './../../interfaces/Home/ConfirmDialogBinders';

@Component({
    selector: 'app-confirm-dialog',
    templateUrl: './confirm-dialog.component.html',
    styleUrls: ['./confirm-dialog.component.css']
})


/** ConfirmDialog component*/
export class ConfirmDialogComponent implements OnInit{

    displayDialog!: boolean;

    /** ConfirmDialog ctor */
    constructor(private confirmDialogParams: ConfirmDialogParams, private confirmDialogBinders: ConfirmDialogBinders,
        private confirmDialogResult: ConfirmDialogResult) {
    }

    ngOnInit() {
      this.displayDialog = true;
    }

    DialogOk() {

      this.confirmDialogResult.emitter.emit(this.confirmDialogBinders);
      this.displayDialog = false;
    }

    DialogCancel() {
      this.confirmDialogResult.emitter.emit(null);
      this.displayDialog = false;
    }

}

