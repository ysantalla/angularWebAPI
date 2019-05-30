import { HttpParams } from '@angular/common/http';
import { isNullOrUndefined } from 'util';

export class OrderBy {
  constructor(public by: string,
              public desc: boolean) {}

  public SetHttpParams(hp: HttpParams): HttpParams {
    if (!isNullOrUndefined(this.by) && this.by !== '') {
      hp = hp.set('orderBy.by', this.by);
    }
    if (!isNullOrUndefined(this.desc) ) {
      hp = hp.set('orderBy.desc', this.desc.toString());
    }
    return hp;
  }
}
