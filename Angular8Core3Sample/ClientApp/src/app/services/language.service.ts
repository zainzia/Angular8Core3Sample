import { Injectable } from '@angular/core';
import { NodeService } from './node.service';
import { JsonNetDecycleModule } from '../helpers/json-net-decycle.module';

@Injectable()
export class LanguageService {

  decycle = new JsonNetDecycleModule();

  constructor(private nodeService: NodeService) {

  }

  getLanguage(id: number) {
    return this.nodeService.fetchURL(`API/Common/Languages/${id}`).then((language) => { return language; });
  }

  getAllLanguages() {
    return this.nodeService.fetchURL(`API/Common/Languages/GetAll`).then((languages) => { return this.decycle.retrocycle(languages); });
  }
}
