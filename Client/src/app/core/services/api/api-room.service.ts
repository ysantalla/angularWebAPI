import { ApiCrudService } from './api-crud.service';
import { Room, RoomFilter } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiRoomService extends ApiCrudService<Room, RoomFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/rooms';
  }
}
