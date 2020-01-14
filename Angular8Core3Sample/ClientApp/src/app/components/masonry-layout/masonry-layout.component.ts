
import { Component, Input, OnInit, AfterContentChecked, HostListener } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

import { MasonryColumn } from './../../interfaces/Home/MasonryColumn';

@Component({
    selector: 'app-masonry-layout',
    templateUrl: './masonry-layout.component.html',
    styleUrls: ['./masonry-layout.component.css']
})

/** masonry-layout component*/
export class MasonryLayoutComponent implements OnInit {


  @Input()
  columns!: MasonryColumn[];

  grid!: Element;

  constructor(private sanitizer: DomSanitizer) {

  }


  ngOnInit() {
   
    this.grid = document.querySelectorAll('.masonry-grid')[0];

  }


  @HostListener('window:resize', ['$event'])
  onResize(event) {
    console.log(event.target.innerWidth);

    this.reorganizeColumns();
  }


  resizeGridColumn(item: Element) {

    let newHeight = item.querySelector('.content').getBoundingClientRect().height;

    let rowHeight = parseInt(window.getComputedStyle(this.grid).getPropertyValue('grid-auto-rows'));

    let rowGap = parseInt(window.getComputedStyle(this.grid).getPropertyValue('grid-row-gap'));

    let rowSpan = Math.ceil( newHeight / ( rowHeight + rowGap ) );

    let htmlElement = item as HTMLElement;

    htmlElement.style.gridRowEnd = "span " + rowSpan;

  }


  reorganizeColumns() {

    if (!this.columns || this.columns.length < 1) {
      return;
    }

    var gridColumns = document.querySelectorAll('.masonry-item');

    for (let i = 0; i < gridColumns.length; i++) {
      this.resizeGridColumn(gridColumns[i] as Element);
    }

  }
  

  getSafeHTML(html: string) {

    return this.sanitizer.bypassSecurityTrustHtml(html);

  }


  imageLoaded() {

    this.reorganizeColumns();

  }


}
