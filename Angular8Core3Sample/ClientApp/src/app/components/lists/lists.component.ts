
import { Component, OnInit, Inject, Injector, EventEmitter, ViewChild, TemplateRef } from '@angular/core';

import { MenuItem } from 'primeng/api';

import { myList } from './../../interfaces/Home/myList/myList';

import { MyListsService } from './../../services/myLists.service';

import { MyListDialogType } from './../../interfaces/Home/myList/MyListsDialogType';

import { AppConfig } from '../../interfaces/home/app-config.module';

import { MessageService } from 'primeng/api';

import { ConfirmDialogComponent } from './../confirm-dialog/confirm-dialog.component';

import { ConfirmDialogParams } from './../../interfaces/Home/ConfirmDialogParams';

import { ConfirmDialogResult } from './../../interfaces/Home/ConfirmDialogResult';

import { ConfirmDialogBinders } from '../../interfaces/Home/ConfirmDialogBinders';

import { MoveMyListItem } from '../../interfaces/Home/myList/MoveMyListItem';
import { AppConfigService } from '../../services/appConfig.service';
import { Country } from '../../interfaces/Home/country.module';
import { CountryService } from '../../services/country.service';
import { BreadcrumbService } from '../../services/breadcrumb.service';


@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css']
})


/** lists component*/
export class ListsComponent implements OnInit {


    @ViewChild('AddListDialogTemplate', { static: true }) AddListDialogTemplate: TemplateRef<any>;

    @ViewChild('DeleteItemDialogTemplate', { static: true }) DeleteItemDialogTemplate: TemplateRef<any>;

    @ViewChild('DeleteListDialogTemplate', { static: true }) DeleteListDialogTemplate: TemplateRef<any>;

    @ViewChild('MoveItemDialogTemplate', { static: true }) MoveItemDialogTemplate: TemplateRef<any>;

    @ViewChild('ErrorDialogTemplate', { static: true }) ErrorDialogTemplate: TemplateRef<any>;

    ConfirmDialogComponent = ConfirmDialogComponent;

    confirmDialogInjector: Injector;

    displayDialog!: boolean;

    currentIndex!: number;

    myLists!: myList[];

    menuItems: MenuItem[];

    productId!: number;

    myListId!: number;

    myListName!: string;

    opType!: MyListDialogType;

    moveToSelectedListId!: number;

    confirmDialogParams!: ConfirmDialogParams;

    confirmDialogResult!: ConfirmDialogResult;

    confirmDialogBinders!: ConfirmDialogBinders;

    country!: Country;


    /** lists ctor */
    constructor(private myListsService: MyListsService,
                private messageService: MessageService,
                private injector: Injector,
                private countryService: CountryService,
                private breadcrumbService: BreadcrumbService,
                private appConfigService: AppConfigService) {

    }


    ngOnInit() {

        this.currentIndex = 0;
        this.menuItems = <MenuItem[]>[];

        this.GetMyLists();
    }


    get MyListDialogType() {
        return MyListDialogType;
    }


    GetMyLists() {

        let appConfig = this.appConfigService.getAppConfig();

        this.countryService.getCountry(this.appConfigService.getAppConfig().CountryID).then((country) => {
            this.country = country;
            this.myListsService.getMyLists(appConfig.LanguageID, appConfig.LanguageID)
                .then((myLists) => {
                    this.myLists = myLists;
                    this.displayDialog = false;

                    let items = <MenuItem[]>[];
                    let item = <MenuItem>{
                        label: this.myLists[0].myListName,
                        icon: 'pi pi-star-o',
                        queryParams: {
                            'myListIndex': 0
                        },
                        command: ((event) => {
                            this.currentIndex = event.item.queryParams['myListIndex'];
                        })
                    };
                    items.push(item);

                    for (let i = 1; i < this.myLists.length; i++) {
                        items.push(<MenuItem>{
                            label: this.myLists[i].myListName,
                            icon: 'pi pi-list',
                            queryParams: {
                                'myListIndex': i
                            },
                            command: ((event) => {
                                this.currentIndex = event.item.queryParams['myListIndex'];
                            })
                        });
                    }

                    this.menuItems = <MenuItem[]>[{
                        label: 'My Lists',
                        items: items
                    }];

                    this.breadcrumbService.setCrumbs([{
                        label: 'My Account',
                        routerLink: '/MyAccount/' + this.myLists[0].userAccountId
                    },
                    {
                        label: 'My Lists',
                        routerLink: '/MyLists'
                    }]);

                }).catch((exception: string) => {
                    this.CreateDialog(MyListDialogType.Error);
                });
        }).catch((error) => {
            this.CreateDialog(MyListDialogType.Error);
        });
    }


    DeleteItem(productId: number, myListId: number) {
        this.myListsService.deleteMyListItem(productId, myListId).then((result) => {
            if (result) {
                this.messageService.add({ severity: 'success', summary: 'Item Deleted!', life: 5000 });
                this.GetMyLists();
            }
        }).catch((exception: string) => {
            this.messageService.add({ severity: 'error', summary: 'Item not Deleted!', detail: 'Please try again later.', life: 5000 });
        });
    }


    MoveItem(productId: number, originalMyListId: number, moveToMyListId: number) {

        let moveMyListItem = <MoveMyListItem>{
            ProductId: productId,
            MoveToMyListId: moveToMyListId,
            OriginalMyListId: originalMyListId
        };

        this.myListsService.moveMyListItem(moveMyListItem).then((result) => {
            if (result) {
                this.messageService.add({ severity: 'success', summary: 'Item Moved!', life: 5000 });
                this.GetMyLists();
            }
        }).catch((exception: string) => {
            this.messageService.add({ severity: 'error', summary: 'Item not Moved!', detail: 'Please try again later.', life: 5000 });
        });
    }


