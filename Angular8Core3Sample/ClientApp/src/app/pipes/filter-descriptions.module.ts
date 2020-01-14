import { Pipe, PipeTransform } from '@angular/core';
import { ProductDescription } from '../interfaces/product.module';

@Pipe({
    name: 'FilterProductDescriptionsByLanguage'
})
export class FilterProductDescriptionsByLanguagePipe implements PipeTransform {

    transform(productDescriptions: ProductDescription[], languageID: number): ProductDescription[] {
        if (!productDescriptions || !languageID) {
            return productDescriptions;
        }
        // To search values only of "name" variable of your object(item)
        //return items.filter(item => item.name.toLowerCase().indexOf(filter.toLowerCase()) !== -1);

        // To search in values of every variable of your object(item)
        return productDescriptions.filter(item => {
            if (item!.Language!.LanguageID == languageID) {
                return true;
            }
        });
    }
}
