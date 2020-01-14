import { Injectable, TemplateRef } from '@angular/core';
import { MyListDialogType } from './myList/MyListsDialogType';

@Injectable()
export class ConfirmDialogParams {

  DialogHeader: string;
  DialogBody: TemplateRef<any>;
  DialogIcon: string;
  DialogButton1Icon: string;
  DialogButton2Icon: string;
  DialogButton1Label: string;
  DialogButton2Label: string;
  opType: MyListDialogType;

}
