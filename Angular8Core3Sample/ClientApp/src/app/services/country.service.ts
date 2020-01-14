
import { Injectable } from '@angular/core';
import { NodeService } from './node.service';
import { JsonNetDecycleModule } from '../helpers/json-net-decycle.module';


@Injectable()
export class CountryService {


  decycle = new JsonNetDecycleModule();


  constructor(private nodeService: NodeService) {}


  getCountry(id: number) {
    return this.nodeService.fetchURL(`API/Common/Countries/${id}`).then((country) => { return country; });
  }


  getAllCountries() {
    return this.nodeService.fetchURL(`API/Common/Countries/All`).then((countries) => { return this.decycle.retrocycle(countries); });
  }


  getAllProvinces() {
    return this.nodeService.fetchURL(`API/Common/Countries/Provinces`).then((provinces) => { return this.decycle.retrocycle(provinces); });
  }


  getAllStates() {
      return this.nodeService.fetchURL(`API/Common/Countries/States`).then((states) => { return this.decycle.retrocycle(states); });
  }

}
