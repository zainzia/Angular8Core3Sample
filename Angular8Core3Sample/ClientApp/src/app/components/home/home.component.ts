import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from '../../services/breadcrumb.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

    constructor(private breadcrumbService: BreadcrumbService) {}

    ngOnInit() {
        this.breadcrumbService.setCrumbs(null);
    }

}
