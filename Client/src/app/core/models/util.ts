export class ModelUtil {
  public DateToString(date: Date): string {
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    // console.log(`ModelUtil: year = ${year}  month = ${month}  day = ${day}`);
    const s = this.IntToString(year, 4) + '-' + this.IntToString(month, 2) + '-' + this.IntToString(day, 2);
    // console.log('ModelUtil: s = ', s);

    return s;
  }

  public StringToDate(s: string): Date {
    return new Date(s);
  }

  public IntToString(x: number, d?: number) {
    let s = x.toString();
    // console.log('x =', x, '   d =', d, 's =', s);
    if ( d ) {
      for ( let i = s.length; i < d; i++ ) {
        s = '0' + s;
      }
    }
    return s;
  }
}