    CreateMyList(name: string) {
        this.myListsService.createMyList(name).then((result) => {
            if (result) {
                this.messageService.add({ severity: 'success', summary: 'List Created!' });
                this.GetMyLists();
            }
        }).catch((exception: string) => {
            this.messageService.add({ severity: 'error', summary: 'List not Created!', detail: 'Please try again later.', life: 5000 });
        });
    }


    DeleteMyList(myListId: number) {
      if (this.currentIndex > 0) {
          this.myListsService.deleteMyList(myListId).then((result) => {
              if (result) {
                  this.messageService.add({ severity: 'success', summary: 'List Deleted!', life: 5000 });
                  this.currentIndex--;
                  this.GetMyLists();
              }
          }).catch((exception: string) => {
              this.messageService.add({ severity: 'error', summary: 'List not Deleted!', detail: 'Please try again later.', life: 5000 });
          });
      }
      else {
          this.messageService.add({ severity: 'error', summary: 'List not Deleted!', detail: 'You cannot delete this List!', life: 5000 });
      }  
    }


    DeleteItemButtonClick(productId: number) {
        this.productId = productId;
        this.CreateDialog(MyListDialogType.DeleteItem)
    }


    MoveItemButtonClick(productId: number) {
        this.productId = productId;
        this.CreateDialog(MyListDialogType.MoveItem)
    }


    DeleteMyListButtonClick(myListId: number) {
        this.myListId = myListId;
        this.CreateDialog(MyListDialogType.DeleteList)
    }


    CreateDialog(dialogType: MyListDialogType) {
        this.opType = dialogType;

        switch (dialogType) {
            case MyListDialogType.AddList:
                this.confirmDialogParams = <ConfirmDialogParams>{ 
                    DialogHeader: 'Add New List',
                    DialogIcon: 'pi pi-plus-circle',
                    DialogButton1Icon: 'pi pi-check',
                    DialogButton2Icon: 'pi pi-times',
                    DialogButton1Label: 'Add',
                    DialogButton2Label: 'Cancel',
                    DialogBody: this.AddListDialogTemplate,
                    opType: dialogType
                }
                break;

            case MyListDialogType.DeleteItem:
                this.confirmDialogParams = <ConfirmDialogParams>{
                    DialogHeader: 'Delete Item',
                    DialogIcon: 'pi pi-minus-circle',
                    DialogButton1Icon: 'pi pi-check',
                    DialogButton2Icon: 'pi pi-times',
                    DialogButton1Label: 'Delete',
                    DialogButton2Label: 'Cancel',
                    DialogBody: this.DeleteItemDialogTemplate,
                    opType: dialogType
                }
                break;

            case MyListDialogType.DeleteList:
                this.confirmDialogParams = <ConfirmDialogParams>{
                    DialogHeader: 'Delete List',
                    DialogIcon: 'pi pi-minus-circle',
                    DialogButton1Icon: 'pi pi-check',
                    DialogButton2Icon: 'pi pi-times',
                    DialogButton1Label: 'Delete',
                    DialogButton2Label: 'Cancel',
                    DialogBody: this.DeleteListDialogTemplate,
                    opType: dialogType
                }
                break;

            case MyListDialogType.MoveItem:
                this.confirmDialogParams = <ConfirmDialogParams>{
                    DialogHeader: 'Move List',
                    DialogIcon: 'pi pi-pencil',
                    DialogButton1Icon: 'pi pi-check',
                    DialogButton2Icon: 'pi pi-times',
                    DialogButton1Label: 'Move',
                    DialogButton2Label: 'Cancel',
                    DialogBody: this.MoveItemDialogTemplate,
                    opType: dialogType
                }
                break;

            case MyListDialogType.Error:
                this.confirmDialogParams = <ConfirmDialogParams>{
                    DialogHeader: 'Error',
                    DialogIcon: 'pi pi-exclamation-triangle',
                    DialogButton1Icon: 'pi pi-check',
                    DialogButton1Label: 'OK',
                    DialogBody: this.ErrorDialogTemplate,
                    opType: dialogType
                }
                break;
        }

        this.confirmDialogResult = new ConfirmDialogResult();
        this.confirmDialogResult.emitter = new EventEmitter<any>();
        this.confirmDialogResult.emitter.subscribe((item) => this.ConfirmDialogResultEvent(item) );

        this.confirmDialogBinders = <ConfirmDialogBinders>{
            binders: {
                myListName: "",
                moveToSelectedList: null
            }
        };

        this.confirmDialogInjector = Injector.create({
            providers: [{
                provide: ConfirmDialogParams,
                useValue: this.confirmDialogParams
            },
            {
                provide: ConfirmDialogBinders,
                useValue: this.confirmDialogBinders
            },
            {
                provide: ConfirmDialogResult,
                useValue: this.confirmDialogResult
            }],
            parent: this.injector
        });
    }


    ConfirmDialogResultEvent(result: any) {

        if (result) {
            switch (this.opType) {
                case MyListDialogType.AddList:
                    this.CreateMyList(result.binders.myListName);
                    break;
                case MyListDialogType.DeleteItem:
                    this.DeleteItem(this.productId, this.myLists[this.currentIndex].myListId);
                    break;
                case MyListDialogType.DeleteList:
                    this.DeleteMyList(this.myLists[this.currentIndex].myListId);
                    break;
                case MyListDialogType.MoveItem:
                    this.MoveItem(this.productId, this.myLists[this.currentIndex].myListId, result.binders.moveToSelectedList.myListId);
                    break;
                default:
                    break;
            }
        }
    }

    getFormattedPrice(num: number) {
        if (num) {
            return num.toLocaleString('en-US', { style: 'currency', currency: this.country.Currency });
        }

        return "";
    }

}
