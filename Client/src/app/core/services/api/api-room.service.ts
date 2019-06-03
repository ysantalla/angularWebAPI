import { Room, RoomFilter } from '@app/core/models/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiCrudService } from './api-crud.service';

@Injectable({
  providedIn: 'root'
})
export class ApiRoomService extends ApiCrudService<Room, RoomFilter> {
  constructor(http: HttpClient) {
    super(http);
    this.url += '/rooms';
  }
}
