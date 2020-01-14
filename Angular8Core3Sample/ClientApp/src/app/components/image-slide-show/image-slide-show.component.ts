import { Component, Input, OnChanges, SimpleChanges, OnInit, EventEmitter } from '@angular/core';
import { slideShowImage } from './../../interfaces/Home/slideShowImage';
import { trigger, transition, style, animate, state } from '@angular/animations';
import { Observable, Subscription } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-image-slide-show',
  templateUrl: './image-slide-show.component.html',
  styleUrls: ['./image-slide-show.component.css'],
  animations: [

    trigger('slideImage', [

      state('slideRight', style({
        transform: 'translateX(100%)'
        })
      ),

      state('previousImage', style({
        transform: 'translateX(0%)'
      })
      ),

      state('slideLeft', style({
        transform: 'translateX(-100%)'
        })
      ),

      state('nextImage', style({
        transform: 'translateX(0)'
        })
      ),

      state('slideCurrent', style({
        transform: 'translateX(0%)',
        opacity: '100%'
        })
      ),

      state('currentImage', style({
        transform: 'translateX(40%)',
        opacity: '10%'
        })
      ),

      transition('nextImage => slideLeft', [
        animate('1s')
      ]),

      transition('slideLeft => nextImage', [
        animate('0s')
      ]),

      transition('previousImage => slideRight', [
        animate('1s')
      ]),

      transition('slideRight => previousImage', [
        animate('0s')
      ]),

      transition('* => currentImage', [
        animate('0s')
      ]),

      transition('currentImage => slideCurrent', [
        animate('500ms')
      ])

    ])

  ]
})


/** imageSlideShow component*/
export class ImageSlideShowComponent implements OnInit, OnChanges{

  @Input()
  images!: slideShowImage[];

  thumbnailImages!: slideShowImage[];

  previousIndex!: number;
  currentIndex!: number;
  nextIndex!: number;

  showButtons!: boolean;

  disableLightBoxButtons!: boolean;

  slidePreviousImage!: boolean;

  slideNextImage!: boolean;

  slideNextLightBoxImage!: boolean;

  slidePreviousLightBoxImage!: boolean;

  slideNextLightBoxImageEventEmitter!: EventEmitter<boolean>;

  slidePreviousLightBoxImageEventEmitter!: EventEmitter<boolean>;

  slideNextLightBoxImageSubscription!: Subscription;

  slidePreviousLightBoxImageSubscription!: Subscription;

  slideCurrentImage!: boolean;

  slideShowTimer!: Observable<number>;

  slideShowSubscription!: Subscription;

  lightBoxSlideShowSubscription!: Subscription;

  lightBox!: boolean;

  
  /** imageSlideShow ctor */
  constructor(private sanitizer: DomSanitizer) {
    
  }


  ngOnInit() {
    this.slidePreviousImage = false;
    this.slideNextImage = false;
    this.slideCurrentImage = false;
    this.slideNextLightBoxImageEventEmitter = new EventEmitter();
    this.slideNextLightBoxImageEventEmitter.subscribe(value => this.slideNextLightBoxImage = value);
    this.slideNextLightBoxImageEventEmitter.emit(false);
    this.slidePreviousLightBoxImageEventEmitter = new EventEmitter();
    this.slidePreviousLightBoxImageEventEmitter.subscribe(value => this.slidePreviousLightBoxImage = value);
    this.slidePreviousLightBoxImageEventEmitter.emit(false);
    this.disableLightBoxButtons = false;
    this.previousIndex = 0;
    this.currentIndex = 0;
    this.nextIndex = 0;
  }


  ngOnChanges(changes: SimpleChanges) {
    if (changes.images && changes.images.currentValue && changes.images.currentValue.length > 0) {
      this.resetSlideShowTimer();
    }
  }


  unsubscribeSlideShowTimer() {
    if (this.slideShowSubscription) {
      this.slideShowSubscription.unsubscribe();
    }
  }


  resetSlideShowTimer() {
    this.slideShowTimer = Observable.timer(7000, 7000);
    this.slideShowSubscription = this.slideShowTimer.subscribe(x => this.nextImage());
  }


  currentImageLoaded() {
    this.slideCurrentImage = true;
    this.slideNextImage = false;
    this.slidePreviousImage = false;
    this.showButtons = true;
  }


  nextImage() {
    this.slideNextImage = true;
    this.showButtons = false;
  }


  previousImage() {
    this.slidePreviousImage = true;
    this.showButtons = false;
  }


  leftButtonClick() {
    this.previousImage();
    this.unsubscribeSlideShowTimer();
  }


  rightButtonClick() {
    this.nextImage();
    this.unsubscribeSlideShowTimer();
  }


  playSlideShowBtn() {
    this.unsubscribeSlideShowTimer();
    this.resetSlideShowTimer();
  }


  pauseSlideShowBtn() {
    this.unsubscribeSlideShowTimer();
  }


  slideNextImageDone(event: any) {
    if (event.toState === "nextImage") {
      if (this.currentIndex < this.images.length - 1) {
        this.nextIndex = this.currentIndex + 1;
      }
      else {
        this.nextIndex = 0;
      }
    }
    else if (event.toState === "slideLeft") {
      if (this.nextIndex > 0) {
        this.previousIndex = this.nextIndex - 1;
      }
      else {
        this.previousIndex = this.images.length - 1;
      }
      this.currentIndex = this.nextIndex;
    }
  }
  

