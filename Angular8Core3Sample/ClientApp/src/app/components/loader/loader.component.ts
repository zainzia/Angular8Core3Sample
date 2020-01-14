import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { LoaderService } from '../../services/loader.service';
import { LoaderState } from '../../interfaces/Home/loader.state';


@Component({
    selector: 'angular-loader',
    templateUrl: 'loader.component.html',
    styleUrls: ['loader.component.css']
})

export class LoaderComponent implements OnInit {

  show = false;

  private subscription: Subscription;

  constructor(private loaderService: LoaderService) {
  }

  
  ngOnInit() { 
    this.subscription = this.loaderService.loaderState
        .subscribe((state: LoaderState) => {
            this.show = state.show;
        });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}

