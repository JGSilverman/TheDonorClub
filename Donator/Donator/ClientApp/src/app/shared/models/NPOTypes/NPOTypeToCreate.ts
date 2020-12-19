export class NPOTypeToCreate {
  name: string;
  taxCodeIdentifier: string;

  constructor(name: string, taxCode: string) {
    this.name = name;
    this.taxCodeIdentifier = taxCode;
  }
}