  slidePreviousImageDone(event: any) {
    if (event.toState === "previousImage") {
      if (this.currentIndex > 0) {
        this.previousIndex = this.currentIndex - 1;
      }
      else {
        this.previousIndex = this.images.length - 1;
      }
    }
    else if (event.toState === "slideRight") {
      if (this.previousIndex === this.images.length - 1) {
          this.nextIndex = 0;
      }
      else {
          this.nextIndex = this.previousIndex + 1;
      }
      this.currentIndex = this.previousIndex;
    }
  }


  showLightBox() {
    this.unsubscribeSlideShowTimer();
    this.setThumbnailImages();
    this.lightBox = true;
    this.resetLightBoxSubscription();
  }


  setThumbnailImages() {
    this.thumbnailImages = this.images.filter((x, index) => {
      if (this.currentIndex <= 5 && index <= 8) {
        return true;
      }
      else if (this.currentIndex > 5 && this.currentIndex <= (this.images.length - 5) &&
        index > (this.currentIndex - 5) && index < (this.currentIndex + 5)) {
        return true;
      }
      else if (this.currentIndex > (this.images.length - 5) && index > (this.images.length - 10) &&
        index < this.images.length) {
        return true;
      }
      return false;
    });
  }


  resetLightBoxSubscription() {
    this.slideShowTimer = Observable.timer(7000, 7000);
    this.lightBoxSlideShowSubscription = this.slideShowTimer.subscribe(x => this.nextLightBoxImage());
  }


  unsubscribeLightBoxSubscription() {
    if (this.lightBoxSlideShowSubscription) {
      this.lightBoxSlideShowSubscription.unsubscribe();
    }
  }


  nextLightBoxImage() {
    this.slideNextLightBoxImageEventEmitter.emit(true);
  }

  previousLightBoxImage() {
    this.slidePreviousLightBoxImageEventEmitter.emit(true);
  }


  playLightBoxSlideShowBtn() {
    this.unsubscribeLightBoxSubscription();
    this.resetLightBoxSubscription();
  }


  pauseLightBoxSlideShowBtn() {;
    this.unsubscribeLightBoxSubscription();
  }


  hideLightBox() {
    this.lightBox = false;
    this.unsubscribeLightBoxSubscription();
    this.resetSlideShowTimer();
  }


  setNextImage(image: slideShowImage) {
    let nextIndex = this.images.indexOf(image);

    if (this.slidePreviousLightBoxImageSubscription) {
        this.slidePreviousLightBoxImageSubscription.unsubscribe();
    }

    if (this.slideNextLightBoxImageSubscription) {
        this.slideNextLightBoxImageSubscription.unsubscribe();
    }

    if (nextIndex < this.currentIndex) {
      
      this.previousIndex = nextIndex;
      this.slidePreviousLightBoxImageEventEmitter.emit(true);
      this.disableLightBoxButtons = true;
      this.unsubscribeLightBoxSubscription();  
    }
    else if (nextIndex > this.currentIndex) {
      
      this.nextIndex = nextIndex;
      this.slideNextLightBoxImageEventEmitter.emit(true);
      this.disableLightBoxButtons = true;
      this.unsubscribeLightBoxSubscription();
    }
  }


  thumbnailButtonClick(image: slideShowImage) {

    this.currentLightBoxImageLoaded();

    if (this.slideNextLightBoxImage) {
        this.slideNextLightBoxImageSubscription = this.slideNextLightBoxImageEventEmitter
            .subscribe(value => {
                if (!value) {
                    this.setNextImage(image)
                }
            });
    }
    else if (this.slidePreviousLightBoxImage) {
        this.slidePreviousLightBoxImageSubscription = this.slidePreviousLightBoxImageEventEmitter
            .subscribe(value => {
                if (!value) {
                    this.setNextImage(image)
                }
            });
    }
    else {
      this.setNextImage(image);
    }
  }


  leftLightBoxButtonClick() {
    this.disableLightBoxButtons = true;
    this.previousLightBoxImage();
    this.setThumbnailImages();
    this.unsubscribeLightBoxSubscription();
  }


  rightLightBoxButtonClick() {
    this.disableLightBoxButtons = true;
    this.nextLightBoxImage();
    this.setThumbnailImages();
    this.unsubscribeLightBoxSubscription();
  }


  currentLightBoxImageLoaded() {
    this.slideNextLightBoxImageEventEmitter.emit(false);
    this.disableLightBoxButtons = false;
    this.slidePreviousLightBoxImageEventEmitter.emit(false);
    this.setThumbnailImages();
  }


  leftThumbnailButtonClick() {
    if (this.images.indexOf(this.thumbnailImages[0]) - 1 > -1) {
      this.thumbnailImages.pop();
      this.thumbnailImages.unshift(this.images[this.images.indexOf(this.thumbnailImages[0]) - 1]);
    }
  }


  rightThumbnailButtonClick() {
    if (this.images.indexOf(this.thumbnailImages[this.thumbnailImages.length - 1]) < this.images.length - 1) {
      this.thumbnailImages.splice(0, 1);
      this.thumbnailImages.push(this.images[this.images.indexOf(this.thumbnailImages[this.thumbnailImages.length - 1]) + 1]);
    }
  }

  getSafeHTML(html: string) {
    return this.sanitizer.bypassSecurityTrustHtml(html);
  }
}
