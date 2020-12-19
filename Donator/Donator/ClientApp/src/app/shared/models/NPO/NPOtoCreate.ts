export class NPOToCreate {
  name: string;
  typeId: number;
  description: string;
  websiteUrl: string;
  createdByUserId: string;
  taxId: string;

  constructor(name: string, typeId: number, desc: string, url: string, userId: string, taxId: string) {
    this.name = name;
    this.typeId = typeId;
    this.description = desc;
    this.websiteUrl = url;
    this.createdByUserId = userId;
    this.taxId = taxId;
  }
}
