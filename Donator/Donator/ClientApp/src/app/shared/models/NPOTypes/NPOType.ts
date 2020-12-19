export class NPOType {
  id: number;
  name: string;
  taxCodeIdentifier: string;

  constructor(id: number, name: string, taxCode: string) {
    this.id = id;
    this.name = name;
    this.taxCodeIdentifier = taxCode;
  }
}
