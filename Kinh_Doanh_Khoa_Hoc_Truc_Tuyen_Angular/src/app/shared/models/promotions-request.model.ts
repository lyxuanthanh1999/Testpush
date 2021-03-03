export class PromotionsRequest {
    id: number;
    name: string;
    fromDate: string;
    toDate: string;
    discountPercent: number;
    discountAmount: number;
    status: boolean;
    applyForAll: boolean;
    content: string;
}
