import { HttpClient } from '@angular/common/http';
import { IApiListService, ApiListService } from './api-list.service';
import { Injectable } from '@angular/core';
import { FreeRoom, FreeRoomFilter } from '@app/core/models/core';


@Injectable({
  providedIn: 'root'
})
export class ApiFreeRoomService extends ApiListService<FreeRoom, FreeRoomFilter>
                  implements IApiListService<FreeRoom, FreeRoomFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/rooms/free';
  }
}
